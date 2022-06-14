using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    [System.Serializable]
    public class BossPrototype : EnemyPrototype
    {
        public int reg;
        public int armor;

        public BossLevelupData bossLevelupData;

        public int GetArmor(int level = 0)
        {
            return MathGame.GetPercentageAdded(armor, level * bossLevelupData.armorBoost);
        }

        public int GetReg(int level = 0)
        {
            return MathGame.GetPercentageAdded(reg, level * bossLevelupData.regBoost);
        }

        [System.Serializable]
        public struct BossLevelupData
        {
            public float armorBoost;
            public float regBoost;
        }
    }
}