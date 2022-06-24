using UnityEngine;
using game;

namespace vom
{
    public class PlayerMoveBehaviour : VomUnitComponent
    {
        private bool _draging;
        private Vector2 _startPos;
        private Vector2 _lastPos;
        public float ignoreThreshold = 10;

        private Vector2 _moveDist;

        public float speed;
        public Transform rotatePart;

        public float rotationLerpFactor = 0.1f;

        public TouchViewBehaviour touchView;

        protected override void Start()
        {
            base.Start();
        }

        public void StartDrag(Vector2 pos)
        {
            _draging = true;
            _startPos = pos;
            _lastPos = pos;
        }

        public void UpdateDrag(Vector2 pos)
        {
            _lastPos = pos;
        }

        public void EndDrag()
        {
            _draging = false;
            touchView.Hide();
        }

        public void Moved()
        {
            // com.SoundService.instance.Play("step");
        }

        protected override void Update()
        {
            base.Update();

            if (_draging)
            {
                var delta = _lastPos - _startPos;
                if (delta.magnitude > ignoreThreshold)
                {
                    ReceiveMoveInput(delta);
                }
                touchView.Show(delta);
            }
        }

        public bool isMoving { get { return _moveDist.magnitude > 0f; } }

        public void Move()
        {
            if (_moveDist.magnitude == 0)
            {
                player.animator.SetBool("move", false);
                if (!player.cc.isGrounded)
                {
                    player.cc.SimpleMove(-8f * Vector3.up);
                }
            }
            else
            {
                player.animator.SetBool("move", true);
                var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
                player.cc.SimpleMove(deltaDist * speed);
                Rotate(deltaDist);
            }

            _moveDist = Vector2.zero;
        }

        public void Rotate(Vector3 to)
        {
            to.y = 0;
            rotatePart.rotation = Quaternion.LookRotation(to);
        }

        public void ReceiveMoveInput(Vector2 dir)
        {
            _moveDist = dir.normalized;
        }
    }
}