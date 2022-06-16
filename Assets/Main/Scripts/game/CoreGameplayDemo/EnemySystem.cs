using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EnemySystem : MonoBehaviour
    {
        public static EnemySystem instance { get; private set; }
        //public List<EnemyBehaviour> enemies { get; private set; }
        public List<EnemyBehaviour> enemies;

        private void Awake()
        {
            instance = this;
        }

        public void SpawnEnemies()
        {

        }
    }
}