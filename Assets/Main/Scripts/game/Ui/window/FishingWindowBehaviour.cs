using UnityEngine;
using DG.Tweening;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using System;

namespace game
{
    public class FishingWindowBehaviour : WindowBehaviour
    {
        public WindowTabsBehaviour wtb;
        public static FishingWindowBehaviour instance;
        public FishingTaskPanelBehaviour fishingTaskPanelBehaviour;
        public FishingAbilityPanelBehaviour abilityPanelBehaviour;
        public FishingAttributePanelBehaviour attributePanelBehaviour;

        public ParticleSystem psFabLearned;
        public ParticleSystem psLevelup;
        public ParticleSystem psRftDone;
        public ParticleSystem psRftStart;

        public Slider slider;
        public Text boatLevelTxt;
        public Text boatLevelTitleTxt;
        public GameObject boatLevelPart;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Setup()
        {
            base.Setup();

            RefreshToTask();
        }

        private void RefreshToTask()
        {
            TryTriggerRftReward();//first check finish then refresh
            wtb.SetTab(0);//Task
            OnShowTask();
            SetWtbAmount();
        }

        private void SetWtbAmount()
        {
            var boatOk = FishingBoatBehaviour.instance.IsAvailable();
            //Debug.Log("SetWtbAmount boatOk"+ boatOk);
            boatOk = true;//test
            if (!boatOk)
            {
                wtb.SetTo1TabLayout(0);
                return;
            }

            var hasBoatLevelupUnlocked = FishingService.instance.HasBoatLevelupUnlocked();
            var hasBoatAbilityUnlocked = FishingService.instance.HasBoatAbilityUnlocked();
            if (hasBoatLevelupUnlocked && hasBoatAbilityUnlocked)
            {
                wtb.SetTo3TabsLayout(0, 1, 2);
            }
            else if (hasBoatLevelupUnlocked)
            {
                wtb.SetTo2TabsLayout(0, 1);
            }
            else
            {
                wtb.SetTo1TabLayout(0);
            }
        }

        public void TickPrice()
        {
            if (!cg.interactable)
                return;

            if (wtb.GetCurrentTab() == 1)
            {
                fishingTaskPanelBehaviour.Refresh();
            }
        }

        public void TryTriggerRftReward()
        {
            //Debug.Log("TryTriggerRftReward");
            var boatOk = FishingBoatBehaviour.instance.IsAvailable();
            var HasFinishedRft = FishingService.instance.HasFinishedRft();
            //Debug.Log("boatOk " + boatOk);
            //Debug.Log("HasFinishedRft " + HasFinishedRft);
            if (boatOk && HasFinishedRft)
            {
                var rewards = FishingService.instance.GetRftRewards();
                //Debug.Log(rewards);
                if (rewards == null || rewards.Count < 1)
                    return;

                //Debug.Log("has rewards");
                foreach (var reward in rewards)
                {
                    UxService.instance.AddItem(reward);
                }

                SoundService.instance.Play("reward");
                var itemsData = new ItemsPopup.ItemsPopupData();
                itemsData.clickBgClose = false;
                itemsData.hasBtnOk = true;
                itemsData.title = LocalizationService.instance.GetLocalizedText("RftTitle");

                var item = FishingService.instance.GetItem();
                var durationTicks = FishingService.instance.GetRftDuration_TimeSpanTicks();
                var durationTimeSpan = TimeSpan.FromTicks(durationTicks);
                string rftString1 = durationTimeSpan.ToString(@"hh\:mm\:ss");
                //Debug.Log(rftString1);

                var otDurationTicks = FishingService.instance.GetOvertimeValidatedDurationTicks();
                //Debug.Log("otDurationTicks " + otDurationTicks);
                string rftString2 = "";
                if (otDurationTicks > 0)
                {
                    var otTimeSpan = TimeSpan.FromTicks(otDurationTicks);
                    // var otHours = otTimeSpan.TotalHours;
                    rftString2 = LocalizationService.instance.GetLocalizedTextFormatted("RftContentS2", otTimeSpan.ToString(@"hh\:mm\:ss"));
                }
                // Debug.Log(rftString2);
                itemsData.content = LocalizationService.instance.GetLocalizedTextFormatted("RftContent", rftString1, rftString2);
                itemsData.items = rewards;
                WindowService.instance.ShowItemsPopup(itemsData);

                FishingService.instance.FinishRft();
            }
        }

        public void OnBoatArrived()
        {
            PlayEffectRftDone();
            if (!cg.interactable)
                return;

            RefreshToTask();
        }

        public void OnAbilityLearned()
        {
            wtb.SetTab(2);//ability
            abilityPanelBehaviour.Refresh();
            PlayEffectFabLearned();
        }

        public void OnShipLeveluped()
        {
            wtb.SetTab(1);//attri
            attributePanelBehaviour.Refresh();
            PlayEffectLevelup();

            var go = Instantiate(attributePanelBehaviour.particleGo, attributePanelBehaviour.particleGo.transform.parent);
            go.SetActive(true);
            Destroy(go, 1);
        }

        public void OnBoatLeave()
        {
            PlayEffectRftGo();
            RefreshToTask();
        }

        public void OnBoatArriving()
        {
            if (!cg.interactable)
                return;

            if (wtb.GetCurrentTab() == 1)
            {
                fishingTaskPanelBehaviour.Refresh();
            }
        }

        public void OnAcced()
        {
            RefreshToTask();
        }

        private void PlayEffectLevelup()
        {
            psLevelup.Play(true);
        }

        private void PlayEffectFabLearned()
        {
            psFabLearned.Play(true);
        }

        private void PlayEffectRftDone()
        {
            SoundService.instance.Play(new string[2] { "pay1", "pay2" });
            psRftDone.Play(true);
        }

        private void PlayEffectRftGo()
        {
            SoundService.instance.Play("btn big");
            psRftStart.Play(true);
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            FishingBoatBehaviour.instance.StopPartViews();
            MainHudBehaviour.instance.RefreshToDefault();
        }

        public override void Hide()
        {
            base.Hide();
            wtb.Off();
        }

        public void OnShowTask()
        {
            MainHudBehaviour.instance.RefreshToDefault();
            fishingTaskPanelBehaviour.Refresh();
        }

        public void OnShowAttribute()
        {
            MainHudBehaviour.instance.RefreshToDefault();
            attributePanelBehaviour.Refresh();
        }

        public void OnShowAbility()
        {
            abilityPanelBehaviour.Refresh();
        }

        public void ShowBoatLevel()
        {
            boatLevelPart.SetActive(true);

            var item = FishingService.instance.GetItem();
            var level = item.saveData.boatLevel;
            var cfg = FishingService.instance.GetConfig();
            boatLevelTxt.text = level + "/" + cfg.boatLevelData.maxLevel;
            boatLevelTitleTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("BoatLevelTitle", level);
            slider.value = (float)level / (float)cfg.boatLevelData.maxLevel;
        }

        public void HideBoatLevel()
        {
            boatLevelPart.SetActive(false);
        }
    }
}