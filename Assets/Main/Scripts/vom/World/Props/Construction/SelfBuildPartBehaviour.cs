using UnityEngine;
using com;

namespace vom
{
    public class SelfBuildPartBehaviour : MonoBehaviour
    {
        public AnimationCurve ac;
        public float duration;
        public Transform buildTrans;
        public Transform unBuildTrans;

        private float _timer;
        private bool _building;
        private bool _buildingReversed;

        void Start()
        {
            _building = false;
            _buildingReversed = false;
            _timer = 0;

            SetUnbuildState();
        }

        void Update()
        {
            if (_building)
            {
                _timer -= GameTime.deltaTime;

                if (_timer <= 0)
                {
                    _building = false;
                    if (_buildingReversed)
                        SetUnbuildState();
                    else
                        SetBuildState();
                    return;
                }

                var t = _timer / duration;
                if (_buildingReversed)
                    t = 1 - t;

                var e = ac.Evaluate(t);

                transform.SetPositionAndRotation(
                    Vector3.Lerp(buildTrans.position, unBuildTrans.position, e),
                        Quaternion.Lerp(buildTrans.rotation, unBuildTrans.rotation, e));
            }
        }

        public void Build()
        {
            _building = true;
            _buildingReversed = false;
            _timer = duration;
        }

        public void UnBuild()
        {
            _building = true;
            _buildingReversed = true;
            _timer = duration;
        }

        void SetBuildState()
        {
            transform.SetPositionAndRotation(buildTrans.position, buildTrans.rotation);
        }

        void SetUnbuildState()
        {
            transform.SetPositionAndRotation(unBuildTrans.position, unBuildTrans.rotation);
        }
    }
}