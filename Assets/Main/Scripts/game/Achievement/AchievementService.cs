using UnityEngine;

namespace game
{
    public class AchievementService : MonoBehaviour
    {
        public static AchievementService instance;

        private void Awake()
        {
            instance = this;
        }

        public AchievementConfig.AchievementItem GetItem(string id)
        {
            foreach (var i in ConfigService.instance.achievementConfig.list)
            {
                if (i.id == id)
                {
                    return i;
                }
            }
            return null;
        }
    }


}
