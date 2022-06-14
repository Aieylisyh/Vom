namespace game
{
    public class PlayIsland : IslandBehaviour
    {
        public override void ClickFunction()
        {
            int levelPassIndex = LevelService.instance.GetNextCampaignLevelIndex();
            var cfg = ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData;
            if (levelPassIndex >= cfg.map)
            {
                WindowService.instance.ShowMap();
            }
            else
            {
                LevelService.instance.EnterCampaignLevel(ConfigService.instance.levelConfig.campaignLevels[0].id);
                WindowService.instance.HideAllWindows();
                MissionService.instance.PushMl("tuto", 1, true);
            }
        }
    }
}
