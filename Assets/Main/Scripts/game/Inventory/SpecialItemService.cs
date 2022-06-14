using UnityEngine;
using System.Collections.Generic;
using com;
using System;

namespace game
{
    public class SpecialItemService : MonoBehaviour
    {
        public static SpecialItemService instance;

        public int cachedNoteIndex { get; private set; }

        //note clover paysheet compass
        private void Awake()
        {
            instance = this;
        }

        public void ShowCompass()
        {
            var ls = LocalizationService.instance;
            int restBoss = 0;
            string bossPhrase = "";
            if (restBoss > 0)
                bossPhrase = ls.GetLocalizedTextFormatted("Compass_Boss", restBoss) + "\n";

            int restDungeonChance = 0;
            string dungeonPhrase = "";
            if (restDungeonChance > 0)
                dungeonPhrase = ls.GetLocalizedTextFormatted("Compass_Dungeon", restDungeonChance) + "\n";

            bool hasNextCampaignLevel = LevelService.instance.HasNextCampaignLevel();
            string levelPhrase = "";
            if (hasNextCampaignLevel)
            {
                var levelId = LevelService.instance.GetCampainLevelId();
                LevelPrototype crtLevel = LevelService.instance.GetPrototype(levelId);
                var levelname = LocalizationService.instance.GetLocalizedText(crtLevel.title);
                levelPhrase = ls.GetLocalizedTextFormatted("Compass_Level", levelname) + "\n";
            }

            string fishingPhrase = "";
            var hasFinishedRft = FishingService.instance.HasFinishedRft();
            var hasRft = FishingService.instance.HasRft();
            if (hasFinishedRft || !hasRft)
            {
                fishingPhrase = ls.GetLocalizedText("Compass_Fishing") + "\n";
            }
            else
            {
                var ts = FishingService.instance.GetRftRestTimeSpan();
                if (ts.Hours <= 1)
                    fishingPhrase = ls.GetLocalizedText("Compass_FishingSoon") + "\n";
            }

            var unspentTp = TalentService.instance.GetTalentPoint();
            string talentPhrase = "";
            if (unspentTp > 0)
                talentPhrase = ls.GetLocalizedTextFormatted("Compass_Talent", unspentTp) + "\n";

            var cloverDone = HaveUsedClover();
            string cloverPhrase = "";
            if (!cloverDone)
                cloverPhrase = ls.GetLocalizedText("Compass_Clover") + "\n";

            var salaryDone = HaveGotSalary();
            string salaryPhrase = "";
            if (!salaryDone)
                salaryPhrase = ls.GetLocalizedText("Compass_Salary") + "\n";

            string raidPhrase = "";
            UxService.instance.CheckRaidCount();
            var raidPrices = ConfigService.instance.levelConfig.raidDiamondPrices;
            var lrc = UxService.instance.gameItemDataCache.cache.lastRaidedCount;
            if (lrc >= raidPrices.Count)
                lrc = raidPrices.Count - 1;

            var price = raidPrices[lrc];
            if (price <= 0)
                raidPhrase = ls.GetLocalizedText("Compass_Raid") + "\n";

            string challengePhrase = "";
            if (TutorialService.instance.IsCampaignLimitEnabled())
            {
                UxService.instance.SyncLevelPlayedCount(null);
                var restChances = ConfigService.instance.levelConfig.campaignLevelPlayLimit;
                restChances -= UxService.instance.gameDataCache.cache.lastPlayedCount;
                if (restChances > 0)
                    challengePhrase = ls.GetLocalizedText("Compass_Challenge") + "\n";
            }

            //  var unreadCount = MainButtonsBehaviour.instance.GetUnreadMailCount();
            //  string mailPhrase = "";
            //  if (unreadCount > 0)
            //      mailPhrase = ls.GetLocalizedTextFormatted("Compass_Mail", unreadCount) + "\n";

            SoundService.instance.Play("btn info");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = ls.GetLocalizedText("Compass_Title");
            confirmBoxData.content = cloverPhrase + salaryPhrase + fishingPhrase + talentPhrase + challengePhrase + raidPhrase + levelPhrase;

            confirmBoxData.btnLeftTxt = ls.GetLocalizedText("ok");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        public void ShowNote()
        {
            SetNoteIndex(0);
            ShowNoteConfirmBox();
        }

        void ShowNextNote()
        {
            var newIndex = GetNoteIndexWithOffset(1);
            SetNoteIndex(newIndex);
            ShowNoteConfirmBox();
        }

        void ShowNoteConfirmBox()
        {
            var noteIndex = cachedNoteIndex;
            string noteTitle = GetLocalizedNoteTitle(noteIndex);
            string noteContent = GetLocalizedNoteContent(noteIndex, true);

            SoundService.instance.Play("scroll open");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = noteTitle;
            confirmBoxData.content = noteContent;
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("NextNote");
            confirmBoxData.btnLeftAction = ShowNextNote;
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        void SetNoteIndex(int i)
        {
            cachedNoteIndex = i;
        }

        int GetNoteIndexWithOffset(int offset)
        {
            var i = cachedNoteIndex + offset;
            var noteCount = GetNoteCount();
            if (i >= noteCount)
                i -= noteCount;
            if (i < 0)
                i += noteCount;

            return i;
        }

        string GetLocalizedNoteContent(int index, bool hideNonRevealed)
        {
            var id = GetNoteId(index);
            if (hideNonRevealed && !CanNoteReveal(index))
                return LocalizationService.instance.GetLocalizedTextFormatted("NoteFail", index + 1);

            return LocalizationService.instance.GetLocalizedText(id);
        }

        string GetLocalizedNoteTitle(int index)
        {
            var s = (index + 1) + "/" + GetNoteCount();
            return LocalizationService.instance.GetLocalizedTextFormatted("NoteTitle", s);
        }

        int GetNoteCount()
        {
            return ConfigService.instance.itemConfig.speicalItemConfig.noteIdMax;
        }

        bool CanNoteReveal(int index)
        {
            return UxService.instance.gameDataCache.cache.playerLevel > index;
        }

        string GetNoteId(int index)
        {
            return ConfigService.instance.itemConfig.speicalItemConfig.noteIdPrefix + (index + 1);
        }

        public void UseSalary()
        {
            if (HaveGotSalary())
            {
                SoundService.instance.Play("scroll close");
                var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
                confirmBoxData.btnClose = false;
                confirmBoxData.btnBgClose = true;
                confirmBoxData.btnLeft = true;
                confirmBoxData.btnRight = false;
                confirmBoxData.title = LocalizationService.instance.GetLocalizedText("SalaryFailTitle");
                confirmBoxData.content = LocalizationService.instance.GetLocalizedText("SalaryDoneContent");
                confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
                WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
                return;
            }

            if (CanGetSalary())
            {
                var salaryReward = GetSalaryRewards();
                foreach (var reward in salaryReward)
                    UxService.instance.AddItem(reward);

                SoundService.instance.Play(new string[2] { "pay1", "pay2" });
                var data = new ItemsPopup.ItemsPopupData();
                data.clickBgClose = false;
                data.hasBtnOk = true;
                data.title = LocalizationService.instance.GetLocalizedText("SalarySucTitle");
                data.content = LocalizationService.instance.GetLocalizedText("SalarySucContent");
                data.items = salaryReward;
                WindowService.instance.ShowItemsPopup(data);

                UxService.instance.gameItemDataCache.cache.rd_pay = UxService.instance.GetRawPlayedDays();
                UxService.instance.SaveGameItemData();
                WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
                SetActiveParticleSystemsBehaviour.instance.ShowSalaryEff();
            }
            else
            {
                SoundService.instance.Play("scroll close");
                var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
                confirmBoxData.btnClose = false;
                confirmBoxData.btnBgClose = true;
                confirmBoxData.btnLeft = true;
                confirmBoxData.btnRight = false;
                confirmBoxData.title = LocalizationService.instance.GetLocalizedText("SalaryFailTitle");
                confirmBoxData.content = LocalizationService.instance.GetLocalizedText("SalaryFailContent");
                confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
                WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
            }
        }

        bool CanGetSalary()
        {
            return MissionService.instance.HasFinishedDl();
            // var timespan = UxService.instance.GetLastLaunchDate();
            // var delta = System.DateTime.Now - timespan;
            // var playedMinutes = delta.Minutes;
            // if (playedMinutes >= ConfigService.instance.itemConfig.speicalItemConfig.salaryMinMinuteReq)
            //     return true;
            //
            // return false;
        }

        bool HaveGotSalary()
        {
            var rd = UxService.instance.gameItemDataCache.cache.rd_pay;
            var rpd = UxService.instance.GetRawPlayedDays();
            Debug.Log("HaveGotSalary " + rd + " " + rpd);
            return rpd <= rd;
        }

        List<Item> GetSalaryRewards()
        {
            List<Item> res = new List<Item>();
            var formula = ConfigService.instance.itemConfig.speicalItemConfig.salaryGoldByPlayerLevel;
            var plv = UxService.instance.gameDataCache.cache.playerLevel;
            int n = formula.GetIntValue(plv);
            res.Add(new Item(n, "Gold"));
            return res;
        }

        public void UseClover()
        {
            if (HaveUsedClover())
            {
                if (CanUseAdClover())
                {
                    ShowPlayCloverAd();
                    return;
                }

                ShowCloverUsed();
                return;
            }

            StartClover();
        }

        void ShowPlayCloverAd()
        {
            SoundService.instance.Play("clover");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = true;
            confirmBoxData.btnBgClose = false;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("CloverDoingTitleAd");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("CloverDoingContentAd");
            confirmBoxData.btnLeftTxt = TextFormat.GetRichTextTag("Ad") + " " + LocalizationService.instance.GetLocalizedText("CloverReveal");
            confirmBoxData.btnLeftAction = () =>
            {
                WaitingCircleBehaviour.instance.SetHideAction(() => { PlayCloverAd(); });
                WaitingCircleBehaviour.instance.Show(0.5f);
            };
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        void PlayCloverAd()
        {
            AdService.instance.PlayAd(
              () =>
              {
                  Debug.Log("ad fail, clover");
                  //AdService.instance.CommonFeedback_Fail();
                  ShowCloverFailed();
              },
              () =>
              {
                  Debug.Log("ad suc, clover");
                  ShowCloverResult(true);
                  AdService.instance.CommonFeedback_Suc();
              },
              () =>
              {
                  Debug.Log("ad cease, clover");
                  ShowCloverFailed();
                  AdService.instance.CommonFeedback_Cease();
              },
              () =>
              {
                  Debug.Log("ad canplay, clover");
              },
              () =>
              {
                  Debug.Log("ad can not play, clover");
                  ShowCloverFailed();
                  AdService.instance.CommonFeedback_CanNotPlay();
              }
              );
        }

        void ShowCloverFailed()
        {
            SoundService.instance.Play("scroll close");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("CloverFailedTitle");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("CloverFailedContent");
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        void ShowCloverUsed()
        {
            SoundService.instance.Play("scroll close");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("CloverUsedTitle");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("CloverContentTitle");
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        void StartClover()
        {
            SoundService.instance.Play("clover");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = false;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("CloverDoingTitle");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("CloverDoingContent");
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("CloverReveal");
            confirmBoxData.btnLeftAction = () =>
            {
                WaitingCircleBehaviour.instance.SetHideAction(() => { ShowCloverResult(false); });
                WaitingCircleBehaviour.instance.Show(0.6f);
            };
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        bool HaveUsedClover()
        {
            var rd = UxService.instance.gameItemDataCache.cache.rd_clover;
            var rpd = UxService.instance.GetRawPlayedDays();
            //Debug.Log("HaveUsedClover " + rd + " " + rpd);
            return rpd <= rd;
        }

        bool CanUseAdClover()
        {
            if (HaveUsedClover_ad())
            {
                return false;
            }
            if (!AdService.instance.CanPlayAd(false))
            {
                return false;
            }
            return true;
        }

        bool HaveUsedClover_ad()
        {
            var rd = UxService.instance.gameItemDataCache.cache.rd_clover_ad;
            var rpd = UxService.instance.GetRawPlayedDays();
            return rpd <= rd;
        }

        void ShowCloverResult(bool isAd)
        {
            var luckyData = PickLuckyData();

            var rewards = luckyData.rewards;
            foreach (var reward in rewards)
                UxService.instance.AddItem(reward);

            var data = new ItemsPopup.ItemsPopupData();
            data.clickBgClose = true;
            data.hasBtnOk = true;
            data.title = LocalizationService.instance.GetLocalizedText(luckyData.dayTitle);
            data.content = LocalizationService.instance.GetLocalizedText(luckyData.dayContent);
            data.items = rewards;

            if (isAd)
            {
                UxService.instance.gameItemDataCache.cache.rd_clover_ad = UxService.instance.GetRawPlayedDays();
            }
            else
            {
                UxService.instance.gameItemDataCache.cache.rd_clover = UxService.instance.GetRawPlayedDays();
                if (CanUseAdClover())
                {
                    data.OkCb = () =>
                    {
                        WaitingCircleBehaviour.instance.SetHideAction(() => { PlayCloverAd(); });
                        WaitingCircleBehaviour.instance.Show(0.5f);
                    };
                    data.content += " " + LocalizationService.instance.GetLocalizedText("CloverDoingContentAd");
                    data.btnOkString = TextFormat.GetRichTextTag("Ad") + " " + LocalizationService.instance.GetLocalizedText("CloverReveal");
                }
            }

            WindowService.instance.ShowItemsPopup(data);
            UxService.instance.SaveGameItemData();
            WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
            SetActiveParticleSystemsBehaviour.instance.ShowCloverEff();
        }

        int GetLuckyIndex()
        {
            int res = 0;
            int level = UnityEngine.Random.Range(0, 10);
            if (level > 5)
                res++;

            if (level > 8)
                res++;
            return res;
        }

        public SpeicalItemConfig.CloverLuckyData PickLuckyData()
        {
            var dayOfWeek = UxService.GetDayIndexOfWeek();
            var cfg = ConfigService.instance.itemConfig.speicalItemConfig;
            SpeicalItemConfig.CloverDayData cloverDayData = cfg.WeekdayData[dayOfWeek];

            var lucky = GetLuckyIndex();
            switch (lucky)
            {
                case 0:
                    SoundService.instance.Play(cfg.calmSoundId);
                    return cloverDayData.calm;
                case 1:
                    SoundService.instance.Play(cfg.pleasantSoundId);
                    return cloverDayData.pleasant;
                case 2:
                    SoundService.instance.Play(cfg.wonderfulSoundId);
                    return cloverDayData.wonderful;
            }
            return cloverDayData.calm;
        }
    }
}