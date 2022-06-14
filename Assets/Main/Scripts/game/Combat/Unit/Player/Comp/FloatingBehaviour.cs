using UnityEngine;

namespace game
{
    public class FloatingBehaviour : Ticker
    {
        public Transform floatingTrans;
        public float floatingFreqZ = 1.5f;
        public float floatingAmplitudeZ = 3f;
        public float floatingFreqX = 1f;
        public float floatingAmplitudeX = 2;
        public Vector3 offset;

        protected override void Tick()
        {
            FloatMove();
        }

        public void FloatMove()
        {
            var z = Mathf.Sin(com.GameTime.time * floatingFreqZ) * floatingAmplitudeZ;
            var x = Mathf.Sin(com.GameTime.time * floatingFreqX) * floatingAmplitudeX;
            floatingTrans.localEulerAngles = new Vector3(x, 0, z) + offset;
        }
    }
}
