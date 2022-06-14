using System.Collections.Generic;
using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    public class LevelConfig : ScriptableObject
    {
        public string campaignPrefix = "cp";
        public float score3StarRatio;
        public float score2StarRatio;

        public List<Item> dailyChanceFullFillPrices;

        public int raidRewardBonusPercent1Star = 0;
        public int raidRewardBonusPercent2Star = 50;
        public int raidRewardBonusPercent3Star = 100;

        public int campaignLevelPlayLimit;
        public List<int> raidDiamondPrices = new List<int>();

        public List<LevelPrototype> otherLevels = new List<LevelPrototype>();
        public List<LevelPrototype> campaignLevels = new List<LevelPrototype>();

        public List<LevelEvent> defaultLevelEvents;

        public Formula idleRewardCountPerMinute;
        public List<Item> idleRewards;
        public int maxIdleHours;
    }
}