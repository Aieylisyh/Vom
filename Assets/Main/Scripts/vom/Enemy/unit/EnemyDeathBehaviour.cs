using com;
using UnityEngine;

namespace vom
{
    public class EnemyDeathBehaviour : VomEnemyComponent
    {
        public bool dead { get; private set; }
        bool _sink;
        float _bodyTimer;
        float _sinkAcc;
        float _sinkSpeed;
        public GameObject deathVfx;

        public override void ResetState()
        {
            dead = false;
            _sinkSpeed = 0;
            _sinkAcc = ConfigSystem.instance.enemyConfig.sinkAcc;
            _bodyTimer = host.proto.bodyTime;
            _sink = host.proto.bodyCanSink;
        }

        protected override void Update()
        {
            if (dead)
            {
                if (!_sink)
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
            host.animator.SetTrigger(EnemyAnimeParams.Die);
            host.targetSearcher.ExitAlert();
            SoundService.instance.Play(new string[2] { "mob die1", "mob die2" });
            host.cc.enabled = false;

            if (deathVfx != null)
            {
                var go = Instantiate(deathVfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
                go.SetActive(true);
            }

            SpawnLoots();
        }

        void SpawnLoots()
        {
            for (int i = 0; i < host.proto.expLootCount; i++)
            {
                LootSystem.instance.SpawnLoot(transform.position, new ItemData(i == 0 ? host.proto.exp : 0, "Exp"));
            }
            for (int i = 0; i < host.proto.soulLootCount; i++)
            {
                LootSystem.instance.SpawnLoot(transform.position, new ItemData(i == 0 ? host.proto.soul : 0, "Soul"));
            }

            //var drop = host.proto.drops;
            //TODO loot item
        }
    }
}