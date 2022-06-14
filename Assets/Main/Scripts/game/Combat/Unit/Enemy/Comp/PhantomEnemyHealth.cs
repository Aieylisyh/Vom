using UnityEngine;

namespace game
{
    public class PhantomEnemyHealth : EnemyHealth
    {

        public float durationShow;
        public GameObject ship;
        private float _phaseTimer;

        public override void ResetState()
        {
            SetHide();
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
                    SetHide();
                }
            }
        }

        private void SetHide()
        {
            ship.SetActive(false);
            _phaseTimer = 0;
        }

        private void SetShow()
        {
            ship.SetActive(true);
            _phaseTimer = durationShow;
        }

        protected override void postDamaged(Damage damage, int delta)
        {
            base.postDamaged(damage, delta);
            if (hp <= 0)
            {
                _phaseTimer = -1;
                ship.SetActive(true);
                return;
            }

            if (delta < 0)
            {
                SetShow();
            }
        }
    }
}
