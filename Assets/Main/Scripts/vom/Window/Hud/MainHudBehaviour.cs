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
        CanvasGroup _cg;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _cg = GetComponent<CanvasGroup>();
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
                goldTxt.DOKill();
                if (goldTxt.text == "")
                {
                    goldTxt.text = endTxt;
                }
                else
                {
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
                soulTxt.DOKill();
                if (soulTxt.text == "")
                {
                    soulTxt.text = endTxt;
                }
                else
                {
                    soulTxt.DOText(endTxt, duration, false, ScrambleMode.Numerals);
                }
            }
        }

        public void SyncExp()
        {
            var exp = DailyPerkSystem.instance.exp;
            var maxExp = DailyPerkSystem.instance.expMax;
            var level = DailyPerkSystem.instance.level;
            // Debug.Log(exp + " " + maxExp + " " + level);
            bool levelChanged = false;
            if (levelTxt.text != "" + level)
            {
                levelChanged = true;
                levelTxt.text = "" + level;
                levelTxt.rectTransform.localScale = Vector3.one * 0.35f;
                levelTxt.DOKill();
                levelTxt.DOScale(1, duration).SetEase(Ease.OutElastic);
            }

            float r = ((float)exp / maxExp);
            if (r > 1)
                r = 1f;

            float endValue = minBarValue + r * (maxBarValue - minBarValue);
            expBar.DOKill();
            if (levelChanged)
            {
                expBar.fillAmount = endValue;
            }
            else
            {
                expBar.DOFillAmount(endValue, duration);
            }
        }

        public void Show()
        {
            _cg.alpha = 1;
            _cg.blocksRaycasts = true;
            _cg.interactable = true;
        }

        public void Hide()
        {
            _cg.alpha = 0;
            _cg.blocksRaycasts = false;
            _cg.interactable = false;
        }
    }
}