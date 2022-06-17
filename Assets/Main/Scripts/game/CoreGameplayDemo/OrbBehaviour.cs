using UnityEngine;
using System.Collections;
using game;

namespace vom
{
    public class OrbBehaviour : MonoBehaviour
    {
        public float rotateDegreeSpeed;
        public float releaseSpeed;
        public float orbitalRadius;
        public Vector3 orbitalOffset;

        public Transform orbitalHost { get; private set; }
        public float orbitalDegree { get; private set; }
        private bool _isOrbital;

        public Transform releaseTarget { get; private set; }
        public TrailRenderer releaseTrait;
        public ParticleSystem releasingPs;
        public AnimationCurve releaseCurveAc;//not the height. but the height delta(induct)
        private float _expectedReleaseTime;
        private float _expectedReleaseTimer;
        private Vector3 _releaseDir;
        private Vector3 _releaseTempPos;

        public string releasingSound;

        public GameObject dieVFX;
        public float releaseHeightOffsetRatioByDistance;
        private float _releaseHeightOffset;

        public RotateAlignMove rotateAlignMove;

        private void OnTriggerEnter(Collider other)
        {
            if (_isOrbital)
                return;

            if (other.gameObject.layer == 3)
            {
                Debug.Log("hit block " + other.gameObject);
                Die(false);
            }
            else
            {
                var ene = other.GetComponent<EnemyBehaviour>();
                if (ene != null)
                {
                    Debug.Log("hit ene" + other.gameObject);
                    //TODO deal damage
                    Die(false);
                }
            }
        }

        public void SetOrbital(float deg, Transform center)
        {
            orbitalHost = center;
            orbitalDegree = deg;
            releaseTrait.enabled = false;
            _isOrbital = true;
        }

        public void SetRelease(Transform target)
        {
            //ReleaseFromOrbital
            releaseTarget = target;
            releaseTrait.enabled = true;
            _isOrbital = false;

            releasingPs.Play();
            com.SoundService.instance.Play(releasingSound);

            _releaseTempPos = transform.position;
            _releaseDir = target.position - _releaseTempPos;
            //var dirNoY = dir;
            //dirNoY.y = 0;
            //var distNoY = dirNoY.magnitude;
            // _releaseHeightOffset = distNoY * releaseHeightOffsetRatioByDistance;
            var dist = _releaseDir.magnitude;
            _releaseHeightOffset = dist * releaseHeightOffsetRatioByDistance;
            _expectedReleaseTime = dist / releaseSpeed;
            _expectedReleaseTimer = 0;
        }

        void SyncOrbitalPos()
        {
            transform.position = orbitalHost.position + orbitalOffset +
                Vector3.right * Mathf.Sin(Mathf.Deg2Rad * orbitalDegree) * orbitalRadius +
                    Vector3.forward * Mathf.Cos(Mathf.Deg2Rad * orbitalDegree) * orbitalRadius;
        }

        public void Die(bool silent)
        {
            if (silent)
            {
                Destroy(gameObject);
            }
            else
            {
                var vfx = Instantiate(dieVFX, transform.position, Quaternion.identity, this.transform.parent);
                vfx.SetActive(true);
                Destroy(gameObject);
            }
        }

        void Update()
        {
            Move();
        }

        void Move()
        {
            if (_isOrbital)
            {
                orbitalDegree += Time.deltaTime * rotateDegreeSpeed;
                var oldPos = transform.position;
                SyncOrbitalPos();
                //rotateAlignMove.Rotate(transform.position - oldPos);
            }
            else
            {

                _releaseTempPos += _releaseDir.normalized * releaseSpeed * Time.deltaTime;

                var acv = releaseCurveAc.Evaluate(_expectedReleaseTimer / _expectedReleaseTime);
                var newPos = _releaseTempPos + acv * _releaseHeightOffset * Vector3.up;
                var dir = newPos - transform.position;
                transform.position = newPos;

                //stop rotate immidiately and go a curve not staight line!
                //if the target moves, will not follow so just do the collision test!

                _expectedReleaseTimer += Time.deltaTime;

                rotateAlignMove.Rotate(dir);
            }
        }
    }
}