using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class CombatConfig : ScriptableObject
    {
        public OrbsParam orbs;
        public int blockerLayerMask = 3;
    }

    [System.Serializable]
    public struct OrbsParam
    {
        public float rotateDegreeSpeed;
        public float orbitalRadius;
    }
}