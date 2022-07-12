using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        public List<EnemyPrototype> enemies;

        public EnemySizes sizes;
    }

    [System.Serializable]
    public struct EnemySizes
    {
        public float tiny;
        public float small;
        public float mid;
        public float big;
        public float huge;
    }

    public enum EnemySize
    {
        Tiny,
        Small,
        Mid,
        Big,
        Huge,
    }
}