using UnityEngine;

namespace game
{
    [System.Serializable]
    public class MissionItem
    {
        public string id;
        public MissionSaveData saveData;

        public MissionItem(MissionPrototype proto)
        {
            id = proto.id;
            saveData = new MissionSaveData(proto.content.quota);
        }

        public bool finished
        {
            get
            {
                return saveData.crt >= saveData.total;
            }
        }
    }

    [System.Serializable]
    public class MissionSaveData
    {
        public int crt;
        public int total;

        public MissionSaveData(int quota)
        {
            total = quota;
            crt = 0;
        }
    }
}