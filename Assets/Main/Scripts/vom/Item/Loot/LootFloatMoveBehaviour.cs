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

        public TrailRenderer trait;

        public override void Init(Vector3 pos)
        {
            base.Init(pos);

            _rb.useGravity = false;
            _rb.isKinematic = true;
            _col.isTrigger = true;
            if (trait != null)
                trait.emitting = true;

            //var radian = Mathf.PI * Random.value * 2;
            var radian = Mathf.PI * Random.Range(0.5f, 1.5f);
            _tempDir = Vector3.right * Mathf.Sin(radian) + Vector3.forward * Mathf.Cos(radian) + Vector3.up * 0.2f;
            _tempDir.Normalize();
            transform.position = pos + _tempDir * 1;

            _releaseTempPos = transform.position;

            var offset1 = Vector3.Cross(Vector3.up, _tempDir).normalized;
            var r = Random.Range(-1f, 1f);
            _releaseOffsetDir = Vector3.up * (1 - Mathf.Abs(r)) + offset1 * r;
        }

        protected override void Absorb()
        {
            var dt = GameTime.deltaTime;

            if (_absorbSpeed < absorbSpeedMax)
                _absorbSpeed += absorbAcc * dt;

            var targetPos = PlayerBehaviour.instance.transform.position + Vector3.up * 0.4f;
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

        protected override void Drop()
        {
            base.Drop();

            _releaseTempPos += _tempDir.normalized * speed * GameTime.deltaTime;

            var acv = releaseCurveAc.Evaluate(1 - _dropTimer / dropTime);
            var newPos = _releaseTempPos + acv * releaseOffset * _releaseOffsetDir;
            transform.position = newPos;
        }

        protected override void StartWait()
        {
            _releaseTempPos = transform.position;
        }

        protected override void StartAbsorb()
        {
            if (trait != null)
                trait.emitting = false;
        }

        protected override void Wait()
        {
            base.Wait();

            var targetPos = PlayerBehaviour.instance.transform.position + Vector3.up * 0.4f;
            var absorbDir = targetPos - transform.position;
            _releaseTempPos += absorbDir.normalized * speed * GameTime.deltaTime;
            var acv = releaseCurveAc.Evaluate(1 - _waitTimer / waitTime);
            var newPos = _releaseTempPos + acv * 0.5f * Vector3.up;
            transform.position = newPos;
        }
    }
}
