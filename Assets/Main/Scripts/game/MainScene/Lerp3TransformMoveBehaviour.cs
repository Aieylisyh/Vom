using UnityEngine;

namespace com
{
    public class Lerp3TransformMoveBehaviour : MonoBehaviour     // bezier lerp
    {
        public Transform pos1;
        public Transform pos2;
        public Transform pos3;

        public Transform target;

        public float leaveTime = 2f;
        public float enterTime = 3.5f;

        private float _timer;
        private float _time;
        public AnimationCurve ac;

        void Update()
        {
            if (_timer <= 0)
                return;

            _timer -= Time.deltaTime;
            if (_timer <= 0)
                _timer = 0;

            float f = ac.Evaluate(_timer / _time);
            MoveTarget(f);
        }

        private void MoveTarget(float f)
        {
            if (isBack)
                f = 1 - f;

            target.SetPositionAndRotation(
                MathGame.Lerp3Bezier(pos1.position, pos2.position, pos3.position, f),
                MathGame.Lerp3Bezier(pos1.rotation, pos2.rotation, pos3.rotation, f));
        }

        bool isBack
        {
            get
            {
                return false;
            }
        }
    }
}
