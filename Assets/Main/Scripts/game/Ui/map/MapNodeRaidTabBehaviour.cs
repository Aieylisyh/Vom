using UnityEngine;
using System.Collections.Generic;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MapNodeRaidTabBehaviour : MonoBehaviour
    {
        public List<SlotBehaviour> slots;
        public bool hideEmpty = false;

        public Text chancesText;
        public Text bonusText;

        public void Refresh()
        {
            var proto = MapNodeInfoPanelBehaviour.instance.levelProto;
            var item = MapNodeInfoPanelBehaviour.instance.levelItem;

            var rawReward = proto.GetRaidRawReward();
            Setup(rawReward);

            UxService.instance.CheckRaidCount();
            var raidPrices = ConfigService.instance.levelConfig.raidDiamondPrices;
            var chances = raidPrices.Count;
            var lrc = UxService.instance.gameItemDataCache.cache.lastRaidedCount;

            var restChances = chances - lrc;
            chancesText.text = LocalizationService.instance.GetLocalizedTextFormatted("MNI_TodayRaidChance", restChances + "/" + chances);

            int raidRewardPercent = GetRaidRewardPercent();
            bonusText.text = LocalizationService.instance.GetLocalizedTextFormatted("MNI_RaidRewardBonus", raidRewardPercent + "%");
        }

        int GetRaidRewardPercent()
        {
            var item = MapNodeInfoPanelBehaviour.instance.levelItem;
            int bonusPercent = 0;
            var starCount = item.saveData.highStar;
            if (starCount == 1)
            {
                bonusPercent = ConfigService.instance.levelConfig.raidRewardBonusPercent1Star;
            }
            else if (starCount == 2)
            {
                bonusPercent = ConfigService.instance.levelConfig.raidRewardBonusPercent2Star;
            }
            else if (starCount == 3)
            {
                bonusPercent = ConfigService.instance.levelConfig.raidRewardBonusPercent3Star;
            }
            return bonusPercent + 100;
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

        ////////////////////////////////interaction/////////////////////////////
        public void OnClickRaid()
        {
            UxService.instance.CheckRaidCount();
            var raidPrices = ConfigService.instance.levelConfig.raidDiamondPrices;
            var chances = raidPrices.Count;
            var lrc = UxService.instance.gameItemDataCache.cache.lastRaidedCount;
            var restChances = chances - lrc;
            //Debug.LogWarning("chances " + chances);
            //Debug.LogWarning("lrc " + lrc);

            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;

            if (restChances <= 0)
            {
                data.title = LocalizationService.instance.GetLocalizedText("MNI_RaidNoChanceTitle");
                data.content = LocalizationService.instance.GetLocalizedText("MNI_RaidNoChanceContent");
                data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            }
            else
            {
                data.title = LocalizationService.instance.GetLocalizedText("MNI_RaidConfirmTitle");
                data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
                data.btnLeftAction = TryDoRaid;

                var price = raidPrices[lrc];
                int raidRewardPercent = GetRaidRewardPercent();
                if (price <= 0)
                {
                    data.content = LocalizationService.instance.GetLocalizedTextFormatted("MNI_RaidFreeContent", raidRewardPercent + "%");
                }
                else
                {
                    data.content = LocalizationService.instance.GetLocalizedTextFormatted("MNI_RaidPriceContent", raidRewardPercent + "%", TextFormat.GetItemText(new Item(price, "Diamond"), true));
                }
            }
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        void TryDoRaid()
        {
            //Debug.LogWarning("TryDoRaid");
            var raidPrices = ConfigService.instance.levelConfig.raidDiamondPrices;
            var lrc = UxService.instance.gameItemDataCache.cache.lastRaidedCount;
            var price = raidPrices[lrc];
            var res = ItemService.instance.IsPriceAffordable(new Item(price, "Diamond"), true);
            if (res.success)
            {
                WaitingCircleBehaviour.instance.SetHideAction(() => { DoRaid(); });
                WaitingCircleBehaviour.instance.Show(0.8f);
            }
            else
            {
                SoundService.instance.Play("btn info");
                var data = new ConfirmBoxPopup.ConfirmBoxData();
                data.btnClose = false;
                data.btnBgClose = true;
                data.btnLeft = false;
                data.btnRight = false;
                data.title = LocalizationService.instance.GetLocalizedText("MNI_RaidNonAffordableTitle");
                data.content = LocalizationService.instance.GetLocalizedText("MNI_RaidNonAffordableContent");
                WindowService.instance.ShowConfirmBoxPopup(data);
            }
        }

        void DoRaid()
        {
            //Debug.LogWarning("DoRaid");
            GiveRaidReward();
            UxService.instance.gameItemDataCache.cache.lastRaidedCount += 1;
            UxService.instance.SaveGameItemData();
            MapNodeInfoPanelBehaviour.instance.OnRaidDone();
            Refresh();
        }



        void GiveRaidReward()
        {
            var proto = MapNodeInfoPanelBehaviour.instance.levelProto;
            int raidRewardPercent = GetRaidRewardPercent();

            List<Item> items = new List<Item>();
            var rawReward = proto.GetRaidRawReward();
            foreach (var r in rawReward)
                items.Add(new Item(MathGame.GetPercentage(r.n, raidRewardPercent), r.id));

            var percent = UxService.instance.GetItemAmount("FlagGold");
            foreach (var item in items)
            {
                if (item.id == "Gold")
                {
                    item.n = MathGame.GetPercentageAdded(item.n, percent);
                    break;
                }
            }

            ItemService.instance.GiveReward(items, false, "MNI_RaidSucTitle");
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MNI_RaidReward");
            data.content = LocalizationService.instance.GetLocalizedText("MNI_RaidReward_desc");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }
    }
}
