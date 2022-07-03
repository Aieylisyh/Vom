using UnityEngine;

namespace game
{
    public class Ticker : MonoBehaviour
    {
        public float TickTime;
        float _nextTimestamp;

        protected virtual void Update()
        {
            if (com.GameTime.time >= _nextTimestamp)
            {
                _nextTimestamp = com.GameTime.time + TickTime;
                Tick();
            }
        }

        protected virtual void Tick()
        {

        }
    }
}
