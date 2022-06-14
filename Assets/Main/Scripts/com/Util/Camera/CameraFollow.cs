using UnityEngine;
using System.Collections;

namespace com
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform TargetTrans;
        public Transform SelfTrans;
        private Vector3 _offset;
        public float FollowLerpFactor = 0.1f;
        public Vector3 Offset;
        private Vector3 posLastClipping;

        void Update()
        {
            SelfTrans.position = Vector3.Lerp(SelfTrans.position, TargetTrans.position + Offset, FollowLerpFactor);
        }

        private void Start()
        {
            posLastClipping = SelfTrans.position;
        }
    }
}
