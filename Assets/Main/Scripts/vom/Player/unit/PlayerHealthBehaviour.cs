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
            ResetHealth();
        }

        public void ResetHealth()
        {
            hp = healthMax;
            SyncBar(true);
            dead = false;
        }

        public void Heal(int v)
        {
            if (v <= 0)
                return;

            var oldHp = hp;
            hp += v;

            if (hp > healthMax)
                hp = healthMax;

            var addHp = hp - oldHp;
            if (addHp > 0)
            {
                FloatingTextPanelBehaviour.instance.CreateCombatValue("<color=#88FF00>+" + addHp + "</color>", transform, new Vector2(0, 35));
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

            //FloatingTextPanelBehaviour.instance.CreateCombatValue("<color=#FF0000>-" + v + "</color>", transform, new Vector2(0, 35));

            if (hp <= 0 && !dead)
            {
                Die();
            }
        }

        void Die()
        {
            dead = true;
            host.animator.SetTrigger(PlayerAnimeParams.die);
            bar.Hide();
            host.attack.OnDead();
            host.combat.UpdateState();
        }
    }
}