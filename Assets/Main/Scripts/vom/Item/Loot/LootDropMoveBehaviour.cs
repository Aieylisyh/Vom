using UnityEngine;

namespace vom
{
    public class LootDropMoveBehaviour : LootMoveBehaviour
    {
        public float force;
        public float torque = 200;

        public override void Init(Vector3 pos)
        {
            base.Init(pos);

            var radian = Mathf.PI * Random.value * 2;
            _tempDir = Vector3.up;
            _tempDir += Vector3.right * Mathf.Sin(radian) + Vector3.forward * Mathf.Cos(radian);
            _tempDir.Normalize();
            transform.position = pos + _tempDir * 1;

            _rb.useGravity = true;
            _rb.isKinematic = false;
            _rb.AddForce(_tempDir * force);

            if (torque > 0)
            {
                _rb.AddTorque(torque, torque, torque);
            }
        }

        protected override void StartAbsorb()
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _col.isTrigger = true;
        }

        protected override void Absorb()
        {
            var dt = com.GameTime.deltaTime;

            if (_absorbSpeed < absorbSpeedMax)
                _absorbSpeed += absorbAcc * dt;

            var targetPos = PlayerBehaviour.instance.transform.position + Vector3.up * 0.5f;
            var absorbDir = targetPos - transform.position;

            var dist = absorbDir.normalized;
            if (_absorbSpeed * dt > absorbDir.magnitude)
            {
                dist *= absorbDir.magnitude;
            }
            else
            {
                dist *= _absorbSpeed * dt;
            }

            transform.position += dist;
        }
    }
}
