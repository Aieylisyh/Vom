using UnityEngine;

namespace vom
{
    public class LootFloatMoveBehaviour : LootMoveBehaviour
    {
        public override void Init(int dropIndex, Vector3 pos)
        {
            base.Init(dropIndex, pos);
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
