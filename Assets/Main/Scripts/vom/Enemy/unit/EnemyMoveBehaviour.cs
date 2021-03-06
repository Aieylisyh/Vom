using UnityEngine;

namespace vom
{
    public class EnemyMoveBehaviour : VomEnemyComponent
    {
        private Vector3 _startPos;
        private Vector3 _moveDist;

        public float speed { get; private set; }
        public float runSpeed { get; private set; }

        public Transform rotatePart;

        public AttackRange stopRange = AttackRange.Melee;
        float _fRange;
        float _acc;
        bool _groundFound;
        float _crtSpeed;

        public KnockBackBehaviour knockBack { get; private set; }

        public override void ResetState()
        {
            speed = host.proto.speed;
            runSpeed = speed + 5f;
            _acc = speed * 0.5f;
            _crtSpeed = 0;

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

            if (host.health.isWounding)
                return;

            if (host.targetSearcher.alerted && host.targetSearcher.alertOrigin != null)
            {
                if (host.targetSearcher.targetDist >= _fRange)
                {
                    if (!host.cc.enabled)
                        host.cc.enabled = true;
                    SetMoveTo(host.targetSearcher.alertOrigin.position);
                }

                PerformMove();
                return;
            }

            RunBack();
        }

        public bool isMoving { get { return _moveDist.magnitude > 0f; } }

        public bool isRunningBack { get { return !host.cc.enabled; } }

        public void Stop()
        {
            if (host.animator.GetBool(EnemyAnimeParams.Move))
                host.animator.SetBool(EnemyAnimeParams.Move, false);

            _crtSpeed = 0;
        }

        public void WoundStop()
        {
            if (host.animator.GetBool(EnemyAnimeParams.Move))
                host.animator.SetBool(EnemyAnimeParams.Move, false);

            _crtSpeed *= 0.5f;
        }

        void RepositionDone()
        {
            //Debug.Log("RepositionDone");
            Stop();
            if (!host.cc.enabled)
            {
                host.cc.enabled = true;
                host.targetSearcher.RepositionDone();

                if (host.health.hp < host.health.hpMax)
                    host.health.ResetHp();
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
                Stop();
                Fall();
            }
            else
            {
                if (!host.animator.GetBool(EnemyAnimeParams.Move))
                    host.animator.SetBool(EnemyAnimeParams.Move, true);

                _crtSpeed += _acc * com.GameTime.deltaTime;
                if (_crtSpeed > speed)
                    _crtSpeed = speed;
                host.cc.SimpleMove(_moveDist * _crtSpeed);
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