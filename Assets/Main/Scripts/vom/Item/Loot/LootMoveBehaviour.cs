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
        protected Transform trans;

        public virtual void Init(Vector3 pos, LootModelSwitcherBehaviour switcher)
        {
            if (switcher == null)
            {
                _rb = GetComponent<Rigidbody>();
                _col = GetComponent<Collider>();
                trans = transform;
            }
            else
            {
                _rb = switcher.crt.GetComponent<Rigidbody>();
                _col = switcher.crt.GetComponent<Collider>();
                trans = switcher.crt.transform;
            }

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

        public bool CanReceive()
        {
            return (_dropTimer <= 0 && _waitTimer <= 0);
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

        protected Transform TargetTrans { get { return PlayerBehaviour.instance.transform; } }
    }
}
