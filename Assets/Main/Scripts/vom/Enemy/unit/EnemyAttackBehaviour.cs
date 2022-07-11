using UnityEngine;
using com;

namespace vom
{
    public class EnemyAttackBehaviour : VomEnemyComponent
    {
        public AttackRange range = AttackRange.Melee;
        float _fRange;

        public float attackInterval;
        private float _attackIntervalTimer;

        public GameObject shootBullet;

        public Transform weaponPos;

        private Vector3 _targetPos;

        public int attack { get; private set; }

        public override void ResetState()
        {
            attack = host.proto.attack;
            _attackIntervalTimer = 0;
            _fRange = CombatSystem.GetRange(range);
        }
   
        public void Attack()
        {
            if (_attackIntervalTimer > 0)
                _attackIntervalTimer -= GameTime.deltaTime;

            if (host.targetSearcher.alerted && host.targetSearcher.target != null)
            {
                if (host.targetSearcher.targetDist < _fRange)
                {
                    _targetPos = host.targetSearcher.target.position;
                    host.move.Rotate(_targetPos - transform.position);
                    PerformAttack();
                }
            }
        }

        void PerformAttack()
        {
            if (_attackIntervalTimer > 0)
                return;

            _attackIntervalTimer = attackInterval;
            host.animator.SetTrigger(EnemyAnimeParams.Attack);
        }

        public void Attacked()
        {
            SpawnShoot(shootBullet, _targetPos);
        }

        public bool isAttacking { get { return _attackIntervalTimer > 0f; } }

        void SpawnShoot(GameObject prefab, Vector3 targetPos)
        {
            GameObject shootGo = Instantiate(prefab, CombatSystem.instance.projectileSpace);
            shootGo.SetActive(true);
            shootGo.transform.position = weaponPos.position;

            var shoot = shootGo.GetComponent<OrbBehaviour>();
            shoot.isEnemyShoot = true;
            shoot.dmg = attack;
            shoot.SetRelease(targetPos);
        }
    }
}