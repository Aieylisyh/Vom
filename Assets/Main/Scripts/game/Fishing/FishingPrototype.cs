using System.Collections.Generic;
using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class FishingPrototype : ScriptableObject
    {
        public List<Item> extraReward;
        public float rewardRatio = 1;
        public int durationSeconds;
        public bool canAcc;

        public int tutoIndex = -1;
    }
}
