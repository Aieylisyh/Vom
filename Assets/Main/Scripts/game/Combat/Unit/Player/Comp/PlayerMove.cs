using UnityEngine;
using com;
namespace game
{
    public class PlayerMove : UnitMove
    {
        public float boundMin;
        public float boundMax;

        private bool _isSinking = false;
        private bool _sinkRotateXDir;
        private float _sinkRotateZFactor;
        private float _sinkRotateYFactor;
        private float _sinkSpeed;
        private float _sinkRotateXSpeed;

        public FloatingBehaviour floatingBehaviour;

        protected override void Tick()
        {
            if (_isSinking)
            {
                Sink();
                return;
            }

            if (self.IsAlive())
            {
                floatingBehaviour.FloatMove();

                if (GameFlowService.instance.IsGameplayControlEnabled())
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundMin, boundMax), transform.position.y, transform.position.z);
                }
            }
        }

        public void StartSink()
        {
            if (_isSinking)
            {
                return;
            }

            var p = ConfigService.instance.combatConfig.playerSinkParam;
            _isSinking = true;
            _sinkSpeed = 0;
            _sinkRotateXSpeed = 0;
            _sinkRotateXDir = Random.value < 0.5f;
            _sinkRotateZFactor = Random.value < 0.5f ? p.rotateZFactor : -p.rotateZFactor;
            _sinkRotateYFactor = Random.value < 0.5f ? p.rotateYFactor : -p.rotateYFactor;
            var go = PoolingService.instance.GetInstance(p.effectId);
            go.transform.position = transform.position;
        }

        private void Sink()
        {
            var p = ConfigService.instance.combatConfig.playerSinkParam;
            _sinkSpeed += p.acc * com.GameTime.deltaTime;
            if (_sinkSpeed > p.speedMax)
            {
            }
            _sinkRotateXSpeed += p.rotateAcc * com.GameTime.deltaTime;
            if (_sinkRotateXSpeed > p.rotateXSpeedMax)
            {
                _sinkRotateXSpeed = p.rotateXSpeedMax;
            }
            transform.position += Vector3.down * _sinkSpeed * com.GameTime.deltaTime;
            var s = new Vector3(
                    _sinkRotateXDir ? _sinkRotateXSpeed : -_sinkRotateXSpeed,
              _sinkRotateYFactor, _sinkRotateZFactor);
            //Debug.Log("---> " + s);
            //Debug.Log(transform.eulerAngles);
            transform.localEulerAngles += s * com.GameTime.deltaTime;
        }

        public override void ResetState()
        {
            base.ResetState();
            _isSinking = false;
        }

        public override void MoveLeft()
        {
            base.MoveLeft();
            LevelTutorialService.instance.ProgressTuto_MoveLeft();
        }

        public override void MoveRight()
        {
            base.MoveRight();
            LevelTutorialService.instance.ProgressTuto_MoveRight();
        }
    }
}
