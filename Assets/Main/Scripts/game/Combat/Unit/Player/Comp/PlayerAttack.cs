using UnityEngine;
using com;

namespace game
{
    public class PlayerAttack : UnitAttack
    {
        public Transform bombSpawnPos;
        public Transform TorSpawnPos;

        public float bombRandomRange = 0.5f;
        private int _basebombDamage;
        private int _baseTorDamage;
        private int _torTraceNum;
        private int _torDirNum;

        private int _bombStorage;
        private int _baseBombStorage;
        private int _bombCount;
        private float _bombTimer;
        private float _baseBombTime;
        private float _resBombTime;
        private bool _doubleBombs;

        public string bombSound;
        public string torSound;
        public string missileSound;

        private float _autoTorTimer;
        private float _autoMissileTimer;
        public float autoTorTime = 4;
        public float autoMissileTime = 3;
        public float torDirStartSpeedNormal;
        public float torDirStartSpeedFast;
        public float torDirMaxSpeedNormal;
        public float torDirMaxSpeedFast;

        public void InitBomb(int bombDmg, int bombStorage, float BombTime)
        {
            _basebombDamage = bombDmg;
            _baseBombStorage = bombStorage;
            _baseBombTime = BombTime;

            var attri = CombatService.instance.playerAttri;
            var add = attri.ttModifier.bombReplenishAdd + attri.cabModifier.bombReplenishAdd;
            _resBombTime = _baseBombTime * (1f - add * 0.01f);
            _bombTimer = 0;

            _doubleBombs = false;
            if (attri.cabModifier.doubleBomb)
                _doubleBombs = true;

            var addStorage = attri.ttModifier.bombBackupNum;
            _bombStorage = _baseBombStorage + addStorage;
            _bombCount = _bombStorage;

            BombContainer.instance.SetBombNum(_bombStorage);
            BombContainer.instance.SetBombRestNum(_bombCount);
        }

        public void InitTor(int torDmg, int torTraceNum, int torDirNum)
        {
            _baseTorDamage = torDmg;
            _torTraceNum = torTraceNum;
            _torDirNum = torDirNum;

            _autoTorTimer = autoTorTime;
            _autoMissileTimer = autoMissileTime;
        }

        public override void ResetState()
        {
            base.ResetState();
        }

        protected override void Tick()
        {
            base.Tick();
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Gameplay)
            {
                RestoreBomb();
                CheckAuto();
            }
        }

        private void CheckAuto()
        {
            if (self.death.isDead)
                return;

            var attri = CombatService.instance.playerAttri;
            if (attri.cabModifier.autoTor)
            {
                _autoTorTimer -= TickTime;
                if (_autoTorTimer <= 0)
                {
                    _autoTorTimer = autoTorTime;
                    UseTorTrace(2);
                }
            }

            if (attri.cabModifier.autoMissile)
            {
                _autoMissileTimer -= TickTime;
                if (_autoMissileTimer <= 0)
                {
                    _autoMissileTimer = autoMissileTime;
                    UseMissile(1, attri.cabModifier.autoMissileDmgPercent);
                }
            }
        }

        private void RestoreBomb()
        {
            if (self.death.isDead)
                return;

            if (_bombCount < _bombStorage)
            {
                _bombTimer += TickTime;
                if (_bombTimer > _resBombTime)
                {
                    _bombTimer = 0;
                    _bombCount++;
                    BombContainer.instance.SetBombRestNum(_bombCount);
                }
            }
        }

        public void TryUseTorTrace(int amount)
        {
            amount = GetTorTraceLaunchAmount();
            UseTorTrace(amount);
        }

        public void UseTorTrace(int amount)
        {
            float f = (amount - 1) * 0.5f;
            for (int i = 0; i < amount; i++)
            {
                float t = i - f;
                //CreateTorTrace(new Vector3(0.6f * t, -0.05f, 0), new Vector3(1.5f * t, -0.5f, t));
                float lambda = (i % 2 == 0 ? 1 : -1);
                CreateTorTrace(new Vector3(0.5f * lambda * (2.5f + i) / 3f, -0.04f * i, 0), new Vector3(lambda, 0, 0), i);
            }

            LaunchTorFeedback();
            LevelTutorialService.instance.ProgressTuto_TorTrace();
        }

        public void UseMissile(int amount, int percentDmg)
        {
            //float f = (amount - 1) * 0.5f;
            for (int i = 0; i < amount; i++)
            {
                //float t = i - f;
                // float lambda = (i % 2 == 0 ? 1 : -1);
                CreateMissile(new Vector3(0, -0.05f, 0), new Vector3(0, -1f, 0), percentDmg);
            }
            SoundService.instance.Play(missileSound);
        }

        public void TryUseTorDirectional(Vector3 primeDir, int amount)
        {
            amount = GetTorDirLaunchAmount();

            primeDir.Normalize();
            var a = 0.25f;
            float f = (amount - 1) * 0.5f;
            for (int i = 0; i < amount; i++)
            {
                float t = i - f;
                CreateTorDir(new Vector3(0.5f * t, -0.05f, 0), primeDir, a + 0.25f * i);
            }

            LaunchTorFeedback();
            LevelTutorialService.instance.ProgressTuto_TorDir();
        }

        void LaunchTorFeedback()
        {
            var go = PoolingService.instance.GetInstance("tor launch");
            go.transform.position = self.move.transform.position + Vector3.down * 0.8f;
            go.transform.rotation = Quaternion.identity;

            SoundService.instance.Play(torSound);
        }

        public void TryUseBomb()
        {
            if (_bombCount <= 0)
                return;

            _bombCount--;
            int amount = GetBombLaunchAmount();
            for (int i = 0; i < amount; i++)
            {
                CreateBomb(new Vector3(Random.Range(-bombRandomRange, bombRandomRange), 0, 0));
            }

            SoundService.instance.Play(bombSound);
            BombContainer.instance.SetBombRestNum(_bombCount);
            LevelTutorialService.instance.ProgressTuto_Bomb();
            /*
             * var d = new Damage();
            d.value = 10;
            d.type = Damage.DamageType.Bomb;
            self.health.OnReceiveDamage(d);
            */
        }

        private int GetTorTraceLaunchAmount()
        {
            var attri = CombatService.instance.playerAttri;
            var chance = attri.ttModifier.torExtraChance;
            int add = 0;
            if (Random.value < (float)chance * 0.01f)
                add = 1;

            if (attri.cabModifier.torExtraAdd > 0)
            {
                add += attri.cabModifier.torExtraAdd;
            }

            return _torTraceNum + add;
        }

        private int GetTorDirLaunchAmount()
        {
            var attri = CombatService.instance.playerAttri;
            var chanceAdd1 = attri.ttModifier.torExtraChance;
            int add = 0;
            if (Random.value < (float)chanceAdd1 * 0.01f)
                add += 1;

            if (attri.cabModifier.torExtraAdd > 0)
            {
                add += attri.cabModifier.torExtraAdd;
            }

            return _torTraceNum + add;
        }

        public int GetBombLaunchAmount()
        {
            if (_doubleBombs)
                return 2;

            return 1;
        }

        private Torpedo CreateTorDir(Vector3 offset, Vector3 primeDir, float turn = 0)
        {
            string prefabId = "torpedoDir";
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.Init(Torpedo.TorType.Direct, TorSpawnPos.position + offset, primeDir);
            tor.SetDamage(GetTorDirDamage());

            tor.torMove.SetWave(turn);

            var attri = CombatService.instance.playerAttri;
            if (attri.cabModifier.torDirHitProj)
            {
                tor.collision.targetFlag = UnitCollision.CollisionFlag.EnemyProjectile | UnitCollision.CollisionFlag.Enemy;
            }
            else
            {
                tor.collision.targetFlag = UnitCollision.CollisionFlag.Enemy;
            }
            if (attri.cabModifier.torDirSpeedUp)
            {
                tor.torMove.useWave = false;
                tor.torMove.speedMax = torDirMaxSpeedFast;
                tor.torMove.startSpeed = torDirStartSpeedFast;
            }
            else
            {
                tor.torMove.useWave = true;
                tor.torMove.speedMax = torDirMaxSpeedNormal;
                tor.torMove.startSpeed = torDirStartSpeedNormal;
            }

            tor.ResetComponentState();
            return tor;
        }

        private Torpedo CreateTorTrace(Vector3 offset, Vector3 primeDir, int index)
        {
            string prefabId = "torpedoTrace";
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.InitTorDrop(Torpedo.TorType.Trace, TorSpawnPos.position + offset, primeDir, index * 1.0f + 4f, index * 0.06f + 0.4f);
            tor.SetDamage(GetTorTraceDamage());

            var attri = CombatService.instance.playerAttri;
            if (attri.cabModifier.torTraceHitProj)
            {
                tor.collision.targetFlag = UnitCollision.CollisionFlag.EnemyProjectile | UnitCollision.CollisionFlag.Enemy;
                tor.torMove.searchTargetType = TorpedoMove.SearchTargetType.EnemyAndGhost;
            }
            else
            {
                tor.collision.targetFlag = UnitCollision.CollisionFlag.Enemy;
                tor.torMove.searchTargetType = TorpedoMove.SearchTargetType.Enemy;
            }

            tor.ResetComponentState();
            return tor;
        }

        private Torpedo CreateMissile(Vector3 offset, Vector3 primeDir, int percentDmg)
        {
            string prefabId = "autoMissile";
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.Init(Torpedo.TorType.Trace, TorSpawnPos.position + offset, primeDir);
            //tor.collision.targetFlag = UnitCollision.CollisionFlag.EnemyProjectile | UnitCollision.CollisionFlag.Enemy;
            //tor.torMove.searchTargetType = TorpedoMove.SearchTargetType.EnemyAndGhost;
            var dmg = GetTorTraceDamage();
            tor.SetMissileDamage(MathGame.GetPercentage(dmg, percentDmg));
            return tor;
        }

        private Bomb CreateBomb(Vector3 offset)
        {
            string prefabId = "bomb";
            var go = PoolingService.instance.GetInstance(prefabId);
            Bomb bomb = go.GetComponent<Bomb>();
            bomb.Init(bombSpawnPos.position + offset);
            bomb.SetDamage(GetBombDamage());

            var attri = CombatService.instance.playerAttri;
            if (attri.cabModifier.bombHitProj)
            {
                bomb.collision.targetFlag = UnitCollision.CollisionFlag.EnemyProjectile | UnitCollision.CollisionFlag.Enemy;
            }
            else
            {
                bomb.collision.targetFlag = UnitCollision.CollisionFlag.Enemy;
            }

            return bomb;
        }

        private int GetBombDamage()
        {
            var attri = CombatService.instance.playerAttri;
            var addCritPercent = attri.ttModifier.bombDmgAdd_critChance;
            int addPercent = 0;
            if (addCritPercent > 0 && Random.value <= 0.16f)
            {
                addPercent = addCritPercent;
            }

            var res = MathGame.GetPercentageAdded(_basebombDamage, addPercent);
            return res;
        }

        private int GetTorTraceDamage()
        {
            int addPercent = 0;
            var attri = CombatService.instance.playerAttri;
            var chancePenetrate = attri.ttModifier.torChancePenetrate;
            if (chancePenetrate > 0 && Random.value < chancePenetrate / 100f)
                addPercent = 100;

            var res = MathGame.GetPercentageAdded(_baseTorDamage, addPercent);
            return res;
        }

        private int GetTorDirDamage()
        {
            int addPercent = 0;
            var attri = CombatService.instance.playerAttri;
            var chancePenetrate = attri.ttModifier.torChancePenetrate;
            if (chancePenetrate > 0 && Random.value < chancePenetrate / 100f)
            {
                addPercent = 100;
            }

            addPercent += attri.cabModifier.torDirDmgAdd;
            var res = MathGame.GetPercentageAdded(_baseTorDamage, addPercent);
            return res;
        }

       public void LaunchBossCinematicTors()
        {
            LaunchBossCinematicTor(new Vector3(-1, -3f, 0), new Vector3(-1, 0, 0), false);
            LaunchBossCinematicTor(new Vector3(-1, -2f, 0), new Vector3(-1, -0.25f, 0), false);
            LaunchBossCinematicTor(new Vector3(0, -1f, 0), new Vector3(0, -1, 0), false);
            LaunchBossCinematicTor(new Vector3(1, -0.5f, 0), new Vector3(1, -0.25f, 0), false);
            LaunchBossCinematicTor(new Vector3(1, 0f, 0), new Vector3(1, 0, 0), true);
            LaunchTorFeedback();
        }

        void LaunchBossCinematicTor(Vector3 offset, Vector3 primeDir, bool camOn)
        {
            //Debug.Log("LaunchBossCinematicTor");
            string prefabId = "torpedoTrace";
            var go = PoolingService.instance.GetInstance(prefabId);
            Torpedo tor = go.GetComponent<Torpedo>();
            tor.InitTorDrop(Torpedo.TorType.Trace, TorSpawnPos.position + offset, primeDir, 6, 0.7f);
            tor.SetDamage(0);
            tor.collision.targetFlag = UnitCollision.CollisionFlag.Enemy;
            tor.torMove.searchTargetType = TorpedoMove.SearchTargetType.Enemy;
            tor.ResetComponentState();

            if (camOn)
            {
                tor.attack.dmg.isBossKiller = true;
                Transform target = tor.transform.GetChild(0);
                CameraControllerBehaviour.instance.SwitchToCinematicView(target,10);
            }
        }
    }
}