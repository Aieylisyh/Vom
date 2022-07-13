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
        public bool normalAttackIsRanged;

        public List<DropPrototype> drops;
        public int exp;
        public int soul;

        public int hp;
        public int attack;
        public float speed;
        public float attackInterval = 1.4f;
        public AttackRange sightRange = AttackRange.MidLong;
        public float bodyTime = 5;
        public bool bodyCanSink = true;
        public int soulLootCount = 1;//how many soul view
        public int expLootCount = 1;//how many exp view

        public bool noAlert;
    }
}