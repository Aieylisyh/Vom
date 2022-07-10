using UnityEngine;

namespace vom
{
    public class AchievementSystem : MonoBehaviour
    {
        public static AchievementSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }
    }
}
