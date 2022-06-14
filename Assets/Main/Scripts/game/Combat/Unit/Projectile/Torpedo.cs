using UnityEngine;

namespace game
{
    public class Torpedo : Projectile
    {
        public TorpedoAi torAi
        {
            get
            {
                return ai as TorpedoAi;
            }
        }

        public TorpedoMove torMove
        {
            get
            {
                return move as TorpedoMove;
            }
        }

        public enum TorType
        {
            Direct,
            Trace,
        }

        public TorType torType;

        public enum DirectAimType
        {
            None,
            PlayerLimitAngularDelta,//限制只能朝Player转directAimOffset角度
            PlayerWithFixedOffset,//朝Player靠近自己的方向有固定directAimOffset距离的偏移
            PlayerWithRandomOffset,//朝Player有固定directAimOffset距离但是正负随机的偏移
        }

        public DirectAimType directAimType = DirectAimType.None;
        public float directAimOffset = 0;

        public PlayerTorpedoModelSetup ptms;

        protected override void Start()
        {
        }

        public void SetDamage(int d)
        {
            attack.dmg.Set(this, d, DamageType.Torpedo, true);
        }
        public void SetMissileDamage(int d)
        {
            attack.dmg.Set(this, d, DamageType.None, true);
        }
        public void SetGhostDamage(int myLevel)
        {
            var ghostPrototype = EnemyService.instance.GetPrototype("Ghost");
            attack.dmg.Set(this, ghostPrototype.GetAttack(myLevel), DamageType.Ghost, true);
        }

        public void Init(Vector3 pos, Vector3 primeDir)
        {
            torMove.transform.position = pos;
            torMove.SetDir(primeDir);
            //Debug.Log("Init primeDir " + primeDir);
            //Debug.Log(torMove.dir);
            //Debug.Log(pos);
            primeDir.z = 0;
            torMove.SetPrimeDir(primeDir);
            CombatService.instance.Register(this);
            SetModel();
            ResetComponentState();
        }

        public void Init(TorType type, Vector3 pos, Vector3 primeDir)
        {
            torType = type;
            Init(pos, primeDir);
        }

        public void InitTorDrop(TorType type, Vector3 pos, Vector3 primeDir, float dropStartSpeed,float pNoRotateTime)
        {
            torType = type;
            Init(pos, primeDir);
            torMove.SetDrop(dropStartSpeed, pNoRotateTime);
        }

        public void SetModel()
        {
            ptms?.Set(this);
            SetBubble();
            SetFire();
        }
    }
}
