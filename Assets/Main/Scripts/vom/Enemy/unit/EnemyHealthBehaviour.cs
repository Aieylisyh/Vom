using UnityEngine;
using com;

namespace vom
{
    public class EnemyHealthBehaviour : VomEnemyComponent
    {
        public int hpMax { get; private set; }
        public int hp { get; private set; }

        [HideInInspector]
        public HpBarBehaviour bar;

        public float hpBarOffset = 0;
        float _woundTimer;
        public float woundTime = 0.35f;

        public override void ResetState()
        {
            if (bar == null)
                bar = HpBarSystem.instance.Create(transform, (host.sizeValue - 0.1f) * 400 + hpBarOffset, host.sizeValue * 1.3f + 0.38f);

            if (host.proto.tier == EnemyTier.Elite)
                bar.powerScaleValue = 1.4f;//give better visual effect feeback
            else if (host.proto.tier == EnemyTier.Boss)
                bar.powerScaleValue = 2.0f;//give better visual effect feeback

            hpMax = host.proto.hp;
            ResetHp();
        }

        public void ResetHp()
        {
            hp = hpMax;
            SyncBar(true);
            bar.Hide();
        }

        public bool isWounding { get { return _woundTimer > 0f; } }

        void Wound()
        {
            _woundTimer = woundTime;
            host.animator.SetTrigger(EnemyAnimeParams.Wound);
        }

        public void OnUpdate()
        {
            if (_woundTimer > 0)
                _woundTimer -= GameTime.deltaTime;
        }

        public void SyncBar(bool instant)
        {
            bar.Set((float)hp / hpMax, instant);
        }

        public void ReceiveDamage(int v)
        {
            hp -= v;

            if ((float)v / hpMax > 0.15f)
                Wound();
            else if ((float)v / hpMax > 0.06f && Random.value > 0.7f)
                Wound();


            bar.Show();
            SyncBar(false);
            FloatingTextPanelBehaviour.instance.CreateDamageValue("<color=#FF5544>-" + v + "</color>", transform, new Vector2(0, 35));

            if (hp <= 0 && !host.death.dead)
            {
                host.death.Die();
            }
        }
    }
}