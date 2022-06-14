using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ShipAbilityPanelBehaviour : MonoBehaviour
    {
        public Text desc;
        public Text shipLevelTxt;
        public List<ShipAbilitySlotBehaviour> slots;
        public Slider slider;
        public ParticleSystem psLearnSlot;

        public void Refresh()
        {
            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;
            shipLevelTxt.text = shipLevel + "/" + shipProto.levelMax;
            slider.value = (float)shipLevel / (float)shipProto.levelMax;

            bool hasAbuToUnlock = false;
            bool hasAbuToUnlockAndLevelReqOk = false;
            int index = 0;

            foreach (var abu in shipProto.abilityUnlocks)
            {
                var slot = slots[index];
                slot.proto = abu;
                //var abilityLevelReached = shipProto.GetAbilityCardLevelByIndex(index);
                //slot.abilityLevelReached = abilityLevelReached;

                if (shipItem.HasUnlockedAbility(abu.ability.id))
                {
                    slot.SetState(ShipAbilitySlotBehaviour.State.Unlocked);
                }
                else
                {
                    hasAbuToUnlock = true;
                    var levelReq = abu.shipLevelRequire;

                    if (shipLevel >= levelReq)
                    {
                        slot.SetState(ShipAbilitySlotBehaviour.State.Unlockable);
                        hasAbuToUnlockAndLevelReqOk = true;
                    }
                    else
                    {
                        bool isNext = false;
                        if (index == 0)
                        {
                            isNext = true;
                        }
                        if (index > 0 && shipItem.HasUnlockedAbility(shipProto.abilityUnlocks[index - 1].ability.id))
                        {
                            isNext = true;
                        }
                        if (isNext)
                        {
                            slot.SetState(ShipAbilitySlotBehaviour.State.Next);
                        }
                        else
                        {
                            slot.SetState(ShipAbilitySlotBehaviour.State.None);
                        }

                    }
                }

                index++;
            }

            if (!hasAbuToUnlock)
            {
                desc.text = LocalizationService.instance.GetLocalizedText("AllAbUnlock");
            }
            else
            {
                if (hasAbuToUnlockAndLevelReqOk)
                {
                    desc.text = LocalizationService.instance.GetLocalizedText("AbUnlockable");
                }
                else
                {
                    desc.text = LocalizationService.instance.GetLocalizedText("ReachToUnlockAb");
                }
            }


            SetMainHud();
        }

        public void SetMainHud()
        {
            var shipProto = ShipService.instance.GetPrototype();
            List<string> resourceList = new List<string>();

            foreach (var abu in shipProto.abilityUnlocks)
            {
                var prices = abu.price;
                foreach (var price in prices)
                {
                    if (resourceList.IndexOf(price.id) < 0)
                    {
                        resourceList.Add(price.id);
                    }
                }
            }
            if (resourceList.Count < 4)
                resourceList.Add("Gold");
            if (resourceList.Count < 4)
                resourceList.Add("Diamond");
            if (resourceList.Count < 4)
                resourceList.Add("");
            if (resourceList.Count < 4)
                resourceList.Add("");

            MainHudBehaviour.instance.SetMode(false, resourceList[0], resourceList[1], resourceList[2], resourceList[3]);
            MainHudBehaviour.instance.Refresh();
        }

        public void PlayEffectLearnSlot(RectTransform rect)
        {
            psLearnSlot.transform.SetParent(rect.transform);
            var rectPs = psLearnSlot.GetComponent<RectTransform>();
            rectPs.anchoredPosition = new Vector2(0, 0);
            psLearnSlot.Play(true);
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("ShipAbilityTitle");
            data.content = LocalizationService.instance.GetLocalizedText("ShipAbilityContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }
    }
}
