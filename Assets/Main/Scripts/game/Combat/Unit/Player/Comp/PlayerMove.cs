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
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundMin, boundMax), transform.position.y, transform.position.z);
            }
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
