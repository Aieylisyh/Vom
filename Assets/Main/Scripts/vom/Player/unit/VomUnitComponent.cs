using UnityEngine;
using game;

namespace vom
{
    public class VomUnitComponent : Ticker
    {
        public bool HasTick;
        public bool IsEnabled = true;

        protected PlayerBehaviour player;

        protected virtual void Awake()
        {
            player = GetComponent<PlayerBehaviour>();
            if (player == null)
            {
                player = GetComponentInParent<PlayerBehaviour>();
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