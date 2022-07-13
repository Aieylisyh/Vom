using UnityEngine;
using com;

namespace vom
{
    public class EnemyAttackBehaviour : VomEnemyComponent
    {
        public AttackRange range = AttackRange.Melee;
        float _fRange;

        public float attackInterval { get; private set; }
        private float _attackIntervalTimer;

        public GameObject shootBullet;
        public string attackSound;

        public Transform weaponPos;

        private Vector3 _targetPos;

        public int attack { get; private set; }

        public override void ResetState()
        {
            attack = host.proto.attack;
            attackInterval = host.proto.attackInterval;
            _attackIntervalTimer = 0;
            _fRange = CombatSystem.GetRange(range);
        }

        public void Attack()
        {
            if (_attackIntervalTimer > 0)
                _attackIntervalTimer -= GameTime.deltaTime;

            if (host.targetSearcher.alerted && host.targetSearcher.alertOrigin != null)
            {
                if (host.targetSearcher.targetDist < _fRange)
                {
                    _targetPos = host.targetSearcher.alertOrigin.position;
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
            if (!string.IsNullOrEmpty(attackSound))
                SoundService.instance.Play(attackSound);
            else if (!host.proto.normalAttackIsRanged)
            {
                SoundService.instance.Play(new string[2] { "melee1", "melee2" });
            }
            host.animator.SetTrigger(EnemyAnimeParams.Attack);
        }

        public void Attacked()
        {
            if (host.proto.normalAttackIsRanged)
            {
                SpawnShoot(shootBullet, _targetPos);
            }
            else
            {
                MeleeAttack();
            }
        }

        void MeleeAttack()
        {
            var radius = ConfigSystem.instance.combatConfig.meleeAttackRadioRangeRatio * _fRange;
            var center = (_targetPos - transform.position).normalized * _fRange * 0.5f + transform.position;
            var currentTarget = PlayerBehaviour.instance;
            var targetCurrentPos = currentTarget.transform.position;
            var dist = targetCurrentPos - center;
            var targetInRange = dist.magnitude < radius;
            if (targetInRange)
            {
                SoundService.instance.Play(new string[2] { "hit1", "hit2" });
                currentTarget.OnHit(attack);
            }
        }

        public bool isAttacking { get { return _attackIntervalTimer > 0f; } }

        void SpawnShoot(GameObject prefab, Vector3 targetPos)
        {
            GameObject shootGo = Instantiate(prefab, CombatSystem.instance.projectileSpace);
            shootGo.SetActive(true);

            var shoot = shootGo.GetComponent<OrbBehaviour>();
            shoot.isEnemyShoot = true;
            shoot.dmg = attack;
            shoot.SetOrigin(weaponPos, true);
            shoot.SetRelease(targetPos);
        }
    }
}