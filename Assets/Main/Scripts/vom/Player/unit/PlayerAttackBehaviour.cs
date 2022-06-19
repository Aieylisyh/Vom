using UnityEngine;
using com;

namespace vom
{
    public class PlayerAttackBehaviour : VomUnitComponent
    {
        public float attackInterval = 1;
        private float _attackIntervalTimer;

        public PlayerEnemySearcherBehaviour searcher;

        public OrbsController orbs;

        public PlayerTargetCircleBehaviour playerTargetCircle;

        public string spawnFireballSound;
        public string spawnIceballSound;
        public string spawnPoisonballSound;

        private Transform _target;
        private Vector3 _targetPos;

        protected override void Start()
        {
            base.Start();
            _attackIntervalTimer = 0;
        }

        void CancelTargeting()
        {
            playerTargetCircle.Hide();
            _target = null;
        }

        public void Attack()
        {
            if (_attackIntervalTimer > 0)
            {
                _attackIntervalTimer -= GameTime.deltaTime;
            }

            if (player.move.isMoving)
            {
                CancelTargeting();
                return;
            }

            if (_attackIntervalTimer <= 0)
            {
                PerformAttack();
                if (_target != null)
                {
                    player.move.Rotate((_targetPos - transform.position).normalized);
                }
            }
        }

        void PerformAttack()
        {
            var e = searcher.GetTargetEnemy();
            if (e != null)
            {
                _target = e.transform;
                _targetPos = _target.position;
                _attackIntervalTimer = attackInterval;
                player.animator.SetBool("move", false);
                player.animator.SetTrigger("attack");
                playerTargetCircle.Show(e);
            }
            else
            {
                CancelTargeting();
            }
        }

        public bool isAttacking { get { return _attackIntervalTimer > 0f; } }

        public void AddFireBalls()
        {
            orbs.AddFireBalls();
            SoundService.instance.Play(spawnFireballSound);
        }

        public void AddIceBalls()
        {
            orbs.AddIceBalls();
            SoundService.instance.Play(spawnIceballSound);
        }

        public void AddPoisonBalls()
        {
            orbs.AddPoisonBalls();
            SoundService.instance.Play(spawnPoisonballSound);
        }

        public void Attacked()
        {
            //Debug.LogWarning("Attacked");
            orbs.LaunchArcane(_targetPos);
            orbs.ReleaseFirst(_targetPos);
            _target = null;
        }
    }
}