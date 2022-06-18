using UnityEngine;
using com;
using DG.Tweening;

namespace game
{
    public class EnemyMove : UnitMove
    {
        private float _sizeFactor;
        public bool hasFloat;
        public float floatFreq = 1;
        public float floatAmplitude = 1;
        protected float _floatOffsetTime;

        public bool goingRight { get; protected set; }
        private bool _spawnFromNear;
        private bool _spawnFromFar;
        private float _spawnHeight;

        protected float _showUpTime;
        protected float _showUpTimer;
        protected float _showUpSpeed;

        private bool _isSinking = false;
        private bool _sinkRotateXDir;
        private float _sinkRotateZFactor;
        private float _sinkRotateYFactor;
        private float _sinkSpeed;
        private float _sinkRotateXSpeed;

        private bool _simpleSpawn;
        protected bool _disableAlignDir;

        protected override void Tick()
        {
            base.Tick();
            if (_isSinking)
            {
                Sink();
                return;
            }

            Move();
            CheckDieBound();
        }

        private void CheckDieBound()
        {
            self.death.Die(true);
        }

        protected override void AlignDir(Vector3 d)
        {
            if (_disableAlignDir)
                return;

            base.AlignDir(d);
        }

        public void StartSink()
        {
        }

        private void Sink()
        {
        }

        public override void ResetState()
        {
            base.ResetState();
            _isSinking = false;
            transform.localEulerAngles = new Vector3(0, 90, 0);
            _disableAlignDir = false;

            transform.position = new Vector3(0, 90, 0);
        }

        public virtual void TurnBack(float duration)
        {
            _disableAlignDir = true;

            Vector3 v = new Vector3(0, goingRight ? 180 : 0, 0);
            rotateAlignMove.trans.DOLocalRotate(v, duration).OnComplete(() =>
            {
                dir = goingRight ? Vector3.right : Vector3.left;
                _disableAlignDir = false;
            });
            goingRight = !goingRight;
        }

        public void Teleport(bool pGoingRight, Vector3 targetPos)
        {
            transform.position = targetPos;
            goingRight = pGoingRight;
            _disableAlignDir = false;
            dir = goingRight ? Vector3.right : Vector3.left;
            //Vector3 v = new Vector3(0, goingRight ? 180 : 0, 0);
            //rotateAlignMove.trans.localEulerAngles = v;
        }

        public void SetSpawnPosition(float sizeFactor, bool goRight, float spawnHeight, bool spawnFromNear = false, bool spawnFromFar = false)
        {
            //Debug.Log("SetSpawnPosition");
            _simpleSpawn = false;
            _sizeFactor = sizeFactor;
            goingRight = goRight;
            _spawnHeight = spawnHeight;
            _spawnFromNear = spawnFromNear;
            _spawnFromFar = spawnFromFar;
        }

        public void SetSimpleSpawn()
        {
            _simpleSpawn = true;
        }

        public override void Move()
        {
            if (!self.IsAlive())
                return;

            if (_showUpTimer > 0)
            {
                ShowUp();
                return;
            }

            BasicMove();
        }

        protected virtual void BasicMove()
        {
            var pos = transform.position;
            pos.z = pos.z * 0.95f;
            transform.position = pos;
            shipFloat();
            Translate(dir);
        }

        protected virtual void ShowUp()
        {
            _showUpTimer -= com.GameTime.deltaTime;
            if (_showUpTimer < 0)
            {
                //Debug.Log("end show up " + transform.position);
                _showUpTimer = 0;
            }

            float t = _showUpTimer / _showUpTime;
            float s = Mathf.Lerp(0, _showUpSpeed, t);
            var TempDir = dir + new Vector3(0, 0, _spawnFromNear ? s : -s);
            //TempDir.Normalize();
            //SetDir(TempDir);
            shipFloat();
            Translate(TempDir.normalized);
        }

        protected void shipFloat()
        {
            if (hasFloat)
            {
                Vector3 floatHeight = Vector3.up;
                floatHeight *= com.GameTime.deltaTime * floatAmplitude * Mathf.Sin(com.GameTime.time * floatFreq - _floatOffsetTime);
                transform.position += floatHeight;
            }
        }
    }
}
