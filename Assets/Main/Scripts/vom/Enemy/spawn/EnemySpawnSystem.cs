using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EnemySpawnSystem : MonoBehaviour
    {
        public List<Transform> enemieSpawnPosList;
        public List<GameObject> enemyPrefabList;

        private void Awake()
        {
        }

        private void Start()
        {
        }

        public void SpawnEnemies()
        {
            foreach (var pos in enemieSpawnPosList)
            {

            }
        }
    }
}