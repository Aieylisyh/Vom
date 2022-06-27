using UnityEngine;
using game;

namespace vom
{
    public class EnemyMoveBehaviour : VomEnemyComponent
    {
        private Vector3 _startPos;

        private Vector3 _moveDist;

        public float speed;
        public float runSpeed = 15f;

        public Transform rotatePart;

        public AttackRange stopRange = AttackRange.Melee;
        float _fRange;

        protected override void Start()
        {
            base.Start();

            _fRange = CombatSystem.GetRange(stopRange);
            _startPos = transform.position;
        }

        public void Moved()
        {
            // com.SoundService.instance.Play("step");
        }

        public void Move()
        {
            if (host.attack.isAttacking)
                return;

            if (host.targetSearcher.alerted && host.targetSearcher.target != null)
            {
                if (host.targetSearcher.targetDist >= _fRange)
                {
                    Debug.Log("mtp");
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

            if (!host.cc.enabled)
                host.cc.enabled = true;

            host.cc.SimpleMove(-1f * Vector3.up);
            host.targetSearcher.RepositionDone();
            if (host.health.hp < host.health.healthMax)
            {
                host.health.HealToFull();
            }
        }

        void RunBack()
        {
            if (host.cc.enabled)
                host.cc.enabled = false;

            var dir = _startPos - transform.position;
            var dist = dir.magnitude;
            if (dist < 0.5f)
            {
                RepositionDone();
                return;
            }

            SetMoveTo(_startPos);
            if (!host.animator.GetBool("move"))
                host.animator.SetBool("move", true);

            var s = _moveDist * runSpeed;
            if (s.magnitude > dist)
            {
                s = s.normalized * dist;
            }

            transform.position += s;
            Rotate(_moveDist);
        }

        void PerformMove()
        {
            if (_moveDist.magnitude == 0)
            {
                if (host.animator.GetBool("move"))
                    host.animator.SetBool("move", false);
                if (!host.cc.isGrounded)
                {
                    host.cc.SimpleMove(-1f * Vector3.up);
                }
            }
            else
            {
                if (!host.animator.GetBool("move"))
                    host.animator.SetBool("move", true);

                host.cc.SimpleMove(_moveDist * speed);
                Rotate(_moveDist);
            }

            _moveDist = Vector2.zero;
        }

        public void Rotate(Vector3 to)
        {
            to.y = 0;
            if (to.magnitude <= 0)
                return;
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