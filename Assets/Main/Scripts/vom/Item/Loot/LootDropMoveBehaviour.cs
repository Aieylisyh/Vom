using UnityEngine;

namespace vom
{
    public class LootDropMoveBehaviour : LootMoveBehaviour
    {
        public override void Init(int dropIndex, Vector3 pos)
        {
            base.Init(dropIndex, pos);

            _rb.useGravity = true;
            _rb.isKinematic = false;
            _rb.AddForce(_tempDir * force);
            _rb.AddTorque(200, 200, 200);
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
            transform.position += absorbDir.normalized * _absorbSpeed * dt;
        }
    }
}
