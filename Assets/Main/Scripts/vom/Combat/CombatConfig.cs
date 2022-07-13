using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    [CreateAssetMenu]
    public class CombatConfig : ScriptableObject
    {
        public OrbsParam orbs;

        public int blockerLayerMask = 3;

        [Range(0.5f,0.9f)]
        public float meleeAttackRadioRangeRatio = 0.6f;
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