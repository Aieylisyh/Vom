using UnityEngine;
using System.Collections.Generic;

namespace vom
{

    public class EnemySystem : MonoBehaviour
    {
        public static EnemySystem instance { get; private set; }

        public List<EnemyBehaviour> enemies { get; private set; }

        public EnemySpawnSystem spawnSys;
        public Transform attackSpace;

        private void Awake()
        {
            instance = this;
            enemies = new List<EnemyBehaviour>();
        }

        private void Start()
        {
            spawnSys.SpawnEnemies();
        }

        public void AddEnemy(EnemyBehaviour e)
        {
            enemies.Add(e);
        }

        public void RemoveEnemy(EnemyBehaviour e)
        {
            enemies.Remove(e);
        }

        public bool HasEnemyTargetedPlayer()
        {
            foreach (var e in enemies)
            {
                if (e.gameObject.activeSelf && !e.health.dead && e.targetSearcher.alerted)
                {
                    return true;
                }
            }

            return false;
        }
    }
}