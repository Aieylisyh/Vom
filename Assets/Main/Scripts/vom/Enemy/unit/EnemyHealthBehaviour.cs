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

            ResetHealth();
        }

        public void ResetHealth()
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
            game.FloatingTextPanelBehaviour.instance.CreateCombatValue("<color=#FFFFFF>-" + v + "</color>", transform, new Vector2(0, 20));

            if (hp <= 0 && !host.death.dead)
            {
                host.death.Die();
            }
        }
    }
}