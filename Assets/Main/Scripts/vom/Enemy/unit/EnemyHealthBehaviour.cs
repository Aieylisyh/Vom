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
        public Collider bodyCol;

        public void Init()
        {
            if (bar == null)
            {
                bar = HpBarSystem.instance.Create(transform, hpBarOffset, 1.0f);
            }

            if (bodyCol != null)
                bodyCol.enabled = true;
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

            if (bodyCol != null)
                bodyCol.enabled = false;

            host.cc.enabled = false;
            //var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            //go.SetActive(true);
            for (int i = 0; i < 6; i++)
            {
                LootSystem.instance.SpawnSoul(transform.position, new ItemData(1, "Soul"), i);
            }
            //HeartDistortSystem.instance.Create(this.transform, 20, 1f);
        }
    }
}