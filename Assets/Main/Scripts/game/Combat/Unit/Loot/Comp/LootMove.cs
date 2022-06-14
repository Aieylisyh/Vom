using UnityEngine;
using com;

namespace game
{
    public class LootMove : UnitMove
    {
        public float dropTime;
        private float _dropTimer;
        public float dropTimeRandomnizer = 0.5f;

        public float waitTime;
        public float waitTimeRandomnizer = 0.5f;
        private float _waitTimer;

        public float absorbAcc;
        public float absorbSpeedMax;
        private float _absorbSpeed;

        public float rotationSpeed;
        private float _rotationSpeedX;
        private float _rotationSpeedY;
        private float _rotationSpeedZ;
        public float gravity;
        public float dropSpread = 2f;
        public int dropIndex;

        public float absorbRange = 0.8f;
        //private static Vector3 targetPos = Vector3.zero;

        public override void ResetState()
        {
            base.ResetState();
            _dropTimer = dropTime + Random.Range(0, dropTimeRandomnizer); ;
            _waitTimer = 0;

            float r = Random.value;
            Vector3 tempDir = Vector3.up;
            tempDir += Vector3.right * (1f - r * 2f) * dropSpread * dropIndex;

            _rotationSpeedX = (Random.value < 0.5f) ? rotationSpeed : -rotationSpeed;
            _rotationSpeedY = (Random.value < 0.5f) ? rotationSpeed : -rotationSpeed;
            _rotationSpeedZ = (Random.value < 0.5f) ? rotationSpeed : -rotationSpeed;

            tempDir.Normalize();
            SetDir(tempDir * Speed);

            //SetTargetPos();
        }

        private void SetTargetPos()
        {
            //if (targetPos == Vector3.zero)
            //{
            //    var targetTrans = LevelHudBehaviour.instance.pauseButton.transform;
            //    var canvasScale = (float)Screen.width / 720;
            //    targetPos = Convert2DAnd3D.GetWorldPosition(CameraController.instance.combatCam, targetTrans.position, canvasScale);
            //    targetPos.z = -3;
            //    targetPos.y += 0.8f;
            //    targetPos.x += 0.4f;
            //}
        }

        void Rotate(float dt)
        {
            transform.Rotate(new Vector3(_rotationSpeedX * dt, _rotationSpeedY * dt, _rotationSpeedZ * dt));
        }

        private void Drop()
        {
            //var dt = com.GameTime.deltaTime;
            var dt = Time.fixedDeltaTime;
            _dropTimer -= dt;
            if (_dropTimer <= 0)
            {
                _dropTimer = 0;
                _waitTimer = waitTime + Random.Range(0, waitTimeRandomnizer);
            }

            dir += gravity * dt * Vector3.down;
            transform.position += dir * dt;
            Rotate(dt);
        }

        private void Wait()
        {
           // var dt = com.GameTime.deltaTime;
            var dt = Time.fixedDeltaTime;
            _waitTimer -= dt;
            if (_waitTimer <= 0)
            {
                _waitTimer = 0;
                _absorbSpeed = 0;
            }
            Rotate(dt);
        }

        private void Absorb()
        {
            //var dt = com.GameTime.deltaTime;
            var dt = Time.fixedDeltaTime;
            if (_absorbSpeed < absorbSpeedMax)
            {
                _absorbSpeed += absorbAcc * dt;
            }

            var targetPos = CombatService.instance.playerShip.move.transform.position;
            var absorbDir = targetPos - transform.position;
            if (absorbDir.magnitude < absorbRange)
            {
                self.Recycle();
                return;
            }

            transform.position += absorbDir.normalized * _absorbSpeed * dt;
            Rotate(dt);
        }

        protected override void Tick()
        {
            base.Tick();
            Move();
        }

        public override void Move()
        {
            if (_dropTimer > 0)
                Drop();
            else if (_waitTimer > 0)
                Wait();
            else
                Absorb();
        }
    }
}
