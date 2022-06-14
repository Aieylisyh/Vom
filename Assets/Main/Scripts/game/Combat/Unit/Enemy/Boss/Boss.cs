using UnityEngine;
using com;

namespace game
{
    public class Boss : Enemy
    {
        public bool spawnFromNear;

        public BossAi bossAi
        {
            get
            {
                return ai as BossAi;
            }
        }
        public BossAttack bossAttack
        {
            get
            {
                return attack as BossAttack;
            }
        }

        public BossMove bossMove
        {
            get
            {
                return move as BossMove;
            }
        }
        public BossHealth bossHealth
        {
            get
            {
                return health as BossHealth;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void SetupEnemy(string id)
        {
            base.SetupEnemy(id);
            var proto = EnemyService.instance.GetPrototype(this) as BossPrototype;

            bossHealth.reg = proto.GetReg(enemyLevel);
            bossHealth.armor = proto.GetArmor(enemyLevel);
        }

        public override void Init(bool goRight, float spawnHeight, string id, bool showupFromNear = false)
        {
            SetupEnemy(id);
            enemyMove.SetSpawnPosition(sizeFactor, goRight, spawnHeight, spawnFromNear, false);
            ResetComponentState();
        }
    }
}
