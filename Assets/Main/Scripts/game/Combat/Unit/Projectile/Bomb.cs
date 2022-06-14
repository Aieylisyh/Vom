using UnityEngine;
using com;

namespace game
{
    public class Bomb : Projectile
    {
        public float speedNormal;
        public float speedFast;
        private bool _isShard;
        public float speedShard;
        public BombMove bombMove { get { return move as BombMove; } }
        public BombAi bombAi { get { return ai as BombAi; } }

        public PlayerBombModelSetup pbms;

        public void SetDamage(int d)
        {
            attack.dmg.Set(this, d, DamageType.Bomb, true);
        }

        public void Init(Vector3 pos, bool pIsShard = false)
        {
            _isShard = pIsShard;
            move.SetDir(Vector3.down);
            move.transform.position = pos;
            move.Speed = speedNormal;

            var attri = CombatService.instance.playerAttri;
            if (attri.cabModifier.bombDropFaster)
                move.Speed = speedFast;

            if (_isShard)
            {
                transform.localScale = Vector3.one * 0.7f;
            }
            else if (attri.cabModifier.bombSizeHuge)
            {
                transform.localScale = Vector3.one * 1.5f;
            }
            else
            {
                transform.localScale = Vector3.one;
            }

            CombatService.instance.Register(this);
            SetModel();
            bombMove.SetShard(false);

            ResetComponentState();
        }

        public void SetModel()
        {
            if (pbms != null)
                pbms.Set(this);

            SetBubble();
            SetFire();
        }

        public void TryCreateShards()
        {
            if (!HasShards())
                return;

            SoundService.instance.Play("exp wave");

            var p = GetShardsDamagePercent();
            var dmg = MathGame.GetPercentage(attack.dmg.value, p);
            if (Random.value >= 0.5f)
            {
                CreateShard(dmg, speedShard);
            }
            else
            {
                CreateShard(dmg, -speedShard);
            }
        }

        bool HasShards()
        {
            return !_isShard && GetShardsDamagePercent() > 0;
        }

        int GetShardsDamagePercent()
        {
            var attri = CombatService.instance.playerAttri;
            return attri.cabModifier.bombShardsDmgPercent;
        }

        public Bomb CreateShard(int damge, float speedX)
        {
            speedX *= Random.Range(0.35f, 1f);

            string prefabId = "bomb";
            var go = PoolingService.instance.GetInstance(prefabId);
            Bomb bomb = go.GetComponent<Bomb>();
            bomb.Init(transform.position, true);

            bomb.SetDamage(damge);
            bomb.collision.targetFlag = 0;//nothing

            bomb.SetShard(speedX);
            return bomb;
        }

        void SetShard(float speedX)
        {
            bombMove.SetShard(true, speedX);
        }

        public void ExitShardState()
        {
            collision.targetFlag = UnitCollision.CollisionFlag.Enemy;
        }
    }
}
