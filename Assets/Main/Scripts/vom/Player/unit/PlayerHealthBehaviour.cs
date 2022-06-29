using UnityEngine;
using game;

namespace vom
{
    public class PlayerHealthBehaviour : VomPlayerComponent
    {
        public int healthMax;
        public int hp { get; private set; }
        [HideInInspector]
        public HpBarBehaviour bar;
        public bool dead { get; private set; }

        public override void ResetState()
        {
            bar = HpBarSystem.instance.Create(transform, 140, 0.75f);
            HealToFull();
        }

        public void HealToFull()
        {
            hp = healthMax;
            SyncBar(true);
            dead = false;
        }

        public void Heal(int v)
        {
            hp += v;
            if (hp>healthMax)
            {
                hp = healthMax;
            }

            SyncBar(true);
        }

        public void SyncBar(bool instant)
        {
            bar.Set((float)hp / healthMax, instant);
        }

        public void ReceiveDamage(int v)
        {
            hp -= v;
            SyncBar(false);

            if (hp <= 0 && !dead)
            {
                Die();
            }
        }

        void Die()
        {
            dead = true;
            host.animator.SetTrigger("Die");
            bar.Hide();
        }
    }
}