using UnityEngine;
using game;

namespace vom
{
    public class EnemyHealthBehaviour : VomEnemyComponent
    {
        public int healthMax;
        public int hp { get; private set; }

        [HideInInspector]
        public HpBarBehaviour bar;

        public bool dead { get; private set; }
        public float hpBarOffset = 155;

        public void Init()
        {
            if (bar == null)
            {
                bar = HpBarSystem.instance.Create(transform, hpBarOffset, 1.0f);
            }

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
            host.targetSearcher.ExitAlert();

            //var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            //go.SetActive(true);
            for (int i = 0; i < 10; i++)
            {
                LootSystem.instance.SpawnGold(transform.position, new ItemData(1, "wood"), i);
            }
            //HeartDistortSystem.instance.Create(this.transform, 20, 1f);
        }
    }
}