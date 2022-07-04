using UnityEngine;

namespace vom
{
    public class DailyPerkSystem : MonoBehaviour
    {
        public static DailyPerkSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }
    }
}