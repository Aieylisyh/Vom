using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class FishingTaskPanelBehaviour : MonoBehaviour
    {
        public Text priceTxt;
        public Text stateTxt;

        public GameObject partGo;
        public GameObject btnGo;

        public GameObject partBack;
        public GameObject btnBackAcc;
        public GameObject btnBackNoAcc;

        public void Refresh()
        {
            FishingWindowBehaviour.instance.HideBoatLevel();
            FishingBoatBehaviour.instance.StopPartViews();
            FishingBoatBehaviour.instance.UpdatePartViews();

            if (!FishingService.instance.HasRft())
            {
                SetNoRft();
                return;
            }
            //Debug.Log("OnShowTask " + FishingBoatBehaviour.instance.IsArriving());
            if (FishingBoatBehaviour.instance.IsArriving())
            {
                SetRftArriving();
                return;
            }

            SetRft();
        }

        private void SetNoRft()
        {
            partGo.SetActive(true);
            partBack.SetActive(false);
            btnGo.SetActive(true);
            stateTxt.text = LocalizationService.instance.GetLocalizedText("BoatStateOk");
            priceTxt.text = "";
        }

        private void SetRft()
        {
            partGo.SetActive(false);
            partBack.SetActive(true);
            stateTxt.text = LocalizationService.instance.GetLocalizedText("BoatStateRft");
            priceTxt.text = "";
            bool hasAcc = FishingService.instance.CanAccRft();
            //Debug.Log("hasAcc " + hasAcc);
            btnBackAcc.SetActive(hasAcc);
            btnBackNoAcc.SetActive(!hasAcc);
            if (hasAcc)
            {
                ShowAccPrice();
            }
        }

        private void SetRftArriving()
        {
            partGo.SetActive(false);
            partBack.SetActive(true);
            btnGo.SetActive(false);
            btnBackAcc.SetActive(false);
            btnBackNoAcc.SetActive(true);

            priceTxt.text = "";
            stateTxt.text = LocalizationService.instance.GetLocalizedText("BoatStateArriving");
        }

        public void OnClickInfoGo()
        {
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("RftInfoTitle");
            data.content = LocalizationService.instance.GetLocalizedText("RftInfoContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickInfoBackAcc()
        {
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("FishingAccInfoTitle");
            data.content = LocalizationService.instance.GetLocalizedText("FishingAccInfoContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickGo()
        {
            SoundService.instance.Play("btn info");
            var boatOk = FishingBoatBehaviour.instance.IsAvailable();
            var hasBoatLevelupUnlocked = FishingService.instance.HasBoatLevelupUnlocked();
            if (hasBoatLevelupUnlocked)
            {
                var data = new ConfirmBoxPopup.ConfirmBoxData();
                data.btnClose = true;
                data.btnBgClose = false;
                data.btnLeft = true;
                data.btnRight = false;
                data.title = LocalizationService.instance.GetLocalizedText("BoatRftGoTitle");
                data.content = LocalizationService.instance.GetLocalizedText("BoatRftGoContent");
                data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
                data.btnLeftAction = StartRft;
                WindowService.instance.ShowConfirmBoxPopup(data);
                return;
            }

            StartRft();
        }

        public void OnClickBackAcc()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("RftAccTitle");
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");
            var priceString = GetAccPriceString();
            var contentString = LocalizationService.instance.GetLocalizedTextFormatted("RftAccContent", priceString);
            data.content = contentString;
            data.btnLeftAction = TryAcc;
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private void StartRft()
        {
            SoundService.instance.Play("tap");
            FishingService.instance.StartRft();

            TriggerGriffthReward();
        }

        private void TriggerGriffthReward()
        {
            string correspondShipAbilityId = "Griffith_ab_2";
            var has = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId);
            if (has)
            {
                var p = ShipService.instance.GetAbilityPrototype(correspondShipAbilityId).intParam;
                var percentage = (float)p / 100f;
                var rewards = FishingService.instance.GetRftExtraRewards(percentage);
                if (rewards == null || rewards.Count < 1)
                    return;

                foreach (var reward in rewards)
                {
                    UxService.instance.AddItem(reward);
                }

                SoundService.instance.Play("reward");
                var itemsData = new ItemsPopup.ItemsPopupData();
                itemsData.clickBgClose = false;
                itemsData.hasBtnOk = true;
                itemsData.title = LocalizationService.instance.GetLocalizedText("GiffithRewardTitle");
                itemsData.content = LocalizationService.instance.GetLocalizedTextFormatted("GiffithRewardContent", p);
                itemsData.items = rewards;
                WindowService.instance.ShowItemsPopup(itemsData);
            }
        }

        private void TryAcc()
        {
            var price = FishingService.instance.GetAccPrice();
            var suc = false;
            if (price[0].id == "Diamond")
            {
                var res = ItemService.instance.IsPriceAffordable(price, true);
                suc = res.success;
            }
            else if (price[0].id == "Free")
            {
                suc = true;
            }
            else if (price[0].id == "Ad")
            {
                StartAdAcc();
                return;
            }

            if (suc)
            {
                OnAccSuc();
            }
            else
            {
                OnAccFail();
            }
        }

        private void StartAdAcc()
        {
            AdService.instance.PlayAd(
              () =>
              {
                  Debug.Log("ad fail, fishacc");
                  OnAccFail();
              },
              () =>
              {
                  Debug.Log("ad suc, fishacc");
                  FishingService.instance.sessionAdAccUsedCount += 1;
                  OnAccSuc();
                  AdService.instance.CommonFeedback_Suc();
              },
              () =>
              {
                  Debug.Log("ad cease, fishacc");
                  AdService.instance.CommonFeedback_Cease();
              },
              () =>
              {
                  Debug.Log("ad canplay, fishacc");
              },
              () =>
              {
                  Debug.Log("ad can not play, fishacc");
                  AdService.instance.CommonFeedback_CanNotPlay();
              }
              );
        }

        private void OnAccSuc()
        {
            FishingService.instance.AccRft();
            SoundService.instance.Play("btn small");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("RftAccOkTitle");
            data.content = LocalizationService.instance.GetLocalizedText("RftAccOkContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
            FishingWindowBehaviour.instance.OnAcced();
        }

        private void OnAccFail()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("RftAccNotAffordableTitle");
            data.content = LocalizationService.instance.GetLocalizedText("RftAccNotAffordableContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private string GetAccPriceString()
        {
            var price = FishingService.instance.GetAccPrice();
            var s = LocalizationService.instance.GetLocalizedText("ok");
            if (price[0].id == "Diamond")
            {
                s = TextFormat.GetItemText(price, true);
            }
            else if (price[0].id == "Free")
            {
                s = LocalizationService.instance.GetLocalizedText("Free");
            }
            else if (price[0].id == "Ad")
            {
                s = LocalizationService.instance.GetLocalizedText("Ad");
            }

            return s;
        }

        private void ShowAccPrice()
        {
            priceTxt.text = GetAccPriceString();
        }
    }
}
