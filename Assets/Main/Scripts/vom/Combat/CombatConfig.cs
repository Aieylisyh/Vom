using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class CombatConfig : ScriptableObject
    {
        public OrbsParam orbs;

        public enemyParam enemy;

        public int blockerLayerMask = 3;
    }

    [System.Serializable]
    public struct enemyParam
    {
        public float alertTime;
        public float sinkAcc;
    }

    [System.Serializable]
    public struct OrbsParam
    {
        public float rotateDegreeSpeed;
        public float orbitalRadius;
        public float orbitalHeightAdd;
        public float orbitalStartHeight;
        public float startPositioningTime;
    }
}