using UnityEngine;
using com;

namespace vom
{
    public class ArcaneBlastOrbBehaviour : MonoBehaviour
    {
        float _timer;
        float _intervalTimer;
        public SkillPrototype skl;

        public GameObject arcaneBlast;

        void Start()
        {
            _timer = skl.duration;
            _intervalTimer = skl.interval;
        }

        void Update()
        {
            Charge();
        }

        void Charge()
        {
            _timer -= GameTime.deltaTime;
            if (_timer < 0)
            {
                Destroy(gameObject);
                return;
            }

            _intervalTimer -= GameTime.deltaTime;
            if (_intervalTimer <= 0)
            {
                _intervalTimer += skl.interval;
                TriggerCharge();
            }
        }

        void TriggerCharge()
        {
            var player = PlayerBehaviour.instance;
            var e = player.attack.searcher.GetTargetEnemy();
            if (e != null)
            {
                var target = e.transform;
                LaunchArcaneBlast(target.position);
            }
        }

        public void LaunchArcaneBlast(Vector3 targetPos)
        {
            SpawnShoot(arcaneBlast, targetPos);
        }

        void SpawnShoot(GameObject prefab, Vector3 targetPos)
        {
            GameObject shootGo = Instantiate(prefab, CombatSystem.instance.projectileSpace);
            shootGo.SetActive(true);
            shootGo.transform.position = transform.position;

            var shoot = shootGo.GetComponent<OrbBehaviour>();
            shoot.SetRelease(targetPos);
        }
    }
}