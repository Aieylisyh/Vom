using UnityEngine;

namespace vom
{
    public class EnemyDeathBehaviour : VomEnemyComponent
    {
        public bool dead { get; private set; }

        public float bodyTime = 5;
        public bool bodyCanSink = true;

        float _bodyTimer;
        float _sinkAcc;
        float _sinkSpeed;

        void Start()
        {
            dead = false;
            _sinkSpeed = 0;
            _sinkAcc = game.ConfigService.instance.combatConfig.enemy.sinkAcc;
            _bodyTimer = bodyTime;
        }

        protected override void Update()
        {
            if (dead)
            {
                if (!bodyCanSink)
                    return;

                var dt = com.GameTime.deltaTime;
                if (_bodyTimer > 0)
                {
                    _bodyTimer -= dt;
                    return;
                }

                _sinkSpeed += _sinkAcc * dt;
                transform.position += Vector3.down * dt * _sinkSpeed;

                if (transform.position.y < -2)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Die()
        {
            dead = true;
            host.health.bar.Hide();
            host.animator.SetTrigger("Die");
            host.targetSearcher.ExitAlert();

            host.cc.enabled = false;
            //var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
            //go.SetActive(true);
            for (int i = 0; i < 1; i++)
            {
                LootSystem.instance.SpawnLoot(transform.position, new ItemData(1, "Soul"));
            }
            //HeartDistortSystem.instance.Create(this.transform, 20, 1f);
        }
    }
}