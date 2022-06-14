using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class FishingAbilityPopup : PopupBehaviour
    {
        public Text titleTxt;

        public Text descTxt;
        public Text levelReqTxt;
        public Text priceTxt;
        public GameObject btn;
        public Image icon;
        private FishingAbilityUnlockPrototype _proto;
        private FishingAbilitySlotBehaviour _slot;

        public void Setup(FishingAbilityUnlockPrototype proto, FishingAbilitySlotBehaviour slot)
        {
            _proto = proto;
            _slot = slot;
            icon.sprite = proto.ability.sp;
            titleTxt.text = LocalizationService.instance.GetLocalizedText(proto.ability.title);
            if (proto.ability.hasIntParam)
            {
                descTxt.text = LocalizationService.instance.GetLocalizedTextFormatted(proto.ability.desc, proto.ability.intParam);
            }
            else
            {
                descTxt.text = LocalizationService.instance.GetLocalizedText(proto.ability.desc);
            }

            string[] priceIds = new string[4];
            priceIds[0] = "";
            priceIds[1] = "";
            priceIds[2] = "";
            priceIds[3] = "";
            var i = 0;
            foreach (var p in proto.price)
            {
                priceIds[i] = p.id;
                i++;
            }

            MainHudBehaviour.instance.SetMode(false, priceIds[0], priceIds[1], priceIds[2], priceIds[3]);
            MainHudBehaviour.instance.Show();

            var IsAbilityCardUnlockPossible = FishingService.instance.IsAbilityLearnPossible(proto);
            var IsAbilityCardUnlockAffordable = FishingService.instance.IsAbilityLearnAffordable(proto);

            var item = FishingService.instance.GetItem();

            if (FishingService.instance.HasUnlockedAbility(proto.ability.id))
            {
                btn.SetActive(false);
                levelReqTxt.text = "";
                priceTxt.text = LocalizationService.instance.GetLocalizedText("AlreadyLearned");
                priceTxt.color = Color.green;
            }
            else
            {
                btn.SetActive(true);
                levelReqTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("BoatLevelReq", proto.boatLevelRequire);
                if (IsAbilityCardUnlockPossible)
                {
                    priceTxt.text = LocalizationService.instance.GetLocalizedText("Need") + ": " + TextFormat.GetItemText(proto.price, true);
                }
                else
                {
                    priceTxt.text = "";
                }

                levelReqTxt.color = IsAbilityCardUnlockPossible ? Color.grey : Color.red;
                priceTxt.color = IsAbilityCardUnlockAffordable ? Color.grey : Color.red;
            }
        }

        public void OnClickLearnBtn()
        {
            var IsAbilityCardUnlockPossible = FishingService.instance.IsAbilityLearnPossible(_proto);
            var IsAbilityCardUnlockAffordable = FishingService.instance.IsAbilityLearnAffordable(_proto);

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");
            if (!IsAbilityCardUnlockPossible)
            {
                data.title = LocalizationService.instance.GetLocalizedText("LearnFailTitle");
                data.content = LocalizationService.instance.GetLocalizedText("LearnFabFailContentLevel");
                WindowService.instance.ShowConfirmBoxPopup(data);
                Sound();
            }
            else if (!IsAbilityCardUnlockAffordable)
            {
                data.title = LocalizationService.instance.GetLocalizedText("LearnFailTitle");
                data.content = LocalizationService.instance.GetLocalizedText("LearnFailContentPrice");
                WindowService.instance.ShowConfirmBoxPopup(data);
                Sound();
            }
            else
            {

                var res = ItemService.instance.IsPriceAffordable(_proto.price, true);
                if (res.success)
                {
                    WaitingCircleBehaviour.instance.SetHideAction(() =>
                    {
                        FloatingTextPanelBehaviour.instance.Create(LocalizationService.instance.GetLocalizedText("LearnSucContent"), 0.5f, 0.67f, true);
                        FishingService.instance.UnlockFishingAbility(_proto.ability.id);
                        OnLearn();
                        SoundService.instance.Play("ship sp");
                    });
                    WaitingCircleBehaviour.instance.Show(0.6f);
                }
                else
                {
                    Debug.LogWarning("unexpected!");
                    data.title = LocalizationService.instance.GetLocalizedText("LearnFailTitle");
                    data.content = LocalizationService.instance.GetLocalizedText("LearnFailContentPrice");
                    WindowService.instance.ShowConfirmBoxPopup(data);
                    Sound();
                }
            }

            Hide();
        }

        private void OnLearn()
        {
            MainHudBehaviour.instance.Refresh();
            FishingWindowBehaviour.instance.OnAbilityLearned();
        }
    }
}
