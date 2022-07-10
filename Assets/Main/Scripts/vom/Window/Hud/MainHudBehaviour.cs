using UnityEngine;
using DG.Tweening;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.UI;

namespace vom
{
    public class MainHudBehaviour : MonoBehaviour
    {
        public static MainHudBehaviour instance { get; private set; }

        public Image expBar;

        public Text goldTxt;
        public Text soulTxt;

        public Text levelTxt;

        public float minBarValue;
        public float maxBarValue;
        //public Text expCrtTxt;
        //public Text expMaxTxt;

        public float duration = 1f;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SyncGold();
            SyncSoul();
            SyncExp();
        }

        public void SyncGold()
        {
            var amount = InventorySystem.instance.GoldCount;
            var endTxt = "" + amount;
            if (goldTxt.text != endTxt)
            {
                if (goldTxt.text == "")
                {
                    goldTxt.text = endTxt;
                }
                else
                {
                    goldTxt.DOKill();
                    goldTxt.DOText(endTxt, duration, false, ScrambleMode.Numerals);
                }
            }
        }

        public void SyncSoul()
        {
            var amount = InventorySystem.instance.SoulCount;
            var endTxt = "" + amount;
            if (soulTxt.text != endTxt)
            {
                if (soulTxt.text == "")
                {
                    soulTxt.text = endTxt;
                }
                else
                {
                    soulTxt.DOKill();
                    soulTxt.DOText(endTxt, duration, false, ScrambleMode.Numerals);
                }
            }
        }

        public void SyncExp()
        {
            var currentExp = DailyPerkSystem.instance.exp;
            var maxExp = DailyPerkSystem.instance.expMax;
            var level = DailyPerkSystem.instance.level;

            bool levelChanged = false;
            if (levelTxt.text != "" + level)
            {
                levelChanged = true;
                levelTxt.text = "" + level;
                levelTxt.rectTransform.localScale = Vector3.one * 0.35f;
                levelTxt.DOKill();
                levelTxt.DOScale(1, duration).SetEase(Ease.OutElastic);
            }

            float r = ((float)currentExp / maxExp);
            var endValue = minBarValue + r * (1 - maxBarValue - minBarValue);

            if (levelChanged)
            {
                expBar.fillAmount = endValue;
            }
            else
            {
                expBar.DOKill();
                expBar.DOFillAmount(endValue, duration);
            }
        }
    }
}