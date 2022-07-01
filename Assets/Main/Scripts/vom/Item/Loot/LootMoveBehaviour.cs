using UnityEngine;

namespace vom
{
    public class LootMoveBehaviour : MonoBehaviour
    {
        public float dropTime;
        protected float _dropTimer;
        public float dropTimeRandomnizer = 0.5f;

        public float waitTime;
        public float waitTimeRandomnizer = 0.5f;
        protected float _waitTimer;

        public float absorbAcc;
        public float absorbSpeedMax;
        protected float _absorbSpeed;

        protected Rigidbody _rb;
        
        protected Collider _col;
        protected Vector3 _tempDir;

        public virtual void Init(int dropIndex, Vector3 pos)
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();

            _dropTimer = dropTime + Random.Range(0, dropTimeRandomnizer); ;
            _waitTimer = 0;
        }

        protected virtual void Drop()
        {
            var dt = com.GameTime.deltaTime;

            _dropTimer -= dt;
            if (_dropTimer <= 0)
            {
                _dropTimer = 0;
                _waitTimer = waitTime + Random.Range(0, waitTimeRandomnizer);

                StartWait();
            }
        }

        protected virtual void Wait()
        {
            var dt = com.GameTime.deltaTime;

            _waitTimer -= dt;
            if (_waitTimer <= 0)
            {
                _waitTimer = 0;
                _absorbSpeed = 0;

                StartAbsorb();
            }
        }

        protected virtual void StartWait()
        {
        }

        protected virtual void StartAbsorb()
        {
        }

        protected virtual void Absorb()
        {
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
