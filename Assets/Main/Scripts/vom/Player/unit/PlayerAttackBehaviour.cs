using UnityEngine;
using com;

namespace vom
{
    public class PlayerAttackBehaviour : VomPlayerComponent
    {
        public float attackInterval = 1;
        private float _attackIntervalTimer;

        public PlayerEnemySearcherBehaviour searcher;

        public OrbsController orbs;

        public PlayerTargetCircleBehaviour playerTargetCircle;

        private Transform _target;
        private Vector3 _targetPos;

        Vector3 _defaultMmoCamOffset;
        Vector3 _cachedMmoCamOffset;
        public float camOffsetDist = 1;
        public float camOffsetSpeed = 2;

        float _skillChargingTimer;
        float _skillIntervalTimer;
        SkillPrototype _skillCharging;

        public ParticleSystem psArcane;

        public override void ResetState()
        {
            _attackIntervalTimer = 0;
            _defaultMmoCamOffset = host.move.mmoCamera.parameters.offset;
            _cachedMmoCamOffset = _defaultMmoCamOffset;
            _skillIntervalTimer = 0;
            _skillChargingTimer = 0;
            _skillCharging = null;
        }

        public void CheckMmoCameraOffset()
        {
            var dir = _cachedMmoCamOffset - host.move.mmoCamera.parameters.offset;
            var dist = dir.magnitude;
            if (dist >= 0.05f)
            {
                if (camOffsetSpeed * GameTime.deltaTime > dist)
                {
                    Vector3 speed = dist * dir.normalized;
                    host.move.mmoCamera.parameters.offset += speed;
                }
                else
                {
                    Vector3 speed = camOffsetSpeed * GameTime.deltaTime * dir.normalized;
                    host.move.mmoCamera.parameters.offset += speed;
                }
            }
        }

        void CancelTargeting()
        {
            if (_target != null || playerTargetCircle.showing || isCharging)
            {
                playerTargetCircle.Hide();
                _target = null;
                _cachedMmoCamOffset = _defaultMmoCamOffset;

                CancelCharging();
            }
        }

        void CancelCharging()
        {
            Debug.Log("CancelCharging");
            _skillIntervalTimer = 0;
            _skillChargingTimer = 0;
            _skillCharging = null;
            host.skill.CastChargeAnim(false);
        }

        public void Attack()
        {
            if (_attackIntervalTimer > 0)
                _attackIntervalTimer -= GameTime.deltaTime;

            if (host.move.isMoving)
            {
                CancelTargeting();
                return;
            }

            if (_target != null)
                host.move.Rotate((_targetPos - transform.position).normalized);

            if (isCharging)
            {
                Charge();
                return;
            }

            if (_attackIntervalTimer <= 0)
                PerformAttack();
        }

        void PerformAttack()
        {
            AimEnemy();
            if (_target != null)
            {
                _attackIntervalTimer = attackInterval;
                host.animator.SetBool("move", false);
                host.animator.SetTrigger("attack");
            }
        }

        public bool isAttacking { get { return isCharging || _attackIntervalTimer > 0f; } }

        public bool isCharging { get { return _skillChargingTimer > 0f; } }

        void Charge()
        {
            _skillChargingTimer -= GameTime.deltaTime;
            if (_skillChargingTimer < 0)
            {
                CancelCharging();
                return;
            }

            _skillIntervalTimer -= GameTime.deltaTime;
            if (_skillIntervalTimer <= 0)
            {
                _skillIntervalTimer += _skillCharging.interval;
                TriggerCharge();
            }
        }

        void TriggerCharge()
        {
            switch (_skillCharging.id)
            {
                case "ArcaneBlasts":
                    AimEnemy();

                    if (_target != null)
                    {
                        _attackIntervalTimer = attackInterval;
                        orbs.LaunchArcaneBlast(_targetPos);
                    }

                    break;
            }
        }

        void AimEnemy()
        {
            var e = searcher.GetTargetEnemy();
            if (e != null)
            {
                host.combat.UpdateState();
                _target = e.transform;
                _targetPos = _target.position;
                var delta = _targetPos - transform.position;
                delta.y = 0;
                if (delta.magnitude > 1)
                    delta = delta.normalized;
                _cachedMmoCamOffset = _defaultMmoCamOffset + delta * camOffsetDist;

                playerTargetCircle.Show(e);
            }
            else
            {
                CancelTargeting();
            }
        }

        public void StartChargingSkill(SkillPrototype skl)
        {
            //Debug.Log("StartChargingSkill");
            host.skill.CastChargeAnim(true);
            host.animator.SetBool("move", false);

            _skillChargingTimer = skl.duration;
            _skillIntervalTimer = skl.interval;
            _skillCharging = skl;

            psArcane.Play(true);
        }

        public void Attacked()
        {
            //Debug.LogWarning("Attacked");
            orbs.LaunchArcane(_targetPos);
            orbs.ReleaseFirst(_targetPos);
        }

        public bool HasTarget { get { return _target != null; } }
        public bool HasAliveTarget
        {
            get
            {
                if (_target == null)
                    return false;

                var e = _target.GetComponent<EnemyBehaviour>();
                return !e.death.dead;
            }
        }
    }
}