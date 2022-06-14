using UnityEngine;

namespace game
{
    [System.Serializable]
    public class TalentItem
    {
        public string id;
        public TalentSaveData saveData;

        public TalentItem(string pId)
        {
            id = pId;
            saveData = new TalentSaveData();
        }

        [System.Serializable]
        public class TalentSaveData
        {
            public int level;

            public TalentSaveData()
            {
                level = 0;
            }
        }
    }
}
