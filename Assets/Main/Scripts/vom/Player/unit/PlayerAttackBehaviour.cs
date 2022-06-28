﻿using UnityEngine;
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

        public string spawnFireballSound;
        public string spawnIceballSound;
        public string spawnPoisonballSound;

        private Transform _target;
        private Vector3 _targetPos;

        Vector3 _defaultMmoCamOffset;
        Vector3 _cachedMmoCamOffset;
        public float camOffsetDist = 1;
        public float camOffsetSpeed = 2;

        protected override void Start()
        {
            base.Start();
            _attackIntervalTimer = 0;
            _defaultMmoCamOffset = host.move.mmoCamera.parameters.offset;
            _cachedMmoCamOffset = _defaultMmoCamOffset;
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
            playerTargetCircle.Hide();
            _target = null;
            _cachedMmoCamOffset = _defaultMmoCamOffset;
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
            {
                host.move.Rotate((_targetPos - transform.position).normalized);
            }

            if (_attackIntervalTimer <= 0)
            {
                PerformAttack();
            }
        }

        void PerformAttack()
        {
            var e = searcher.GetTargetEnemy();
            if (e != null)
            {
                _target = e.transform;
                _targetPos = _target.position;
                var delta = _targetPos - transform.position;
                delta.y = 0;
                if (delta.magnitude > 1)
                    delta = delta.normalized;
                _cachedMmoCamOffset = _defaultMmoCamOffset + delta * camOffsetDist;

                _attackIntervalTimer = attackInterval;
                host.animator.SetBool("move", false);
                host.animator.SetTrigger("attack");
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
        }

        public bool HasTarget { get { return _target != null; } }
        public bool HasAliveTarget
        {
            get
            {
                if (_target == null)
                    return false;

                var e = _target.GetComponent<EnemyBehaviour>();
                return !e.health.dead;
            }
        }
    }
}