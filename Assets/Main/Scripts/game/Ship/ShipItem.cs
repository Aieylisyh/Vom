using System.Collections.Generic;
using UnityEngine;

namespace game
{
    [System.Serializable]
    public class ShipItem
    {
        public ShipSaveData saveData;
        public string id;
        public ShipItem(string pId)
        {
            id = pId;
            saveData = new ShipSaveData();
        }

        public bool HasUnlockedAbility(string s)
        {
            foreach (var ua in saveData.unlockedAbilities)
            {
                if (ua == s)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetAbilityLevel(string s)
        {
            int res = 1;
            for (int i = 0; i < saveData.unlockedAbilities.Count; i++)
            {
                var ua = saveData.unlockedAbilities[i];
                if (ua == s)
                {
                    res += 1;
                }
            }

            return res;
        }
    }

    [System.Serializable]
    public class ShipSaveData
    {
        public bool unlocked;
        public int level;
        public List<string> unlockedAbilities;

        //每天每船首战有100%的额外经验 触发后记录这个值
        //public int rawPlayedDays;

        public ShipSaveData()
        {
            unlocked = false;
            level = 1;
            unlockedAbilities = new List<string>();
            //rawPlayedDays = -1;
        }
    }
}
