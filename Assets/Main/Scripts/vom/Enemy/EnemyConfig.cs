using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        public List<EnemyPrototype> enemies;
    }
}
