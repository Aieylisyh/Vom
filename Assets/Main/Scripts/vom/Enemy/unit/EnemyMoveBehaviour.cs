using UnityEngine;

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

        bool _groundFound;

        public KnockBackBehaviour knockBack { get; private set; }

        public override void ResetState()
        {
            knockBack = GetComponent<KnockBackBehaviour>();
            knockBack.setCc(host.cc);

            _fRange = CombatSystem.GetRange(stopRange);
            _startPos = transform.position;
            _groundFound = false;
        }

        public void Moved()
        {
            // com.SoundService.instance.Play("step");
        }

        void FindGround()
        {
            if (!host.cc.enabled)
                host.cc.enabled = true;
            //Debug.Log(transform.position);
            Fall();
            if (host.cc.isGrounded)
            {
                //Debug.LogWarning(transform.position);
                _groundFound = true;
                _startPos = transform.position;
            }
        }

        public void Move()
        {
            if (!host.targetSearcher.IsInPlayerView())
            {
                if (!_groundFound)
                    return;
                //Debug.Log("RunBack !IsInPlayerView");
                RunBack();
                return;
            }

            if (!_groundFound)
            {
                FindGround();
                return;
            }

            if (host.attack.isAttacking)
                return;

            if (host.targetSearcher.alerted && host.targetSearcher.target != null)
            {
                if (host.targetSearcher.targetDist >= _fRange)
                {
                    if (!host.cc.enabled)
                        host.cc.enabled = true;
                    SetMoveTo(host.targetSearcher.target.position);
                }

                PerformMove();
                return;
            }

            RunBack();
        }

        public bool isMoving { get { return _moveDist.magnitude > 0f; } }

        public bool isRunningBack { get { return !host.cc.enabled; } }

        void RepositionDone()
        {
            //Debug.Log("RepositionDone");
            if (host.animator.GetBool(EnemyAnimeParams.Move))
                host.animator.SetBool(EnemyAnimeParams.Move, false);

            if (!host.cc.enabled)
            {
                host.cc.enabled = true;
                host.targetSearcher.RepositionDone();

                if (host.health.hp < host.health.healthMax)
                {
                    host.health.ResetHealth();
                }
            }

            if (host.targetSearcher.IsInPlayerView())
            {
                //if has tile below
                //Debug.Log(host.targetSearcher.targetDist);
                Fall();
            }
        }

        void Fall()
        {
            //Debug.Log("Fall");
            if (!host.cc.isGrounded)
            {
                //Debug.Log("Fall11");
                host.cc.SimpleMove(-0.5f * Vector3.up);
            }
        }

        void RunBack()
        {
            if (host.cc.enabled)
                host.cc.enabled = false;

            var dir = _startPos - transform.position;
            var restDist = dir.magnitude;
            if (restDist < 0.5f)
            {
                //Debug.Log("RepositionDone");
                RepositionDone();
                return;
            }

            //Debug.Log("rb" + dir);
            SetMoveTo(_startPos);
            if (!host.animator.GetBool(EnemyAnimeParams.Move))
                host.animator.SetBool(EnemyAnimeParams.Move, true);

            var s = dir.normalized * runSpeed * com.GameTime.deltaTime;
            if (s.magnitude > restDist)
                s = s.normalized * restDist;

            transform.position += s;
            Rotate(dir);
        }

        void PerformMove()
        {
            if (_moveDist.magnitude == 0)
            {
                if (!host.animator.GetBool(EnemyAnimeParams.Move))
                    host.animator.SetBool(EnemyAnimeParams.Move, false);
                Fall();
            }
            else
            {
                if (!host.animator.GetBool(EnemyAnimeParams.Move))
                    host.animator.SetBool(EnemyAnimeParams.Move, true);
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

            //Debug.Log(to);
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