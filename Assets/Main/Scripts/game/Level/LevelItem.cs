using UnityEngine;

namespace game
{
    [System.Serializable]
    public class LevelItem
    {
        public string id;
        public LevelSaveData saveData;
        public bool passed
        {
            get { return saveData.highScore > 0; }
        }

        public LevelItem(string pId)
        {
            id = pId;
            saveData.highScore = 0;
            saveData.highStar = 0;
            saveData.unlocked = false;
            saveData.lastRawPlayedDays = -1;
            saveData.lastPlayedCount = 0;
        }

        [System.Serializable]
        public struct LevelSaveData
        {
            public int highScore;
            public int highStar;
            public bool unlocked;

            public int lastRawPlayedDays;
            public int lastPlayedCount;
        }
    }
}
