using UnityEngine;
using System.Collections.Generic;

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

        Rigidbody _rb;
        public float force;
        Collider _col;

        public void Init(int dropIndex, Vector3 pos)
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();

            _dropTimer = dropTime + Random.Range(0, dropTimeRandomnizer); ;
            _waitTimer = 0;

            var radian = dropIndex * 0.6f;
            //Random.value;??
            Vector3 tempDir = Vector3.up;
            tempDir += Vector3.right * Mathf.Sin(radian) + Vector3.forward * Mathf.Cos(radian);
            tempDir.Normalize();
            transform.position = pos + tempDir * 1;

            _rb.useGravity = true;
            _rb.isKinematic = false;
            _rb.AddForce(tempDir * force);
            _rb.AddTorque(200, 200, 200);
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

                _rb.useGravity = false;
                _rb.isKinematic = true;
                _col.isTrigger = true;
            }
        }

        private void Absorb()
        {
            var dt = com.GameTime.deltaTime;

            if (_absorbSpeed < absorbSpeedMax)
                _absorbSpeed += absorbAcc * dt;

            var targetPos = PlayerBehaviour.instance.transform.position + Vector3.up * 0.5f;
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

    [System.Serializable]
    public struct pickInfo
    {
        public List<string> ids;
        public List<GameObject> gos;

        public void Close()
        {
            foreach (var go in gos)
            {
                go.SetActive(false);
            }
        }
    }
}
