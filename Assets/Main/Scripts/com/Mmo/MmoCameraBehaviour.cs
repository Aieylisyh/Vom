using UnityEngine;

namespace com
{
    public class MmoCameraBehaviour : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;

        [Range(0f, 1.5f)]
        public float pitch;
        [Range(-2f, 2f)]
        public float yaw;

        [Range(2f, 20f)]
        public float distance;

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
            var yawed = backward * Mathf.Cos(yaw) + Vector3.right * Mathf.Sin(yaw);
            var ideaPos = target.position + (yawed * Mathf.Cos(pitch) + Vector3.up * Mathf.Sin(pitch)) * distance;
            transform.position = ideaPos;
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.position += offset;
        }
    }
}