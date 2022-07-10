using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class EnemyPrototype : ScriptableObject
    {
        public EnemyBehaviour prefab;

        public List<DropPrototype> drops;

        public int hp;
        public int attack;

    }
}
