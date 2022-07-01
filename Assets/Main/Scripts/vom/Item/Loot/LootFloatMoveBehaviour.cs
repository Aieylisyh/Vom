using UnityEngine;
using com;

namespace vom
{
    public class LootFloatMoveBehaviour : LootMoveBehaviour
    {
        public float speed;

        Vector3 _releaseOffsetDir;

        public float releaseOffset;

        public AnimationCurve releaseCurveAc;

        private Vector3 _releaseTempPos;

        public override void Init(int dropIndex, Vector3 pos)
        {
            base.Init(dropIndex, pos);

            _rb.useGravity = false;
            _rb.isKinematic = true;
            _col.isTrigger = true;

            var radian = dropIndex * 0.6f;
            //Random.value;??
            _tempDir = Vector3.right * Mathf.Sin(radian) + Vector3.forward * Mathf.Cos(radian) + Vector3.up * 1.3f;
            _tempDir.Normalize();
            transform.position = pos + _tempDir * 1;

            _releaseTempPos = transform.position;

            var offset1 = Vector3.Cross(Vector3.up, _tempDir).normalized;
            var r = Random.Range(-1f, 1f);
            _releaseOffsetDir = Vector3.up * (1 - Mathf.Abs(r)) + offset1 * r;
        }

        protected override void StartAbsorb()
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _col.isTrigger = true;
        }

        protected override void Absorb()
        {
            var dt = GameTime.deltaTime;

            if (_absorbSpeed < absorbSpeedMax)
                _absorbSpeed += absorbAcc * dt;

            var targetPos = PlayerBehaviour.instance.transform.position + Vector3.up * 0.4f;
            var absorbDir = targetPos - transform.position;
            transform.position += absorbDir.normalized * _absorbSpeed * dt;
        }

        protected override void Drop()
        {
            base.Drop();

            _releaseTempPos += _tempDir.normalized * speed * GameTime.deltaTime;

            var acv = releaseCurveAc.Evaluate(1 - _dropTimer / dropTime);
            var newPos = _releaseTempPos + acv * releaseOffset * _releaseOffsetDir;
            var dir = newPos - transform.position;
            transform.position = newPos;
        }

        protected override void Wait()
        {
            base.Wait();
        }
    }
}
