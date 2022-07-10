using UnityEngine;

namespace vom
{
    public class DailyPerkSystem : MonoBehaviour
    {
        public static DailyPerkSystem instance { get; private set; }

        public int exp { get; private set; }
        public int level { get; private set; }
        public int expMax { get; private set; }

        private void Awake()
        {
            instance = this;

            exp = 235;
            expMax = 500;
            level = 3;
        }
    }
}