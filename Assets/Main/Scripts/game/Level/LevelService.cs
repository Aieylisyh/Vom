using com;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class LevelService : MonoBehaviour
    {
        public static LevelService instance { get; private set; }
        public MoveMaterialTiling mmt;
        public RuntimeLevel runtimeLevel { get; private set; }

        private string _levelId;

        public List<LevelPrototype> allLevels { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            allLevels = new List<LevelPrototype>();
            allLevels.AddRange(ConfigService.instance.levelConfig.otherLevels);
            allLevels.AddRange(ConfigService.instance.levelConfig.campaignLevels);
        }

        public void ClearLevel()
        {
            runtimeLevel = null;
            LevelTutorialService.instance.SetTuto(LevelTutorialService.TutoType.None);
            CombatService.instance.playerShip.ResetPlayerView(false);
            CombatService.instance.playerShip.InitData(false);
            CombatService.instance.ClearUnits();
            CombatService.instance.StopTimer();
            LevelHudBehaviour.instance.Hide();
        }

        public void Revive()
        {
            if (GameFlowService.instance.windowState != GameFlowService.WindowState.Gameplay)
                return;

            SoundService.instance.Play("revive");
            CombatService.instance.ClearUnits();
            LevelHudBehaviour.instance.SetupForCombat();
            CombatService.instance.StartTimer();
            runtimeLevel.OnRevive();
            CombatService.instance.playerShip.ResetPlayerView(true);
            CombatService.instance.playerShip.InitData(false);
        }

        public void CheckLevelPassedMission()
        {
            foreach (var cpLevel in ConfigService.instance.levelConfig.campaignLevels)
            {
                var item = GetLevelItem(cpLevel.id);
                if (item.passed)
                {
                    string missionId = "p-" + cpLevel.id;
                    MissionService.instance.PushMl(missionId, 1, false);
                }
            }
        }

        public void LeaveLevel()
        {
            if (GameFlowService.instance.windowState != GameFlowService.WindowState.Gameplay)
                return;

            CameraControllerBehaviour.instance.EnterPort();
            ClearLevel();
            GameFlowService.instance.SetInputState(GameFlowService.InputState.Forbidden);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Main);
        }

        public LevelItem GetLevelItem(string id)
        {
            var levels = UxService.instance.gameDataCache.cache.levels;
            foreach (var lv in levels)
            {
                if (lv != null && lv.id == id)
                    return lv;
            }

            var newLevel = new LevelItem(id);
            UxService.instance.gameDataCache.cache.levels.Add(newLevel);
            UxService.instance.SaveGameData();
            return newLevel;
        }

        public LevelItem GetLevelItemFromRawData(string id, IRuntimeDataCache<GameData> data)
        {
            var levels = data.cache.levels;
            foreach (var lv in levels)
            {
                if (lv != null && lv.id == id)
                    return lv;
            }

            return new LevelItem(id);
        }

        public int GetRuntimeCampaignLevelIndex()
        {
            return GetCampaignLevelIndex(runtimeLevel.levelProto);
        }

        public int GetCampaignLevelIndex(LevelPrototype levelProto)
        {
            var levelItems = UxService.instance.gameDataCache.cache.levels;
            int res = 0;
            foreach (var cpLevel in ConfigService.instance.levelConfig.campaignLevels)
            {
                if (cpLevel == levelProto)
                    return res;

                res++;
            }

            return -1;
        }

        public int GetCampaignLevelIndex(string levelId)
        {
            return GetCampaignLevelIndex(GetPrototype(levelId));
        }

        public int GetCampaignLevelStars(IRuntimeDataCache<GameData> data)
        {
            int res = 0;
            foreach (var cpLevel in ConfigService.instance.levelConfig.campaignLevels)
            {
                var item = GetLevelItemFromRawData(cpLevel.id, data);
                var star = item.saveData.highStar;
                res += star;
            }

            return res;
        }

        public int GetNextCampaignLevelIndex()
        {
            var levelItems = UxService.instance.gameDataCache.cache.levels;
            int res = 0;
            foreach (var cpLevel in ConfigService.instance.levelConfig.campaignLevels)
            {
                bool passed = false;
                foreach (var lvItem in levelItems)
                {
                    if (lvItem.id == cpLevel.id && lvItem.passed)
                    {
                        passed = true;
                        break;
                    }
                }

                if (passed)
                {
                    res++;
                }
                else
                {
                    break;
                }
            }

            return res;
        }

        public void FirstPassCrtLevel(ref Item cabReward)
        {
            //Debug.Log("first pass level");
            var proto = runtimeLevel.levelProto;
            var firstPassReward = proto.GetFirstPassReward();
            if (firstPassReward == null || firstPassReward.Count < 1)
                return;

            //send reward mail
            string pLevelName = runtimeLevel.levelProto.title;
            List<Item> rewards = new List<Item>();
            foreach (var reward in firstPassReward)
            {
                var protoReward = ItemService.instance.GetPrototype(reward.id);
                //Debug.Log(reward.id);
                if (protoReward.usage == ItemPrototype.Usage.Cab)
                {
                    cabReward = reward;
                    UxService.instance.AddItem(reward);
                }
                else
                {
                    rewards.Add(reward);
                }
            }
            MailService.instance.AddMailLevelPassed(pLevelName, rewards);
        }

        public string GetCampainLevelId(int index = -1)
        {
            if (!HasNextCampaignLevel())
            {
                Debug.LogError("GetCampainLevelId but all levels are passed!");
                index = -1;
            }

            if (index < 0)
                index = GetNextCampaignLevelIndex();

            return ConfigService.instance.levelConfig.campaignLevels[index].id;
            //return ConfigService.instance.levelConfig.compaignPrefix + (index + 1);
        }

        public void SetupNextCompaignLevel()
        {
            SetLevelId(GetCampainLevelId());
        }

        public void SetLevelId(string s)
        {
            _levelId = s;
        }

        public bool HasNextCampaignLevel()
        {
            var index = GetNextCampaignLevelIndex();
            return ConfigService.instance.levelConfig.campaignLevels.Count > index;
        }

        public void StartLevel()
        {
            //Debug.Log("StartLevel " + _levelId);
            runtimeLevel = new RuntimeLevel(_levelId);
            CombatService.instance.StartTimer();
            CombatAbilityService.instance.RefreshAndReset();
            CombatService.instance.CreatePlayerAttri();
        }

        public bool IsPediaLevel()
        {
            if (_levelId == "Pedia")
                return true;

            return false;
        }

        public int GetEnemyLevel()
        {
            if (runtimeLevel == null)
                return 0;

            return runtimeLevel.GetEnemyLevel();
        }

        public void AddLevelLoot(string id, int amount)
        {
            if (runtimeLevel == null)
                return;

            if (amount == 0)
                return;

            //Debug.Log("Loot: " + id + " " + amount);
            var parsedRes = ConfigService.instance.itemConfig.ParseComplexItem(id, amount);
            if (parsedRes != null && parsedRes.Count > 0)
            {
                foreach (var i in parsedRes)
                {
                    AddLevelLoot(i.id, i.n);
                }
                return;
            }

            var loots = runtimeLevel.totalLoot;
            foreach (var loot in loots)
            {
                if (loot.id == id)
                {
                    loot.n += amount;
                    return;
                }
            }

            runtimeLevel.totalLoot.Add(new Item(amount, id));
        }

        public void CombatUpdate(float timePassed)
        {
            runtimeLevel?.TriggerLevelEvent(timePassed);
        }

        public void DispatchLevelEvent(LevelEvent e)
        {
            //var s = "DispatchLevelEvent time:" + e.time + " -> " + e.evt;
            //if (e.floatParam != null && e.floatParam.Count > 0)
            //    s += " " + e.floatParam[0];
            //if (e.stringParam != null && e.stringParam.Count > 0)
            //    s += " " + e.stringParam[0];
            //Debug.Log(s);

            switch (e.evt)
            {
                case "init":
                    mmt.SetRandomSpeed();
                    CombatService.instance.playerShip.ResetPlayerView(true);
                    LevelHudBehaviour.instance.SetupForCombat();
                    break;

                case "scene":
                    CombatService.instance.playerShip.OpenLights();
                    break;

                case "cab":
                    StartCombatAbilitySelection();
                    break;

                case "tip":
                    var tipKey = e.stringParam[0];
                    string s = LocalizationService.instance.GetLocalizedText(tipKey);

                    switch (tipKey)
                    {
                        case "Tip_Move":
                            LevelTutorialService.instance.SetTuto(LevelTutorialService.TutoType.Move);
                            break;

                        case "Tip_Bomb":
                            LevelTutorialService.instance.SetTuto(LevelTutorialService.TutoType.Bomb);
                            break;

                        case "Tip_TorTrace":
                            LevelTutorialService.instance.SetTuto(LevelTutorialService.TutoType.TorTrace);
                            break;

                        case "Tip_TorDir":
                            LevelTutorialService.instance.SetTuto(LevelTutorialService.TutoType.TorDir);
                            break;
                    }

                    LevelHudBehaviour.instance.ShowTip(s, LevelTutorialService.instance.TryEndTuto);
                    break;

                case "final":
                    string descString = LocalizationService.instance.GetLocalizedText("FinalWave");
                    FloatingTextPanelBehaviour.instance.Create(descString, 0.5f, 0.46f, true);
                    break;

                case "ene":
                    string enemyId = e.stringParam[0];
                    var enemyGo = PoolingService.instance.GetInstance(enemyId);
                    Enemy enemy = enemyGo.GetComponent<Enemy>();
                    bool showupFromNear = false;
                    if (e.boolParam.Count > 1)
                        showupFromNear = e.boolParam[1];
                    //Debug.Log("ene " + enemy.gameObject.name);
                    enemy.Init(e.boolParam[0], e.floatParam[0], enemyId, showupFromNear);
                    break;
                    //some events event add a bruch of set event into the initial queue
                    //the simpliest event is log text
            }
        }

        public LevelPrototype GetPrototype(string id)
        {
            foreach (var i in allLevels)
            {
                if (i.id == id)
                    return i;
            }

            return null;
        }

        public void StartCombatAbilitySelection()
        {
            var picked = CombatAbilityService.instance.Get3ToSelect(0);
            if (picked == null || picked.Count != 3)
                return;

            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Paused);
            LevelHudBehaviour.instance.SetupForCab();
            CombatAbilitySelectionBehaviour.instance.Setup(picked);
            CombatAbilitySelectionBehaviour.instance.Show();
        }

        public void EndCombatAbilitySelection()
        {
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
            LevelHudBehaviour.instance.SetupEndCab();
        }

        public void EnterPediaLevel()
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Gameplay
            || GameFlowService.instance.gameFlowEvent == GameFlowService.GameFlowEvent.GoToCombat)
                return;

            WindowService.instance.HideAllWindows();
            ClearLevel();
            SetLevelId("Pedia");
           
            CameraControllerBehaviour.instance.LeavePort();
            GameFlowService.instance.SetInputState(GameFlowService.InputState.Forbidden);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Gameplay);
        }

        public void EnterCampaignLevel(string s = "")
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Gameplay
            || GameFlowService.instance.gameFlowEvent == GameFlowService.GameFlowEvent.GoToCombat)
                return;

            if (s == "")
            {
                SetupNextCompaignLevel();
            }
            else
            {
                SetLevelId(s);
            }

            ClearLevel();
            CameraControllerBehaviour.instance.LeavePort();
            GameFlowService.instance.SetInputState(GameFlowService.InputState.Forbidden);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Gameplay);
        }

        public int GetStarCount()
        {
            int res = 0;
            foreach (var lv in allLevels)
            {
                var item = GetLevelItem(lv.id);
                res += item.saveData.highStar;
            }

            return res;
        }

        public Item GetDailyChanceFullFillPrice()
        {
            var prices = ConfigService.instance.levelConfig.dailyChanceFullFillPrices;
            var times = UxService.instance.gameDataCache.cache.buyPlayChanceCount;
            if (times < prices.Count)
            {
                return prices[times];
            }
            return prices[prices.Count - 1];
        }
    }
}