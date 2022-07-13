using UnityEngine;

namespace vom
{
    public class EnemyHealthBehaviour : VomEnemyComponent
    {
        public int hpMax { get; private set; }
        public int hp { get; private set; }

        [HideInInspector]
        public HpBarBehaviour bar;

        public float hpBarOffset = 0;

        public override void ResetState()
        {
            if (bar == null)
                bar = HpBarSystem.instance.Create(transform, (host.sizeValue - 0.1f) * 400 + hpBarOffset, host.sizeValue * 1.3f + 0.38f);

            hpMax = host.proto.hp;

            ResetHp();
        }

        public void ResetHp()
        {
            hp = hpMax;
            SyncBar(true);
            bar.Hide();
        }

        public void SyncBar(bool instant)
        {
            bar.Set((float)hp / hpMax, instant);
        }

        public void ReceiveDamage(int v)
        {
            hp -= v;
            bar.Show();
            SyncBar(false);
            FloatingTextPanelBehaviour.instance.CreateCombatValue("<color=#FFFFFF>-" + v + "</color>", transform, new Vector2(0, 20));

            if (hp <= 0 && !host.death.dead)
            {
                host.death.Die();
            }
        }
    }
}