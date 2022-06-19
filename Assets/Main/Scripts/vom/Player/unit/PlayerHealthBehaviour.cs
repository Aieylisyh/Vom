using UnityEngine;
using game;

namespace vom
{
    public class PlayerHealthBehaviour : VomUnitComponent
    {
        public int healthMax;
        public int hp { get; private set; }
        public HealthBar bar;

        public void HealToFull()
        {
            hp = healthMax;
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
        }

        public bool isAlive { get { return hp > 0; } }
    }
}