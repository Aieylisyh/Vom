using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class EnemyPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }

        public EnemyBehaviour prefab;

        public List<DropPrototype> drops;
        public int exp;
        public int soul;

        public int hp;
        public int attack;
        public int speed;
    }
}
