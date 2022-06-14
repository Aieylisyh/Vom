using UnityEngine;
using com;
using System.Collections.Generic;

namespace game
{
    public class MissionService : MonoBehaviour
    {
        public static MissionService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public MissionConfig cfg
        {
            get
            {
                return ConfigService.instance.missionConfig;
            }
        }

        public bool HasFinishedDl()
        {
            CheckoutDaily();

            var cache = UxService.instance.gameDataCache.cache;
            return cache.dlDone >= cfg.daily.Count;
        }

        void CheckoutDaily()
        {
            //Debug.Log("CheckoutDaily");
            var tutoCfg = ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData;
            var li = LevelService.instance.GetNextCampaignLevelIndex();
            var rd = UxService.instance.GetRawPlayedDays();
            var cache = UxService.instance.gameDataCache.cache;

            bool hasDl = li >= tutoCfg.mission_dl;
            if (!hasDl)
            {
                cache.missionDl = null;
                cache.dailyRawDays = -1;
            }
            else
            {
                //Debug.Log("hasDl");
                if (rd > cache.dailyRawDays)
                {
                    //Debug.Log("new dl");
                    cache.dailyRawDays = rd;
                    cache.dlDone = 0;
                    cache.missionDl = GetDailyMissionItem(0);
                }
                else if (rd == cache.dailyRawDays)
                {
                    //Debug.Log("today dl");
                    //Debug.Log(cfg.daily.Count);
                    //Debug.Log(cache.dlDone);
                    if (cache.missionDl == null && cache.dlDone < cfg.daily.Count)
                    {
                        cache.missionDl = GetDailyMissionItem(cache.dlDone);
                    }
                }
                else
                {
                    //Debug.Log("future dl");
                    cache.missionDl = null;
                }
            }

            UxService.instance.SaveGameData();
        }

        MissionItem GetDailyMissionItem(int index)
        {
            return new MissionItem(GetPrototype_Daily(index));
        }

        void CheckoutMainline()
        {
            var cache = UxService.instance.gameDataCache.cache;
            var crtMl = cache.missionMl;
            var li = LevelService.instance.GetNextCampaignLevelIndex();
            var tutoCfg = ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData;

            bool hasMl = li >= tutoCfg.mission_ml;
            if (!hasMl)
            {
                cache.missionMl = null;
            }
            else
            {
                //Debug.Log(cache.missionMl);
                if (cache.missionMl == null)
                {
                    var mlMissions = cfg.mainline;
                    foreach (var mlMission in mlMissions)
                    {
                        //Debug.Log("mlMission " + mlMission.id);
                        if (cache.missionsDoneMl.Contains(mlMission.id))
                            continue;
                        cache.missionMl = new MissionItem(mlMission);
                        break;
                    }
                }
            }

            UxService.instance.SaveGameData();
        }

        public void SyncMissions()
        {
            CheckoutDaily();
            CheckoutMainline();
            SyncUi();
        }

        public void SyncUi()
        {
            //Debug.Log(GameFlowService.instance.windowState);
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Gameplay ||
                GameFlowService.instance.windowState == GameFlowService.WindowState.None)
            {
                MissionPanelBehaviour.instance.HideMl();
                MissionPanelBehaviour.instance.HideDl();
                return;
            }

            var mlItem = GetCrtMainlineItem();
            var dlItem = GetCrtDailyItem();
            var ml = GetCrtMainlineProto();
            var dl = GetCrtDailyProto();
            if (ml == null)
            {
                MissionPanelBehaviour.instance.HideMl();
            }
            else
            {
                MissionPanelBehaviour.instance.SetMl(ml.GetMissionText(), mlItem.saveData.crt, mlItem.saveData.total, mlItem.finished);
            }

            if (dl == null)
            {
                MissionPanelBehaviour.instance.HideDl();
            }
            else
            {
                MissionPanelBehaviour.instance.SetDl(dl.GetMissionText(), dlItem.saveData.crt, dlItem.saveData.total, dlItem.finished);
            }
        }

        public MissionPrototype GetPrototype_Mainline(string id)
        {
            var ms = cfg.mainline;
            foreach (var m in ms)
            {
                if (m.id == id)
                    return m;
            }

            return null;
        }

        public MissionPrototype GetPrototype_Daily(int index)
        {
            var list = cfg.daily[index];
            if (list.ms.Count > 0)
            {
                var rd = UxService.instance.GetRawPlayedDays();
                return list.ms[rd % list.ms.Count];
            }

            return null;
        }

        public MissionPrototype GetCrtMainlineProto()
        {
            var item = GetCrtMainlineItem();
            if (item != null)
                return GetPrototype_Mainline(item.id);

            return null;
        }

        public MissionPrototype GetCrtDailyProto()
        {
            var item = GetCrtDailyItem();
            if (item != null)
            {
                foreach (var dlList in cfg.daily)
                {
                    foreach (var dl in dlList.ms)
                    {
                        if (item.id == dl.id)
                        {
                            return dl;
                        }
                    }
                }
            }

            return null;
        }

        public MissionItem GetCrtMainlineItem()
        {
            return UxService.instance.gameDataCache.cache.missionMl;
        }

        public MissionItem GetCrtDailyItem()
        {
            return UxService.instance.gameDataCache.cache.missionDl;
        }

        public void TryCommitMainline()
        {
            var item = GetCrtMainlineItem();
            var proto = GetCrtMainlineProto();
            if (item.finished)
            {
                GiveReward(proto);
                ValidateMainline();
            }
            else
            {
                PreviewReward(proto, "MsMlPreviewTitle");
            }
        }

        public void TryCommitDaily()
        {
            var item = GetCrtDailyItem();
            var proto = GetCrtDailyProto();

            if (item.finished)
            {
                GiveReward(proto);
                ValidateDaily();
                return;
            }

            if (proto.content.type == MissionPrototype.MissionType.Ad)
            {
                PlayMissionAd();
                return;
            }

            PreviewReward(proto, "MsDlPreviewTitle");
        }

        void PlayMissionAd()
        {
            AdService.instance.PlayAd(
              () =>
              {
                  Debug.Log("ad fail, mission");
                  AdService.instance.CommonFeedback_Fail();
              },
              () =>
              {
                  Debug.Log("ad suc, mission");
                  MissionAdPlayDone();
                  AdService.instance.CommonFeedback_Suc();
              },
              () =>
              {
                  Debug.Log("ad cease, mission");
                  AdService.instance.CommonFeedback_Cease();
              },
              () =>
              {
                  Debug.Log("ad canplay, mission");
              },
              () =>
              {
                  Debug.Log("ad can not play, mission");
                  AdService.instance.CommonFeedback_CanNotPlay();
              }
              );
        }

        void MissionAdPlayDone()
        {
            PushDl("ad", 1, true);
            TryCommitDaily();
        }

        void PreviewReward(MissionPrototype proto, string titleKey)
        {
            var reward = proto.reward;
            SoundService.instance.Play("btn info");
            var data = new ItemsPopup.ItemsPopupData();
            data.clickBgClose = true;
            data.hasBtnOk = false;
            data.title = LocalizationService.instance.GetLocalizedText(titleKey);
            data.content = LocalizationService.instance.GetLocalizedText("MsPreviewContent");
            data.items = reward;

            if (proto.id == "town")
            {
                data.content = LocalizationService.instance.GetLocalizedText("Ms_CheckTownDesc") + "\n" + data.content;
                data.hasBtnOk = true;
                data.btnOkString = LocalizationService.instance.GetLocalizedText("Town");
                data.OkCb = () => { WindowService.instance.ShowTown(); };
            }
            WindowService.instance.ShowItemsPopup(data);
        }

        void GiveReward(MissionPrototype proto)
        {
            var reward = proto.reward;
            foreach (var r in reward)
            {
                UxService.instance.AddItem(r);
            }

            SoundService.instance.Play("reward");
            var data = new ItemsPopup.ItemsPopupData();
            data.clickBgClose = false;
            data.hasBtnOk = true;
            data.title = LocalizationService.instance.GetLocalizedText("MsDoneTitle");
            data.content = LocalizationService.instance.GetLocalizedText("MsDoneContent");
            data.items = reward;
            WindowService.instance.ShowItemsPopup(data);
            SetActiveParticleSystemsBehaviour.instance.ShowCloverEff();
            UxService.instance.SaveGameItemData();
        }

        void ValidateMainline()
        {
            var cache = UxService.instance.gameDataCache.cache;
            cache.missionsDoneMl.Add(cache.missionMl.id);
            cache.missionMl = null;
            SyncMissions();

            LevelService.instance.CheckLevelPassedMission();
            ShipService.instance.CheckShipUpgradeMissions();
        }

        void ValidateDaily()
        {
            var cache = UxService.instance.gameDataCache.cache;
            cache.missionDl = null;
            cache.dlDone += 1;
            SyncMissions();
        }

        public void PushMl(string id, int quota, bool addOrSet)
        {
            //Debug.Log("PushMl " + id);
            var ms = GetCrtMainlineItem();
            if (ms != null && ms.id == id)
            {
                if (addOrSet)
                {
                    ms.saveData.crt += quota;
                }
                else
                {
                    ms.saveData.crt = quota;
                }
                //Debug.Log("PushMl ok " + id);
                if (GameFlowService.instance.windowState != GameFlowService.WindowState.Gameplay)
                    SyncMissions();
            }
        }

        public void PushDl(string id, int quota, bool addOrSet)
        {
            //Debug.Log("PushDl " + id);
            var ms = GetCrtDailyItem();
            if (ms != null && ms.id == id)
            {
                if (addOrSet)
                {
                    ms.saveData.crt += quota;
                }
                else
                {
                    ms.saveData.crt = quota;
                }
                //Debug.Log("PushDl ok " + id);
                if (GameFlowService.instance.windowState != GameFlowService.WindowState.Gameplay)
                    SyncMissions();
            }
        }
    }
}