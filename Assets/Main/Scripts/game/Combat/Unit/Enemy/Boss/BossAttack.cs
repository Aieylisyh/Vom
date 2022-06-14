using UnityEngine;
using com;
using System.Collections.Generic;

namespace game
{
    public class BossAttack : EnemyAttack
    {
        private float _intervalTime;
        private float _intervalTimer;
        private bool _attacking;
        private string _crtAttakId;
        private float _damageFactor;
        private int _attackLimit;
        public Transform muzzlePos1;
        public Transform muzzlePos2;
        public Transform muzzlePos3;

        private int _attackPosId;

        public List<LineRendererLaser> lrls;


        public float laserDuration = 1;
        private float _laserTimer;
        private float _dmgPerTick;
        public float range = 1.4f;
        private LineRendererLaser _lrl;
        private bool _randomLaser;
        private int _randomLaserIndex = 1;

        public BossSkillPrototype.AttackMode attackMode { get; protected set; }

        private int ringAtkIndex = 0;
        private float ringAtkDeltaAngle = 0;

        public string psTeleportId = "teleport";
        public string psSummonId = "summon";
        public string psRingId = "swing";

        public override void ResetState()
        {
            foreach (var lrl in lrls)
            {
                lrl?.Stop();
            }

            base.ResetState();
        }

        void AssignByProto(BossSkillPrototype sklProto)
        {
            attackMode = sklProto.attackMode;
            _crtAttakId = sklProto.paramString1;
            _damageFactor = sklProto.damageRatio;
            _intervalTime = sklProto.interval;
            _attackPosId = sklProto.attackPosId;
            _attackLimit = sklProto.attackCountMax;

            switch (attackMode)
            {
                case BossSkillPrototype.AttackMode.TorSingle:

                    break;

                case BossSkillPrototype.AttackMode.TorRing:
                    ringAtkIndex = 0;
                    ringAtkDeltaAngle = 160f / (float)sklProto.paramInt1;
                    var goTorRing = PoolingService.instance.GetInstance(psRingId);
                    goTorRing.transform.position = transform.position + new Vector3(0, 0, -2.5f);
                    break;

                case BossSkillPrototype.AttackMode.Laser:
                    _dmgPerTick = (float)_damage * _damageFactor * TickTime;
                    _randomLaser = false;
                    if (sklProto.paramBool1)
                    {
                        _randomLaser = true;
                        _randomLaserIndex = 0;
                    }
                    break;

                case BossSkillPrototype.AttackMode.Summon:
                    if (sklProto.paramBool1)
                    {
                        var goSummon = PoolingService.instance.GetInstance(psSummonId);
                        goSummon.transform.position = transform.position + new Vector3(0, 0, -2.5f);
                        SoundService.instance.Play("ll");
                    }
                    break;

                case BossSkillPrototype.AttackMode.Teleport:
                    SoundService.instance.Play("acc");
                    break;
            }

        }

        public void InitBossAttack(BossSkillPrototype sklProto)
        {
            if (!self.IsAlive())
                return;

            _attacking = true;
            _intervalTimer = 0;

            AssignByProto(sklProto);
        }

        void LaserAttack()
        {
            _laserTimer = laserDuration;
            if (_randomLaser)
            {
                _randomLaserIndex++;
                if (_randomLaserIndex > 3)
                    _randomLaserIndex = 1;
                SetLaserLine(_randomLaserIndex);
                return;
            }

            SetLaserLine(_attackPosId);
        }

        void SingleAttack()
        {
            var dir = GetRelativeDirection(dirPrime);
            CreateTorDir(Vector3.zero, dir);
        }

        void RingAttack()
        {
            ringAtkIndex++;
            var dir0 = new Vector3(-3.1f, 1, 0);
            var dir1 = new Vector3(3.1f, 1, 0);
            var dir = Vector3.RotateTowards(dir0, dir1, ringAtkIndex * ringAtkDeltaAngle * Mathf.Deg2Rad, 0);
            //Debug.Log(ringAtkIndex * ringAtkDeltaAngle);
            CreateTorDir(Vector3.zero, GetRelativeDirection(dir.normalized));
        }

        void SummonEnemy()
        {
            string enemyId = _crtAttakId;
            var enemyGo = PoolingService.instance.GetInstance(enemyId);
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            enemy.Init(Random.value < 0.5f, Random.Range(5f, 11f), enemyId, false);
            var ed = enemy.death as EnemyDeath;
            ed.hasLoot = false;
        }

        void Teleport()
        {
            var em = self.move as EnemyMove;
            var cfg = ConfigService.instance.combatConfig.levelFieldParam;

            var pos = em.transform.position;
            var x = pos.x;

            var goTeleport = PoolingService.instance.GetInstance(psTeleportId);
            goTeleport.transform.position = pos + new Vector3(0, 0, -2);
            var posTo = pos;
            if (Mathf.Abs(cfg.boundLeft - x) > Mathf.Abs(cfg.boundRight - x))
            {
                posTo = new Vector3(cfg.boundLeft + 2.8f, pos.y, pos.z);
                em.Teleport(false, posTo);
            }
            else
            {
                posTo = new Vector3(cfg.boundRight - 2.8f, pos.y, pos.z);
                em.Teleport(true, posTo);
            }
            var goTeleportTo = PoolingService.instance.GetInstance(psTeleportId);
            goTeleportTo.transform.position = posTo + new Vector3(0, 0, -2);
        }

        protected override void LaunchAttack()
        {
            //Debug.Log("LaunchAttack " + attackMode);
            switch (attackMode)
            {
                case BossSkillPrototype.AttackMode.TorSingle:
                    SingleAttack();
                    break;

                case BossSkillPrototype.AttackMode.TorRing:
                    RingAttack();
                    break;

                case BossSkillPrototype.AttackMode.Laser:
                    LaserAttack();
                    break;

                case BossSkillPrototype.AttackMode.Summon:
                    SummonEnemy();
                    break;

                case BossSkillPrototype.AttackMode.Teleport:
                    Teleport();
                    break;
            }
        }

        void SetLaserLine(int i)
        {
            if (lrls[i] != null)
                _lrl = lrls[i];
            else
                _lrl = lrls[0];
        }

        public void Cease()
        {
            //Debug.Log("Cease");
            _attacking = false;
            _laserTimer = 0;
            if (_lrl != null)
                _lrl.Stop();
        }

        protected override void Tick()
        {
            if (!self.IsAlive())
                return;

            if (_attacking)
            {
                _intervalTimer -= TickTime;
                if (_intervalTimer < 0)
                {
                    _intervalTimer += _intervalTime;
                    if (_attackLimit != 0)
                    {
                        Attack();
                        if (_attackLimit > 0)
                        {
                            _attackLimit--;
                        }
                    }
                }
            }

            if (_lrl != null)
            {
                if (_laserTimer <= 0)
                    return;

                _laserTimer -= TickTime;
                var pShip = CombatService.instance.playerShip;
                var deltaX = pShip.move.transform.position.x - _lrl.transform.position.x;
                if (Mathf.Abs(deltaX) < range)
                    pShip.health.OnReceiveDamage(new Damage(self, (int)_dmgPerTick, DamageType.Laser, true));
            }
        }

        protected override void AttackFeedback()
        {
            switch (attackMode)
            {
                case BossSkillPrototype.AttackMode.TorSingle:
                    SoundService.instance.Play(attackSound);
                    if (hasMuzzleEffect)
                    {
                        var go = PoolingService.instance.GetInstance(muzzlePrefabId);
                        go.transform.position = GetMuzzlePos(_attackPosId).position;
                    }
                    break;

                case BossSkillPrototype.AttackMode.TorRing:
                    SoundService.instance.Play(attackSound);
                    if (hasMuzzleEffect)
                    {
                        var go = PoolingService.instance.GetInstance(muzzlePrefabId);
                        go.transform.position = GetMuzzlePos(_attackPosId).position;
                    }
                    break;

                case BossSkillPrototype.AttackMode.Laser:
                    _lrl.Play();
                    SoundService.instance.Play("lll");
                    break;

                case BossSkillPrototype.AttackMode.Summon:
                 
                    break;
            }
        }

        protected override void CreateTorDir(Vector3 offset, Vector3 primeDir)
        {
            CreateTor(offset, primeDir, 0);
        }

        private void CreateTor(Vector3 offset, Vector3 primeDir, float directAimOffset)
        {
            string prefabId = _crtAttakId;
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();

            var pos = GetMuzzlePos(_attackPosId).position;
            tor.Init(pos + offset, primeDir);
            tor.directAimOffset = directAimOffset;
            tor.SetDamage((int)(_damage * _damageFactor));
        }

        Transform GetMuzzlePos(int id)
        {
            var res = muzzlePos;
            if (id == 1)
                res = muzzlePos1;
            if (id == 2)
                res = muzzlePos2;
            if (id == 3)
                res = muzzlePos3;
            return res;
        }
    }
}
