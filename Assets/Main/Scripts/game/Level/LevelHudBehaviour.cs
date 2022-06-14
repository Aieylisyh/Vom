using UnityEngine;
using DG.Tweening;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class LevelHudBehaviour : MonoBehaviour
    {
        public static LevelHudBehaviour instance { get; private set; }

        public CanvasGroup cg;
        public Text levelName;
        public Text levelSub;
        public GameObject pauseButton;
        public GameObject bombs;
        //public ShakeBehaviour pauseButtonShake;
        public GameObject[] cabToggleShows;
        public GameObject[] cabToggleHides;

        public BlinkTipBehaviour btb;

        private void Awake()
        {
            instance = this;
            Hide();
        }

        public void SetupForCombat()
        {
            var runtimeLevel = LevelService.instance.runtimeLevel;
            string pLevelName = runtimeLevel.levelProto.title;
            string pLevelDesc = runtimeLevel.levelProto.desc;

            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
            levelName.text = LocalizationService.instance.GetLocalizedText(pLevelName);
            levelSub.text = "";

            HideTip();
            pauseButton.SetActive(true);
            bombs.SetActive(true);
        }

        public void UpdateWave(int crt, int total, bool animate)
        {
            var s = crt + "/" + total;
            levelSub.text = LocalizationService.instance.GetLocalizedTextFormatted("CrtWave", s);
            if (animate)
            {
                levelSub.rectTransform.DOPunchScale(Vector3.one * 0.35f, 0.65f, 1, 1);
            }
        }

        public void Hide()
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
            HideTip();
        }

        public void SetupEndCab()
        {
            foreach (var s in cabToggleShows)
            {
                s.SetActive(false);
            }
            foreach (var s in cabToggleHides)
            {
                s.SetActive(true);
            }

            CombatAbilitySelectionBehaviour.instance.Hide();
        }

        public void SetupForCab()
        {
            foreach (var s in cabToggleShows)
            {
                s.SetActive(true);
            }
            foreach (var s in cabToggleHides)
            {
                s.SetActive(false);
            }
        }

        public void ShowTip(string s)
        {
            btb.Show(s);
        }

        public void ShowTip(string s, System.Func<bool> endReason)
        {
            btb.Show(s, endReason);
        }

        public void HideTip()
        {
            //Debug.LogWarning("HideTip" );
            btb.Hide(true);
        }
    }
}
