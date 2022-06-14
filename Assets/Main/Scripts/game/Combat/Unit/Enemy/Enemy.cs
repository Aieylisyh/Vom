using UnityEngine;
using com;

namespace game
{
    public class Enemy : Unit
    {
        public string id;
        public float sizeFactor = 1;
        public int enemyLevel { get; private set; }

        public EnemyAi enemyAi
        {
            get
            {
                return ai as EnemyAi;
            }
        }
        public EnemyAttack enemyAttack
        {
            get
            {
                return attack as EnemyAttack;
            }
        }
        //public UnitDeath death;
        public EnemyMove enemyMove
        {
            get
            {
                return move as EnemyMove;
            }
        }
        public EnemyHealth enemyHealth
        {
            get
            {
                return health as EnemyHealth;
            }
        }
        //public UnitReward reward;
        //public UnitRevive revive;
        //public UnitCollision collision;

        protected override void Start()
        {
            base.Start();
        }

        protected virtual void SetupEnemy(string id)
        {
            //Debug.Log("SetupEnemy " + gameObject.name);
            CombatService.instance.Register(this);
            this.id = id;

            var proto = EnemyService.instance.GetPrototype(this);
            enemyLevel = LevelService.instance.GetEnemyLevel();

            move.Speed = proto.speed;
            var atk = proto.GetAttack(enemyLevel);
            enemyAttack.Init(atk);
            var hp = proto.GetHp(enemyLevel);
            enemyHealth.Init(sizeFactor, hp);
            //Debug.Log("lv" + enemyLevel + " " + id + " Hp:" + hp + " Atk:" + atk);

            ToggleSpecialModule(false);
        }

        public virtual void InitSpawned(string id)
        {
            SetupEnemy(id);
            enemyMove.SetSimpleSpawn();
            ResetComponentState();
        }

        public virtual void Init(bool goRight, float spawnHeight, string id, bool showupFromNear = false)
        {
            SetupEnemy(id);
            enemyMove.SetSpawnPosition(sizeFactor, goRight, spawnHeight, showupFromNear, false);
            ResetComponentState();
        }

        public override void Recycle()
        {
            DetachHealthBar();
            PoolingService.instance.Recycle(this);
        }

        public void DetachHealthBar()
        {
            if (health.hb != null)
            {
                PoolingService.instance.Recycle(health.hb.gameObject);
                //health.hb = null;
            }
        }
    }
}