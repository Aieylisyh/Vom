using System.Collections.Generic;
using System;

namespace game
{
    [System.Serializable]
    public class FishingItem
    {
        public FishingSaveData saveData;

        public FishingItem()
        {
            saveData = new FishingSaveData();
        }
    }


    [System.Serializable]
    public class FishingSaveData
    {
        public bool hasRft;
        public long flStartDate;
        public int rftIndex;

        public DateTime startDate
        {
            get
            {
                return new DateTime(flStartDate);
            }
            set
            {
                flStartDate = value.Ticks;
            }
        }

        public int boatLevel;
        public List<string> unlockedAbilities;

        public FishingSaveData()
        {
            hasRft = false;
            boatLevel = 0;
            unlockedAbilities = new List<string>();
            rftIndex = 0;
        }
    }
}
