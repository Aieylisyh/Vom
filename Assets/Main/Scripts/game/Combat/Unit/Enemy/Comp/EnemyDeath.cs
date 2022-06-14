using UnityEngine;
using com;

namespace game
{
    public class EnemyDeath : UnitDeath
    {
        public enum DeathExplosion
        {
            None,
            Small,
            SmallBlue,
            Light,
            Mid,
            MidBlue,
            Big,
            BigBlue,
            LL,
            Ghost,
            Red
        };

        public DeathExplosion deathExplosion;
        public bool attackOnDeath;
        public float shakeTime = 0;
        public bool hasLoot;

        public override void ResetState()
        {
            hasLoot = true;
            base.ResetState();
        }

        protected override void DieUnsilent()
        {
            SpawnLoot();

            MissionService.instance.PushDl("ene", 1, true);
            MissionService.instance.PushDl("ene big", 1, true);
            Explode();
            Vibration.VibrateShort();
            if (attackOnDeath)
                self.attack.Attack();

            EnemyMove m = self.move as EnemyMove;
            m.StartSink();//not recycle now

            if (shakeTime > 0)
            {
                CameraControllerBehaviour.instance.ShakeCombatCam(shakeTime);
            }
        }

        private void Explode()
        {
            string effectId = "";
            switch (deathExplosion)
            {
                case DeathExplosion.None:
                    break;
                case DeathExplosion.Small:
                    effectId = "exp small";
                    break;
                case DeathExplosion.SmallBlue:
                    effectId = "exp small blue";
                    break;
                case DeathExplosion.Light:
                    effectId = "exp light";
                    break;
                case DeathExplosion.Mid:
                    effectId = "exp mid";
                    break;
                case DeathExplosion.MidBlue:
                    effectId = "exp mid blue";
                    break;
                case DeathExplosion.Big:
                    effectId = "exp big";
                    break;
                case DeathExplosion.BigBlue:
                    effectId = "exp big blue";
                    break;
                case DeathExplosion.LL:
                    effectId = "exp ll";
                    break;
                case DeathExplosion.Ghost:
                    effectId = "exp ghost";
                    break;
                case DeathExplosion.Red:
                    effectId = "exp red";
                    break;
            }

            SpawnEffect(effectId);
        }

        protected void SpawnLoot()
        {
            if (!hasLoot)
                return;

            var proto = EnemyService.instance.GetPrototype(self);
            var enemyLevel = (self as Enemy).enemyLevel;
            var drops = proto.GetDrops(enemyLevel);
            var score = proto.dropData.score;

            LevelService.instance.runtimeLevel.AddLevelScore(score);

            LevelService.instance.AddLevelLoot("Exp", proto.GetExp(enemyLevel));

            for (int i = 0; i < drops.Count; i++)
            {
                var drop = drops[i];
                if (drop.n <= 0)
                    continue;

                SpawnOneLoot(drop, i + 1);
                var fakeDrop = new Item(0, drop.id);
                if (drop.n > 2)
                {
                    SpawnOneLoot(fakeDrop, i + 1);
                }
                if (drop.n > 15)
                {
                    SpawnOneLoot(fakeDrop, i + 1);
                }
                if (drop.n > 300)
                {
                    SpawnOneLoot(fakeDrop, i + 1);
                }
            }
        }

        private void SpawnOneLoot(Item item, int indexCount)
        {
            if (string.IsNullOrEmpty(item.id))
                return;

            var go = PoolingService.instance.GetInstance("loot");
            Loot loot = go.GetComponent<Loot>();
            loot.transform.position = self.move.transform.position;
            loot.Init(item, indexCount);
        }
    }
}