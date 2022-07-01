using UnityEngine;
using com;
using game;

namespace vom
{
    public class OrbBehaviour : MonoBehaviour
    {
        public CameraShake.ShakeLevel hitShakeLevel;

        private float _rotateDegreeSpeed;
        public float releaseSpeed;
        private float _orbitalRadius;
        public Vector3 orbitalOffset;

        public Transform orbitalHost { get; private set; }
        public float orbitalDegree { get; private set; }
        private bool _isOrbital;

        public Vector3 releaseTargetPos { get; private set; }
        public TrailRenderer releaseTrait;
        public ParticleSystem releasingPs;
        public AnimationCurve releaseCurveAc;//not the height. but the height delta(induct)

        private float _expectedReleaseTime;
        private float _expectedReleaseTimer;
        private Vector3 _releaseDir;

        private Vector3 _releaseTempPos;

        public string releasingSound;
        public string explodeSound;

        public GameObject dieVFX;

        private float _orbitalStartHeight;
        private float _orbitalHeightAdd;
        public float releaseOffsetRatioByDistance;
        private float _releaseOffset;
        private Vector3 _releaseOffsetDir;

        private RotateAlignMove _rotateAlignMove;

        private AutoDestory _des;

        private float _startPositioningTimer;
        private float _startPositioningTime;
        [HideInInspector]
        public bool isEnemyShoot;

        public int dmg = 1;//test only
        bool _triggered;

        private void Awake()
        {
            _rotateAlignMove = GetComponent<RotateAlignMove>();
            _des = GetComponent<AutoDestory>();
            var cfg = ConfigService.instance.combatConfig;
            _rotateDegreeSpeed = cfg.orbs.rotateDegreeSpeed;
            _orbitalRadius = cfg.orbs.orbitalRadius;
            _orbitalStartHeight = cfg.orbs.orbitalStartHeight;
            _orbitalHeightAdd = cfg.orbs.orbitalHeightAdd;
            _startPositioningTime = cfg.orbs.startPositioningTime;
            _triggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;

            if (_isOrbital)
                return;

            if (other.gameObject.layer == ConfigService.instance.combatConfig.blockerLayerMask)
            {
                //Debug.Log("hit block " + other.gameObject);
                Die(false);
            }
            else
            {
                if (isEnemyShoot)
                {
                    var play = other.GetComponent<PlayerBehaviour>();
                    if (play != null)
                    {
                        _triggered = true;
                        play.OnHit(this);
                        Die(false);
                    }
                }
                else
                {
                    var ene = other.GetComponent<EnemyBehaviour>();
                    if (ene != null)
                    {
                        _triggered = true;
                        //Debug.Log("hit ene" + other.gameObject);
                        ene.OnHit(this);
                        Die(false);
                    }
                }

            }
        }

        public void SetOrbital(float deg, Transform center)
        {
            orbitalHost = center;
            orbitalDegree = deg;
            if (releaseTrait != null)
                releaseTrait.enabled = false;
            _isOrbital = true;

            _startPositioningTimer = _startPositioningTime;
        }

        public void SetRelease(Vector3 target)
        {
            //ReleaseFromOrbital
            releaseTargetPos = target;
            if (releaseTrait != null)
                releaseTrait.enabled = true;
            _isOrbital = false;

            if (releasingPs != null)
                releasingPs.Play();

            var offsetR =
            _des.enabled = true;
            SoundService.instance.Play(releasingSound);

            _releaseTempPos = transform.position;
            _releaseDir = target - _releaseTempPos;
            var offset1 = Vector3.Cross(Vector3.up, _releaseDir).normalized;
            var r = Random.Range(-1f, 1f);
            _releaseOffsetDir = Vector3.up * (1 - Mathf.Abs(r)) + offset1 * r;

            var dist = _releaseDir.magnitude;
            _releaseOffset = dist * releaseOffsetRatioByDistance;
            _expectedReleaseTime = dist / releaseSpeed;
            _expectedReleaseTimer = 0;
        }

        public bool IsReadyInOrbital()
        {
            if (_startPositioningTime > 0)
            {
                return _startPositioningTimer <= 0;
            }
            return true;
        }

        void SyncOrbitalPos()
        {
            float r = 0;
            if (_startPositioningTime > 0)
            {
                _startPositioningTimer -= GameTime.deltaTime;
                if (_startPositioningTimer < 0)
                {
                    _startPositioningTimer = 0;
                }
                r = _startPositioningTimer / _startPositioningTime;
            }

            transform.position = orbitalHost.position +
                orbitalOffset + Mathf.Lerp(_orbitalStartHeight, _orbitalStartHeight + _orbitalHeightAdd, r) * Vector3.up +
            (Vector3.right * Mathf.Sin(Mathf.Deg2Rad * orbitalDegree) +
            Vector3.forward * Mathf.Cos(Mathf.Deg2Rad * orbitalDegree)) *
            Mathf.Lerp(1, 0, r) * _orbitalRadius;
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
                SoundService.instance.Play(explodeSound);
                Destroy(gameObject, 1f);
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
                if (_startPositioningTimer <= 0)
                {
                    orbitalDegree += GameTime.deltaTime * _rotateDegreeSpeed;
                }

                //var oldPos = transform.position;
                SyncOrbitalPos();
                //rotateAlignMove.Rotate(transform.position - oldPos);
            }
            else
            {
                _releaseTempPos += _releaseDir.normalized * releaseSpeed * GameTime.deltaTime;

                var acv = releaseCurveAc.Evaluate(_expectedReleaseTimer / _expectedReleaseTime);
                var newPos = _releaseTempPos + acv * _releaseOffset * _releaseOffsetDir;
                var dir = newPos - transform.position;
                transform.position = newPos;

                //stop rotate immidiately and go a curve not staight line!
                //if the target moves, will not follow so just do the collision test!
                _expectedReleaseTimer += GameTime.deltaTime;

                _rotateAlignMove.Rotate(dir);
            }
        }
    }
}