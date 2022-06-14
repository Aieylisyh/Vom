using UnityEngine;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class TalentWindowBehaviour : WindowBehaviour
    {
        public WindowTabsBehaviour wtb;
        public TalentPanelBehaviour talentPanelHull;
        public TalentPanelBehaviour talentPanelBomb;
        public TalentPanelBehaviour talentPanelTorpedo;

        public static TalentWindowBehaviour instance;
        public Text pointSpentCategoryTxt;
        public Text pointRestTxt;
        public GameObject btnReset;

        public ParticleSystem psReset;
        public ParticleSystem psLearn;
        public ParticleSystem psLearnSlot;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Setup()
        {
            base.Setup();
            wtb.SetTab(1);
            OnShowTalentBomb();
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            MainHudBehaviour.instance.RefreshToDefault();
        }

        public override void Hide()
        {
            base.Hide();

            wtb.Off();
            TalentPanelBehaviour.currentDisplayingInstance = null;
        }

        public void PlayEffectLearn()
        {
            psLearn.Play(true);
        }

        public void PlayEffectLearnSlot(TalentSlotBehaviour slot)
        {
            psLearnSlot.transform.SetParent(slot.transform);
            var rect = psLearnSlot.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -5);
            psLearnSlot.Play(true);
        }

        public void PlayEffectReset()
        {
            psReset.Play(true);
        }

        private void RefreshPoints()
        {
            //int hullPoints = TalentService.instance.GetAssignedTpsOfCategory(talentPanelHull.category);
            //int bombPoints = TalentService.instance.GetAssignedTpsOfCategory(talentPanelBomb.category);
            //int torpedoPoints = TalentService.instance.GetAssignedTpsOfCategory(talentPanelTorpedo.category);
            //int totalSpent = hullPoints + bombPoints + torpedoPoints;
            int totalSpent = TalentService.instance.GetAssignedTps();

            var cateCrt = TalentPanelBehaviour.currentDisplayingInstance.category;
            var pointSpentCategory = TalentService.instance.GetAssignedTpsOfCategory(cateCrt);
            var categoryTitle = cateCrt.ToString();
            pointSpentCategoryTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("SpentTalentsCategory",
                LocalizationService.instance.GetLocalizedText(categoryTitle),
                pointSpentCategory);

            int crtPoint = TalentService.instance.GetTalentPoint();
            //string paramCrtPoint = "<size=175%>" + crtPoint + "</size>";
            //pointRestTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("TalentPoints", paramCrtPoint);
            pointRestTxt.text = "<sprite name=TalentPoint><size=32> : </size>" + crtPoint;
        }

        public void Refresh()
        {
            MainHudBehaviour.instance.RefreshToDefault();
            TalentPanelBehaviour.currentDisplayingInstance.Setup();
            TalentPanelBehaviour.currentDisplayingInstance.Refresh();

            RefreshPoints();
            ShowResetView();
        }

        public void OnShowTalentHull()
        {
            TalentPanelBehaviour.currentDisplayingInstance = talentPanelHull;
            Refresh();
        }
        public void OnShowTalentBomb()
        {
            TalentPanelBehaviour.currentDisplayingInstance = talentPanelBomb;
            Refresh();
        }
        public void OnShowTalentTorpedo()
        {
            TalentPanelBehaviour.currentDisplayingInstance = talentPanelTorpedo;
            Refresh();
        }

        public void OnClickResetTalents()
        {
            SoundService.instance.Play("btn info");
            if (TalentService.instance.GetAssignedTps() < 1)
            {
                return;
            }

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("ResetTalents");
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ResetTalents");
            var priceString = TextFormat.GetItemText(TalentService.instance.GetResetPrice(), true);
            var contentString = LocalizationService.instance.GetLocalizedTextFormatted("ResetTalentsContent", priceString);
            data.content = contentString;
            data.btnLeftAction = () => { TryResetTalents(); };
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        private void ShowResetView()
        {
            var price = TalentService.instance.GetResetPrice();
            if (price.n < 1)
            {
                btnReset.SetActive(false);
            }
            else
            {
                btnReset.SetActive(true);
            }
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("Talent");
            data.content = LocalizationService.instance.GetLocalizedText("Talent_desc");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void TryResetTalents()
        {
            var res = ItemService.instance.IsPriceAffordable(TalentService.instance.GetResetPrice(), true);
            if (res.success)
            {
                WaitingCircleBehaviour.instance.SetHideAction(() =>
                {
                    UxService.instance.gameDataCache.cache.resetTalentCount += 1;
                    UxService.instance.SaveGameData();

                    TalentService.instance.ResetAllTalents();
                    PlayEffectReset();
                    Refresh();
                    SoundService.instance.Play("talent reset");
                });
                WaitingCircleBehaviour.instance.Show(1f);
            }
            else
            {
                SoundService.instance.Play("btn info");
                var data = new ConfirmBoxPopup.ConfirmBoxData();
                data.btnClose = false;
                data.btnBgClose = true;
                data.btnLeft = false;
                data.btnRight = false;
                data.title = LocalizationService.instance.GetLocalizedText("ResetTalentsFailTitle");
                data.content = LocalizationService.instance.GetLocalizedText("ResetTalentsFailContent");
                WindowService.instance.ShowConfirmBoxPopup(data);
            }
        }
    }
}
