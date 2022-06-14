using UnityEngine;

namespace com
{
    public class CameraShake : MonoBehaviour
    {
        public int Turn = 5;
        public float Amplitude = 4;
        public Transform self;
        public float Time = 1;
        private float _timer;

        public static CameraShake Instance;
        public static CameraShake InstanceStrong;
        public bool isStrongInstance;

        void Start()
        {
            _timer = 0;
            if (isStrongInstance)
            {
                InstanceStrong = this;
            }
            else
            {
                Instance = this;
            }
        }

        void Update()
        {
            if (_timer > 0)
            {
                float t = _timer / Time;
                float f = 1 - Mathf.Abs(2 * t - 1f);
                float amp = f * Amplitude;
                self.localPosition = Vector3.up * amp * Mathf.Sin(Mathf.PI * t * 2 * Turn);
                //Debug.Log("amp " + amp);
                _timer -= UnityEngine.Time.deltaTime;

                if (_timer <= 0)
                {
                    self.localPosition = Vector3.zero;
                }
            }
        }
        public void Shake()
        {
            _timer = Time;
        }
    }
}