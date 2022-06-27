using UnityEngine;
using game;

namespace vom
{
    public class EnemyMoveBehaviour : VomEnemyComponent
    {
        private Vector3 _startPos;

        private Vector2 _moveDist;

        public float speed;
        public float runSpeed = 15f;

        public Transform rotatePart;

        public AttackRange stopRange = AttackRange.Melee;
        float _fRange;

        protected override void Start()
        {
            base.Start();
            _startPos = transform.position;
            _fRange = CombatSystem.GetRange(stopRange);
        }

        public void Moved()
        {
            // com.SoundService.instance.Play("step");
        }

        public void Move()
        {
            if (host.attack.isAttacking)
            {
                return;
            }

            if (host.targetSearcher.alerted && host.targetSearcher.target != null)
            {
                if (host.targetSearcher.targetDist >= _fRange)
                {
                    SetMoveTo(host.targetSearcher.target.position);
                }

                PerformMove();
                return;
            }

            RunBack();
        }

        public bool isMoving { get { return _moveDist.magnitude > 0f; } }

        void RepositionDone()
        {
            if (host.animator.GetBool("move"))
                host.animator.SetBool("move", false);

            host.targetSearcher.RepositionDone();
        }

        void RunBack()
        {
            var dir = _startPos - transform.position;
            if (dir.magnitude < 0.5f)
            {
                RepositionDone();
                return;
            }

            SetMoveTo(_startPos);
            if (!host.animator.GetBool("move"))
                host.animator.SetBool("move", true);

            var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
            host.cc.SimpleMove(deltaDist * speed);//TODO maybe directly pass all obstacles transit
            Rotate(deltaDist);
        }

        void PerformMove()
        {
            if (_moveDist.magnitude == 0)
            {
                host.animator.SetBool("move", false);
                if (!host.cc.isGrounded)
                {
                    host.cc.SimpleMove(-8f * Vector3.up);
                }
            }
            else
            {
                host.animator.SetBool("move", true);
                var deltaDist = Vector3.right * _moveDist.x + Vector3.forward * _moveDist.y;
                host.cc.SimpleMove(deltaDist * runSpeed);
                Rotate(deltaDist);
            }

            _moveDist = Vector2.zero;
        }

        public void Rotate(Vector3 to)
        {
            to.y = 0;
            rotatePart.rotation = Quaternion.LookRotation(to);
        }

        public void SetMoveTo(Vector3 targetPos)
        {
            var to = targetPos - transform.position;
            to.y = 0;
            _moveDist = to.normalized;
        }
    }
}