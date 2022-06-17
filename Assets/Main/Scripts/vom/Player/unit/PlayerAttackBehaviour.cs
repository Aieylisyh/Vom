using UnityEngine;
using com;

namespace vom
{
    public class PlayerAttackBehaviour : VomUnitComponent
    {
        public float attackInterval = 1;
        private float _attackIntervalTimer;

        public PlayerEnemySearcherBehaviour searcher;

        public PlayerOrbBehaviour orbs;

        protected override void Start()
        {
            base.Start();
            _attackIntervalTimer = 0;
        }

        public void Attack()
        {
            if (_attackIntervalTimer > 0)
            {
                _attackIntervalTimer -= GameTime.deltaTime;
            }

            if (player.move.isMoving)
                return;

            if (_attackIntervalTimer <= 0)
            {
                PerformAttack();
                if (_target != null)
                {
                    player.move.Rotate((_targetPos - transform.position).normalized);
                }
            }
        }

        private Transform _target;
        private Vector3 _targetPos;
        void PerformAttack()
        {
            var e = searcher.GetTargetEnemy();
            if (e != null)
            {
                _target = e.transform;
                _targetPos = _target.position;
                _attackIntervalTimer = attackInterval;
                player.animator.SetTrigger("DisplayAttack");
            }
        }

        public void AddFireBalls()
        {
            orbs.AddFireBalls();
        }

        public void Attacked()
        {
            //Debug.LogWarning("Attacked");
            orbs.ReleaseFirst(_targetPos);
            _target = null;
        }
    }
}