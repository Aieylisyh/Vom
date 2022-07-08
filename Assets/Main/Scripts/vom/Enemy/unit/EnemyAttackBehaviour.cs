using UnityEngine;
using com;

namespace vom
{
    public class EnemyAttackBehaviour : VomEnemyComponent
    {
        public AttackRange range = AttackRange.Melee;
        float _fRange;
        public int dmg;

        public float attackInterval;
        private float _attackIntervalTimer;

        public GameObject shootBullet;

        public Transform weaponPos;

        private Vector3 _targetPos;

        void Start()
        {
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
                    PerformAttack();
                }
            }
        }

        void PerformAttack()
        {
            if (_attackIntervalTimer > 0)
                return;

            _attackIntervalTimer = attackInterval;
            host.animator.SetTrigger("MeleeAttack");
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
            shoot.dmg = dmg;
            shoot.SetRelease(targetPos);
        }
    }
}