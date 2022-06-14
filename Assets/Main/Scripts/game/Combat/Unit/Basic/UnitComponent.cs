using UnityEngine;

namespace game
{
    public class UnitComponent : Ticker
    {
        public bool HasTick;
        public bool IsEnabled = true;

        protected Unit self;

        protected virtual void Awake()
        {
            self = GetComponent<Unit>();
            if (self == null)
            {
                self = GetComponentInParent<Unit>();
            }
        }

        protected virtual void Start()
        {
        }

        protected override void Update()
        {
            if (!IsEnabled)
            {
                return;
            }

            if (!HasTick)
            {
                return;
            }

            base.Update();
        }

        public virtual void ResetState()
        {

        }
    }
}
