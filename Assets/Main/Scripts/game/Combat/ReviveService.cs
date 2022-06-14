using UnityEngine;
using System;
using com;

namespace game
{
    public class ReviveService : MonoBehaviour
    {
        public static ReviveService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public bool CanRevive()
        {
            if (GetRestReviveCount() < 1)
            {
                return false;
            }
            if (IsFirstWave())
            {
                //return false;
            }
            if (IsNoReviveLevelType())
            {
                return false;
            }
            if (IsReviveLockedLevel())
            {
                return false;
            }

            return true;
        }

        public bool IsNoReviveLevelType()
        {
            var rtl = LevelService.instance.runtimeLevel;
            if (rtl.levelProto.levelType == LevelPrototype.LevelType.Abysse)
            {
                return true;
            }
            if (rtl.levelProto.levelType == LevelPrototype.LevelType.Extra)
            {
                return false;
            }
            if (rtl.levelProto.levelType == LevelPrototype.LevelType.Boss)
            {
                return true;
            }

            return false;
        }

        public bool IsFirstWave()
        {
            var rtl = LevelService.instance.runtimeLevel;
            return rtl.currentWaveIndex == 0;
        }

        public bool IsReviveLockedLevel()
        {
            var index = LevelService.instance.GetRuntimeCampaignLevelIndex();
            if (index < 0)
            {
                return false;
            }

            return index < ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData.revive;
        }

        private bool RevivePriceAffordable(Item price)
        {
            return false;
        }

        private Item GetRevivePrice()
        {
            var cfg = ConfigService.instance.combatConfig.playerParam;
            if (GetRestReviveCount() > 1)
            {
                return new Item(cfg.reviveDiamond1, "Diamond");
            }

            if (AdService.instance.CanPlayAd(false))
            {
                return new Item(1, "Ad");
            }

            return new Item(cfg.reviveDiamond2, "Diamond");
        }

        public void DemandRevive(Action rejectCb)
        {
            if (!CanRevive())
            {
                rejectCb?.Invoke();
                return;
            }

            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnLeftAction = () => { OnReviveConfirmed(rejectCb); };
            data.btnRightAction = () => { rejectCb?.Invoke(); };
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = true;
            data.title = LocalizationService.instance.GetLocalizedText("ReviveTitle");
            var price = GetRevivePrice();
            var priceString = TextFormat.GetItemRichTextWithSubName(price);
            var proto = ItemService.instance.GetPrototype(price.id);
            data.content = LocalizationService.instance.GetLocalizedTextFormatted("ReviveContent", priceString);

            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Revive");
            data.btnRightTxt = LocalizationService.instance.GetLocalizedText("Retreat");

            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private void OnReviveConfirmed(Action rejectCb)
        {
            var price = GetRevivePrice();
            if (price.id == "Ad")
            {
                AdService.instance.PlayAd(
                () =>
                {
                    Debug.Log("ad fail, revive");
                    //AdService.instance.CommonFeedback_Fail();
                    rejectCb?.Invoke();
                },
                () =>
                {
                    Debug.Log("ad suc, revive");
                    //ItemService.instance.IsPriceAffordable(price, true);
                    LevelService.instance.Revive();
                    AdService.instance.CommonFeedback_Suc();
                },
                () =>
                {
                    Debug.Log("ad cease, revive");
                    AdService.instance.CommonFeedback_Cease();
                },
                () =>
                {
                    Debug.Log("ad canplay, revive");
                },
                () =>
                {
                    Debug.Log("ad can not play, revive");
                    AdService.instance.CommonFeedback_CanNotPlay();
                }
                );
                return;
            }

            var res = ItemService.instance.IsPriceAffordable(price, true);
            if (res.success)
            {
                SoundService.instance.Play(new string[3] { "reward", "pay1", "pay2" });
                LevelService.instance.Revive();
            }
            else
            {
                ItemService.instance.ShowTransactionFailConfirmBox(res);
                rejectCb?.Invoke();
            }
        }

        private int GetRestReviveCount()
        {
            var reviveCount = LevelService.instance.runtimeLevel.reviveCount;
            var maxCount = ConfigService.instance.combatConfig.reviveMaxCount;
            return maxCount - reviveCount;
        }
    }
}
