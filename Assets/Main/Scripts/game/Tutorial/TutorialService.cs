using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class TutorialService : MonoBehaviour
    {
        public static TutorialService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public bool IsCampaignLimitEnabled()
        {
            var li = LevelService.instance.GetNextCampaignLevelIndex();
            var cfg = ConfigService.instance.tutorialConfig;

            return li >= cfg.minLevelIndexEnableFunctionsData.playCountLimit;
        }

        public void RecordLevelPlayed(bool win)
        {
            var crtLevelItem = LevelService.instance.runtimeLevel.levelItem;
            UxService.instance.SyncLevelPlayedCount(crtLevelItem);

            if (IsCampaignLimitEnabled())
            {
                if (win && !crtLevelItem.passed)
                {
                    //You passed this level for the first time\n\nYou receive 1 free challenge chance!
                }
                else
                {
                    crtLevelItem.saveData.lastPlayedCount += 1;
                    UxService.instance.gameDataCache.cache.lastPlayedCount += 1;
                }
            }

            UxService.instance.SaveGameData();
        }
    }
}
