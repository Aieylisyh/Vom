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
        public GameObject deathVfx;

        void Start()
        {
            dead = false;
            _sinkSpeed = 0;
            _sinkAcc = ConfigSystem.instance.combatConfig.enemy.sinkAcc;
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

            if (deathVfx != null)
            {
                var go = Instantiate(deathVfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
                go.SetActive(true);
            }

            LootSystem.instance.SpawnLoot(transform.position, new ItemData(host.proto.soul, "Soul"));
            LootSystem.instance.SpawnLoot(transform.position, new ItemData(host.proto.exp, "Exp"));
        }
    }
}