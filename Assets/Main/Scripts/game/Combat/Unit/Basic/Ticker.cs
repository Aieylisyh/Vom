using UnityEngine;

namespace game
{
    public class Ticker : MonoBehaviour
    {
        public float TickTime;
        private float _tickTimer;
        public bool fixedUpdateTick;

        protected virtual void Update()
        {
            if (fixedUpdateTick)
                return;

            _tickTimer -= com.GameTime.deltaTime;
            if (_tickTimer < 0)
            {
                _tickTimer = TickTime;
                Tick();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!fixedUpdateTick)
                return;

            Tick();
        }

        protected virtual void Tick()
        {

        }
    }
}
