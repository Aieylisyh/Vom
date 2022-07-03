using UnityEngine;

namespace vom
{
    public class EnemyHealthBehaviour : VomEnemyComponent
    {
        public int healthMax;
        public int hp { get; private set; }

        [HideInInspector]
        public HpBarBehaviour bar;

        public float hpBarOffset = 0;

        void Start()
        {
            if (bar == null)
                bar = HpBarSystem.instance.Create(transform, (host.size - 0.1f) * 400 + hpBarOffset, host.size * 1.3f + 0.38f);

            HealToFull();
        }

        public void HealToFull()
        {
            hp = healthMax;
            SyncBar(true);
            bar.Hide();
        }

        public void SyncBar(bool instant)
        {
            bar.Set((float)hp / healthMax, instant);
        }

        public void ReceiveDamage(int v)
        {
            hp -= v;
            bar.Show();
            SyncBar(false);

            if (hp <= 0 && !host.death.dead)
            {
                host.death.Die();
            }
        }
    }
}