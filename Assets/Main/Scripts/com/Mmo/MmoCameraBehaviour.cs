using UnityEngine;

namespace com
{
    public class MmoCameraBehaviour : MonoBehaviour
    {
        public Transform target;
        public MmoCameraParameters parameters;

        void Start()
        {
        }

        void Update()
        {
            Sync();
        }

        public void Sync()
        {
            // var backward = -target.forward;
            // var yawed = backward * Mathf.Cos(yaw) + target.right * Mathf.Sin(yaw);
            // var ideaPos = target.position + (yawed * Mathf.Cos(pitch) + Vector3.up * Mathf.Sin(pitch)) * distance;
            // transform.position = ideaPos;
            // transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            // transform.position += offset;

            var backward = -Vector3.forward;
            var yawed = backward * Mathf.Cos(parameters.yaw) + Vector3.right * Mathf.Sin(parameters.yaw);
            var ideaPos = target.position + (yawed * Mathf.Cos(parameters.pitch) + Vector3.up * Mathf.Sin(parameters.pitch)) * parameters.distance;
            transform.position = ideaPos;
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.position += parameters.offset;
        }
    }
}