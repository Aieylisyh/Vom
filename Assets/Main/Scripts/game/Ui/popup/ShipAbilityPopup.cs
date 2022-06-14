using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class ShipAbilityPopup : PopupBehaviour
    {
        public Text titleTxt;

        public Text descTxt;
        public Text levelReqTxt;
        public Text priceTxt;
        public GameObject btn;
        public Image icon;

        private ShipPrototype.ShipAbilityUnlockPrototype _proto;
        private ShipAbilitySlotBehaviour _slot;

        public void Setup(ShipPrototype.ShipAbilityUnlockPrototype proto, ShipAbilitySlotBehaviour slot)
        {
            _proto = proto;
            _slot = slot;
            icon.sprite = proto.ability.sp;
            titleTxt.text = LocalizationService.instance.GetLocalizedText(proto.ability.title);
            if (proto.ability.hasIntParam)
            {
                if (proto.ability.hasIntOtherParam)
                {
                    descTxt.text = LocalizationService.instance.GetLocalizedTextFormatted(proto.ability.desc, proto.ability.intParam, proto.ability.intOtherParam);
                }
                else
                {
                    descTxt.text = LocalizationService.instance.GetLocalizedTextFormatted(proto.ability.desc, proto.ability.intParam);
                }
            }
            else
            {
                descTxt.text = LocalizationService.instance.GetLocalizedText(proto.ability.desc);
            }

            var IsAbilityCardUnlockPossible = ShipService.instance.IsShipAbilityLearnPossible(proto);
            var IsAbilityCardUnlockAffordable = ShipService.instance.IsShipAbilityLearnAffordable(proto);


            var shipItem = ShipService.instance.GetShipItem();

            if (shipItem.HasUnlockedAbility(proto.ability.id))
            {
                btn.SetActive(false);
                levelReqTxt.text = "";
                priceTxt.text = LocalizationService.instance.GetLocalizedText("AlreadyLearned");
                priceTxt.color = Color.green;
            }
            else
            {
                btn.SetActive(true);
                var shipNameLocalized = LocalizationService.instance.GetLocalizedText(ShipService.instance.GetPrototype().title);
                levelReqTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("ShipLevelReq", shipNameLocalized, proto.shipLevelRequire);
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
            var IsAbilityCardUnlockPossible = ShipService.instance.IsShipAbilityLearnPossible(_proto);
            var IsAbilityCardUnlockAffordable = ShipService.instance.IsShipAbilityLearnAffordable(_proto);

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");

            if (!IsAbilityCardUnlockPossible)
            {
                data.title = LocalizationService.instance.GetLocalizedText("LearnFailTitle");
                data.content = LocalizationService.instance.GetLocalizedText("LearnFailContentLevel");
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
                        ShipService.instance.UnlockShipAbility(_proto.ability.id);
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
            ShipWindowBehaviour.instance.UpdateShipView();

            SharkIsland.instance.TryShowShark();
            PortalIsland.instance.TryShowPortal();
            BirdIsland.instance.TryShowBird();
            StatuesBehaviour.instance.TryShowStatues();

            ShipWindowBehaviour.instance.abilityPanelBehaviour.PlayEffectLearnSlot(_slot.GetComponent<RectTransform>());
            ShipWindowBehaviour.instance.PlayEffectSab();
        }
    }
}