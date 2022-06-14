using UnityEngine;
using System.Collections.Generic;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MapNodePlayTabBehaviour : MonoBehaviour
    {
        public List<SlotBehaviour> slots;
        public bool hideEmpty = false;

        public GameObject claimedBanner;
        public Text titleText;
        public Text chancesText;
        public Text priceText;

        public GameObject btnUnlock;
        public GameObject btnPlay;

        public void Refresh()
        {
            var proto = MapNodeInfoPanelBehaviour.instance.levelProto;
            var item = MapNodeInfoPanelBehaviour.instance.levelItem;

            List<Item> items = proto.GetFirstPassReward();
            Setup(items);

            var claimed = item.passed;
            claimedBanner.SetActive(claimed);

            if (item.passed)
            {
                var accountData = UxService.instance.accountDataCache;
                var deltaPlayed = System.DateTime.Now - accountData.cache.firstLaunchDate;
                var days = deltaPlayed.TotalDays;
                string lastPlayedString = "";
                if (days < 1)
                {
                    lastPlayedString = LocalizationService.instance.GetLocalizedTextFormatted("LastPlayedHoursAgo", deltaPlayed.Hours + "");
                }
                else
                {
                    lastPlayedString = LocalizationService.instance.GetLocalizedTextFormatted("LastPlayedDaysAgo", (int)days + "");
                }

                titleText.text = LocalizationService.instance.GetLocalizedTextFormatted("MNI_PassedDate", lastPlayedString);
            }
            else
            {
                titleText.text = LocalizationService.instance.GetLocalizedText("MNI_NotPassed");
            }

            if (!item.saveData.unlocked && proto.levelType == LevelPrototype.LevelType.Extra)
            {
                ShowUnlock();
            }
            else
            {
                ShowPlay();
            }
        }

        void ShowUnlock()
        {
            btnPlay.SetActive(false);
            btnUnlock.SetActive(true);

            var item = MapNodeInfoPanelBehaviour.instance.levelItem;
            var proto = MapNodeInfoPanelBehaviour.instance.levelProto;
            var price = new Item(proto.unlockPrice.n, proto.unlockPrice.id);
            priceText.text = TextFormat.GetItemText(price, true);
        }

        void ShowPlay()
        {
            btnPlay.SetActive(true);
            btnUnlock.SetActive(false);

            if (TutorialService.instance.IsCampaignLimitEnabled())
            {
                var item = MapNodeInfoPanelBehaviour.instance.levelItem;
                CheckPlayCount();
                var chances = ConfigService.instance.levelConfig.campaignLevelPlayLimit;
                var restChances = chances;
                //restChances -= item.saveData.lastPlayedCount;
                restChances -= UxService.instance.gameDataCache.cache.lastPlayedCount;
                chancesText.text = LocalizationService.instance.GetLocalizedTextFormatted("MNI_TodayChance", restChances + "/" + chances);
            }
            else
            {
                chancesText.text = "";
            }
        }

        public void Clear()
        {
            Setup(new List<Item>());
        }

        private void CheckHideLines(int count)
        {
            if (!hideEmpty)
                return;

            int slotCount = 0;
            foreach (var s in slots)
            {
                s.gameObject.SetActive(slotCount < count);
                slotCount++;
            }
        }

        public void Setup(List<Item> items)
        {
            CheckHideLines(items.Count);
            if (items == null)
            {
                foreach (var s in slots)
                {
                    s.SetEmpty();
                }
            }
            int i = -1;
            foreach (var s in slots)
            {
                i++;
                if (items.Count > i)
                {
                    s.Setup(items[i]);
                }
                else
                {
                    s.SetEmpty();
                }
            }
        }

        void CheckPlayCount()
        {
            var crtLevelItem = MapNodeInfoPanelBehaviour.instance.levelItem;
            UxService.instance.SyncLevelPlayedCount(crtLevelItem);
        }

        ////////////////////////////////interaction/////////////////////////////
        public void OnClickPlay()
        {
            if (TutorialService.instance.IsCampaignLimitEnabled())
            {
                CheckPlayCount();
                //var item = MapNodeInfoPanelBehaviour.instance.levelItem;
                //var lpc = item.saveData.lastPlayedCount;
                var lpc = UxService.instance.gameDataCache.cache.lastPlayedCount;
                if (lpc >= ConfigService.instance.levelConfig.campaignLevelPlayLimit)
                {
                    ShowCanNotPlayPopup();
                    return;
                }
            }

            LevelService.instance.EnterCampaignLevel(MapNodeInfoPanelBehaviour.instance.levelId);
            WindowService.instance.HideAllWindows();
        }

        public void OnClickUnlock()
        {
            var proto = MapNodeInfoPanelBehaviour.instance.levelProto;
            var price = new Item(proto.unlockPrice.n, proto.unlockPrice.id);

            var res = ItemService.instance.IsPriceAffordable(price, true);
            if (res.success)
            {
                WaitingCircleBehaviour.instance.SetHideAction(() => { DoUnlock(); });
                WaitingCircleBehaviour.instance.Show(1.2f);
            }
            else
            {
                ShowCanNotUnlockPopup();
            }
        }

        void DoUnlock()
        {
            Debug.LogWarning("DoUnlock");
            var item = MapNodeInfoPanelBehaviour.instance.levelItem;
            item.saveData.unlocked = true;
            UxService.instance.SaveGameData();

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_UnlockedTitle");
            data.content = LocalizationService.instance.GetLocalizedText("MNI_UnlockedContent");
            WindowService.instance.ShowConfirmBoxPopup(data);

            MapNodeInfoPanelBehaviour.instance.OnUnlockDone();
            MapWindowBehaviour.instance.map.RefreshMap();
            Refresh();
        }

        void ShowCanNotPlayPopup()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_NoChanceTitle");

            var price = LevelService.instance.GetDailyChanceFullFillPrice();
            var priceString = TextFormat.GetItemText(price, true);
            data.content = LocalizationService.instance.GetLocalizedTextFormatted("MNI_NoChanceContent", priceString);
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("MNI_BuyChance");
            data.btnLeftAction = TryBuyChance;
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        void ShowBuyChanceFailPopup()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_BuyChanceTitle");
            data.content = LocalizationService.instance.GetLocalizedText("MNI_BuyChanceContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        void TryBuyChance()
        {
            var price = LevelService.instance.GetDailyChanceFullFillPrice();
            var res = ItemService.instance.IsPriceAffordable(price, true);
            if (res.success)
            {
                //var item = MapNodeInfoPanelBehaviour.instance.levelItem;
                //item.saveData.lastPlayedCount = 0;
                UxService.instance.gameDataCache.cache.lastPlayedCount = 0;
                UxService.instance.gameDataCache.cache.buyPlayChanceCount += 1;
                UxService.instance.SaveGameData();

                WaitingCircleBehaviour.instance.SetHideAction(() =>
                {
                    MapNodeInfoPanelBehaviour.instance.OnUnlockDone();
                    Refresh();
                });
                WaitingCircleBehaviour.instance.Show(0.6f);
            }
            else
            {
                ShowBuyChanceFailPopup();
            }
        }

        void ShowCanNotUnlockPopup()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_UnlockNonAffordableTitle");
            data.content = LocalizationService.instance.GetLocalizedText("MNI_UnlockNonAffordableContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_PassReward");
            data.content = LocalizationService.instance.GetLocalizedText("MNI_PassReward_desc");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }
    }
}
