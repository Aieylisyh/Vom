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

        public Vector3 lastEular { get; private set; }

        public float rotationLerpFactor = 0.1f;

        protected override void Start()
        {
            base.Start();
            lastEular = transform.forward;
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
            }
        }

        public bool isMoving { get { return _moveDist.magnitude > 0f; } }

        public void Move()
        {
            if (_moveDist.magnitude == 0)
            {
                player.animator.SetBool("move", false);
            }
            else
            {
                player.animator.SetBool("move", true);
                var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
                player.cc.SimpleMove(deltaDist * speed);
                lastEular = deltaDist;
            }

            _moveDist = Vector2.zero;

            Rotate();
        }

        public void Rotate()
        {
            if (isMoving)
                return;

            var dir = Vector3.Lerp(transform.forward, lastEular, rotationLerpFactor);
            transform.rotation = Quaternion.LookRotation(dir);
        }

        public void Rotate(Vector3 to)
        {
            transform.rotation = Quaternion.LookRotation(to);
            lastEular = to;
        }

        public void ReceiveMoveInput(Vector2 dir)
        {
            _moveDist = dir.normalized;
        }
    }
}