using UnityEngine;

namespace vom
{
    public class LootMoveBehaviour : MonoBehaviour
    {
        public float dropTime;
        private float _dropTimer;
        public float dropTimeRandomnizer = 0.5f;

        public float waitTime;
        public float waitTimeRandomnizer = 0.5f;
        private float _waitTimer;

        public float absorbAcc;
        public float absorbSpeedMax;
        private float _absorbSpeed;

        public Rigidbody rb;
        public float force;
        public Collider col;

        public void Init(int dropIndex)
        {
            _dropTimer = dropTime + Random.Range(0, dropTimeRandomnizer); ;
            _waitTimer = 0;

            float r1 = Random.value;
            r1 = r1 * 2 - 1;
            float r2 = Random.value;
            r2 = r2 * 2 - 1;
            //TODO dropIndex
            Vector3 tempDir = Vector3.up;
            tempDir += Vector3.right * r2 + Vector3.forward * r1;

            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(tempDir.normalized * force);
        }

        private void Drop()
        {
            var dt = com.GameTime.deltaTime;

            _dropTimer -= dt;
            if (_dropTimer <= 0)
            {
                _dropTimer = 0;
                _waitTimer = waitTime + Random.Range(0, waitTimeRandomnizer);
            }
        }

        private void Wait()
        {
            var dt = com.GameTime.deltaTime;

            _waitTimer -= dt;
            if (_waitTimer <= 0)
            {
                _waitTimer = 0;
                _absorbSpeed = 0;

                rb.useGravity = false;
                rb.isKinematic = true;
                col.isTrigger = true;
            }
        }

        private void Absorb()
        {
            var dt = com.GameTime.deltaTime;

            if (_absorbSpeed < absorbSpeedMax)
                _absorbSpeed += absorbAcc * dt;

            var targetPos = PlayerBehaviour.instance.transform.position;
            var absorbDir = targetPos - transform.position;
            transform.position += absorbDir.normalized * _absorbSpeed * dt;
        }

        void Update()
        {
            if (_dropTimer > 0)
                Drop();
            else if (_waitTimer > 0)
                Wait();
            else
                Absorb();
        }
    }
}
