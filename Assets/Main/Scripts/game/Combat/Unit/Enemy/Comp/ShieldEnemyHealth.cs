using UnityEngine;
using com;

namespace game
{
    public class ShieldEnemyHealth : EnemyHealth
    {
        public float durationVulnerable;
        public float durationAbsorb;

        public ParticleSystem psShield;
        public ParticleSystem psShieldStatic;
        public RotateBehaviour rb1;
        public RotateBehaviour rb2;
        private float _phaseTimer;
        private bool _currentVulnerable;

        public override void ResetState()
        {

            SetVulnerable();
            base.ResetState();
        }

        protected override void Tick()
        {
            base.Tick();

            if (_phaseTimer > 0)
            {
                _phaseTimer -= TickTime;
                if (_phaseTimer <= 0)
                {
                    if (_currentVulnerable)
                    {
                        SetAbsorb();
                    }
                    else
                    {
                        SetVulnerable();
                    }
                }
            }
        }

        private void SetVulnerable()
        {
            _currentVulnerable = true;
            _phaseTimer = durationVulnerable;
            rb1.Stop();
            rb2.Stop();
            psShieldStatic.Stop(true);
        }

        private void SetAbsorb()
        {
            _currentVulnerable = false;
            _phaseTimer = durationAbsorb;
            rb1.Play();
            rb2.Play();
            psShieldStatic.Play(true);
        }

        protected override int RefineDamageValue(Damage damage)
        {
            var delta = base.RefineDamageValue(damage);
            if (!_currentVulnerable && delta > 0)
            {
                psShield.Play(true);
                return -delta;
            }

            return delta;
        }
    }
}
