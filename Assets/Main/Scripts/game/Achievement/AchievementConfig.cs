using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class AchievementConfig : ScriptableObject
    {
        public List<AchievementItem> list;
        //彩蛋有成就：获得一枚价值100000的七彩珍珠。 集齐一册（6本）海草之王

        [System.Serializable]
        public class AchievementItem
        {
            public string id;
            public bool enabled;
            public string title;
            public string desc;
        }
    }
}