using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class FishingAbilityPanelBehaviour : MonoBehaviour
    {
        public Text desc;
        public List<FishingAbilitySlotBehaviour> slots;
        public ParticleSystem psLearnSlot;

        public void Refresh()
        {
            FishingWindowBehaviour.instance.ShowBoatLevel();
            FishingBoatBehaviour.instance.StopPartViews();
            FishingBoatBehaviour.instance.UpdatePartViews();

            var item = FishingService.instance.GetItem();
            var cfg = FishingService.instance.GetConfig();
            var unlockedAbIds = item.saveData.unlockedAbilities;
            var boatLevel = FishingService.instance.GetBoatLevel();

            bool hasAbuToUnlock = false;
            bool hasAbuToUnlockAndLevelReqOk = false;

            int index = 0;
            foreach (var abu in cfg.abilityUnlocks)
            {
                var slot = slots[index];
                slot.proto = abu;

                if (FishingService.instance.HasUnlockedAbility(abu.ability.id))
                {
                    slot.SetState(FishingAbilitySlotBehaviour.State.Unlocked);
                }
                else
                {
                    hasAbuToUnlock = true;

                    var levelReq = abu.boatLevelRequire;
                    if (boatLevel >= levelReq)
                    {
                        slot.SetState(FishingAbilitySlotBehaviour.State.Unlockable);
                        hasAbuToUnlockAndLevelReqOk = true;
                    }
                    else
                    {
                        bool isNext = false;
                        if (index == 0)
                        {
                            isNext = true;
                        }
                        if (index > 0 && FishingService.instance.HasUnlockedAbility(cfg.abilityUnlocks[index - 1].ability.id))
                        {
                            isNext = true;
                        }

                        if (isNext)
                        {
                            slot.SetState(FishingAbilitySlotBehaviour.State.Next);
                        }
                        else
                        {
                            slot.SetState(FishingAbilitySlotBehaviour.State.None);
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
                    desc.text = LocalizationService.instance.GetLocalizedText("ReachToUnlockFishingAb");
                }
            }
            //MainHudBehaviour.instance.Hide();
        }

        public void SetMainHud(string res1 = "", string res2 = "", string res3 = "", string res4 = "")
        {
            MainHudBehaviour.instance.SetMode(false, res1, res2, res3, res4);
            MainHudBehaviour.instance.Show();
        }

        public void PlayEffectLearnSlot(RectTransform rect)
        {
            psLearnSlot.transform.SetParent(rect.transform);
            var rectPs = psLearnSlot.GetComponent<RectTransform>();
            rectPs.anchoredPosition = new Vector2(0, 0);
            psLearnSlot.Play(true);
        }
    }
}
