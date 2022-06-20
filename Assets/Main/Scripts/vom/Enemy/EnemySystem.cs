using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EnemySystem : MonoBehaviour
    {
        public static EnemySystem instance { get; private set; }

        public List<EnemyBehaviour> enemies;

        public EnemySpawnSystem spawnSys;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
           // enemies = new List<EnemyBehaviour>();

            spawnSys.SpawnEnemies();
        }

        public void AddEnemy(EnemyBehaviour e)
        {
            enemies.Add(e);
        }
    }
}