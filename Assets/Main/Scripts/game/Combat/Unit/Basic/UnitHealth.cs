using UnityEngine;
using com;

namespace game
{
    public class UnitHealth : UnitComponent
    {
        public int hpMax;
        public int hp { get; protected set; }

        public int armor;
        public int armor_hotWeapon;
        public int armor_laser;
        public int armor_ghost;

        public float dmgReduce;
        public float dmgReduce_hotWeapon;
        public float dmgReduce_laser;
        public float dmgReduce_ghost;

        public float reg;

        public HealthBar hb;
        public ShakeBehaviour shakeBehaviour;

        public override void ResetState()
        {
            base.ResetState();
            hp = hpMax;
            hb?.Set(1, true);
            hb?.Hide();
        }

        public bool HasRemainingHp()
        {
            return hp > 0;
        }

        public virtual void RegTick()
        {
            if (!HasRemainingHp())
            {
                return;
            }

            OnHealthChange((int)GetRegPerTick());
        }

        protected virtual float GetRegPerTick()
        {
            return reg * TickTime;
        }

        protected float HealthRatio
        {
            get { return (float)hp / (float)hpMax; }
        }

        protected void OnHitFeedBack(int v)
        {
            if (v < -19)
            {
                SoundService.instance.Play("hit nor");
            }
            else if (v <= 0)
            {
                SoundService.instance.Play("hit l");
            }

            if (v < -1)
            {
                shakeBehaviour?.Shake();
            }
        }

        public virtual void OnReceiveDamage(Damage damage = null)
        {
            //Debug.Log(gameObject + "OnReceiveDamage " + damage.value);
            if (damage == null)
            {
                //no damage, no feed back, usually because of aoe
                return;
            }

            if (GetIsInvinsible())
            {
                TriggerInvinsible();
                return;
            }

            //Debug.Log("OnReceiveDamage  value " + damage.value);
            var delta = RefineDamageValue(damage);
            //Debug.Log("delta " + delta);
            OnHealthChange(-delta);
            postDamaged(damage, -delta);
        }

        protected virtual void postDamaged(Damage damage, int delta)
        {

        }

        protected virtual int RefineDamageValue(Damage damage)
        {
            var delta = GetDamageAfterArmor(damage);
            return delta;
        }

        protected virtual bool GetIsInvinsible()
        {
            return false;
        }

        protected virtual void TriggerInvinsible()
        {
            Debug.Log("TriggerInvinsible");
        }

        protected int GetDamageAfterArmor(Damage damage)
        {
            float v = damage.value;
            v -= armor;
            v *= (100f - dmgReduce) / 100f;
            if (damage.type == DamageType.Bomb || damage.type == DamageType.Torpedo)
            {
                v -= armor_hotWeapon;
                v *= (100f - dmgReduce_hotWeapon) / 100f;
            }
            if (damage.type == DamageType.Laser)
            {
                v -= armor_laser;
                v *= (100f - dmgReduce_laser) / 100f;
            }
            if (damage.type == DamageType.Ghost)
            {
                v -= armor_ghost;
                v *= (100f - dmgReduce_ghost) / 100f;
            }

            v = Mathf.Clamp(v, 1, 99999);
            return Mathf.RoundToInt(v);
        }

        protected virtual bool TryDie()
        {
            if (hp <= 0)
            {
                ClearHealth();
                self.death.Die(false);
                return true;
            }

            if (hp < 1)
            {
                hp = 1;
            }
            return false;
        }

        protected virtual void OnHealthChange(int v)
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main)
                return;

            hp = Mathf.Clamp(hp + v, 0, hpMax);
            //Debug.Log(self.gameObject + " hpChange v" + v + " hp " + hp + "/" + hpMax);

            if (TryDie())
                return;

            SetHealth(hp);
        }

        public void SetHealth(int v)
        {
            hp = Mathf.Clamp(v, 0, hpMax);
            if (hp >= hpMax)
            {
                hb?.Hide();
            }
            else
            {
                SetHealthBar(HealthRatio);
            }
        }

        protected virtual void SetHealthBar(float r)
        {
            hb?.Set(r);
        }

        public virtual void ClearHealth()
        {
            hb?.Hide();
            hp = 0;
        }
    }
}
