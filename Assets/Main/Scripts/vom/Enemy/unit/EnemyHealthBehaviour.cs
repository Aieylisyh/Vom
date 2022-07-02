using UnityEngine;

namespace vom
{
    public class EnemyHealthBehaviour : VomEnemyComponent
    {
        public int healthMax;
        public int hp { get; private set; }

        [HideInInspector]
        public HpBarBehaviour bar;

        public float hpBarOffset = 155;

        void Start()
        {
            if (bar == null)
                bar = HpBarSystem.instance.Create(transform, hpBarOffset, 1.0f);

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