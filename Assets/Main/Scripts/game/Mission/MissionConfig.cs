using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class MissionConfig : ScriptableObject
    {
        public List<MissionPrototype> mainline;

        public List<DlMissions> daily;

        //public int dailyCount;

        [System.Serializable]
        public struct DlMissions
        {
            public List<MissionPrototype> ms;
        }
    }
}