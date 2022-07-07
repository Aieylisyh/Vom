using UnityEngine;
using com;

namespace vom
{
    public class ArcaneBlastOrbBehaviour : MonoBehaviour
    {
        float _timer;
        float _intervalTimer;
        public SkillPrototype skl;

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
                player.attack.orbs.LaunchArcaneBlast(target.position);
            }
        }
    }
}