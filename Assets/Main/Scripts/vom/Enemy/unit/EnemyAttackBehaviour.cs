using UnityEngine;
using com;

namespace vom
{
    public class EnemyAttackBehaviour : VomEnemyComponent
    {
        public AttackRange range= AttackRange.Melee;
        float _fRange;
        public int dmg;

        public float attackInterval;
        private float _attackIntervalTimer;

        public GameObject shootBullet;

        public Transform weaponPos;

        private Transform _target;
        private Vector3 _targetPos;

        public bool HasTarget { get { return _target != null; } }

        protected override void Start()
        {
            base.Start();
            _attackIntervalTimer = 0;
            _fRange = CombatSystem.instance.GetRange(range);
        }

        public void Attack()
        {
            if (_attackIntervalTimer > 0)
                _attackIntervalTimer -= GameTime.deltaTime;

            var dir = PlayerBehaviour.instance.transform.position - transform.position;
            if (dir.magnitude < _fRange)
            {
                _target = PlayerBehaviour.instance.transform;
                _targetPos = _target.position;
                PerformAttack();
            }
            else
            {
                _target = null;
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

        void SpawnShoot(GameObject prefab, Vector3 targetPos)
        {
            GameObject shootGo = Instantiate(prefab, EnemySystem.instance.attackSpace);
            shootGo.SetActive(true);
            shootGo.transform.position = weaponPos.position;

            var shoot = shootGo.GetComponent<OrbBehaviour>();
            shoot.isEnemyShoot = true;
            shoot.dmg = dmg;
            shoot.SetRelease(targetPos);
        }
    }
}
