using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class EnemySpawnSystem : MonoBehaviour
    {
        public static EnemySpawnSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }
    }
}