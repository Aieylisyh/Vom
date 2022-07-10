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

        private void Awake()
        {
            instance = this;
        }

        public void SyncGold(bool instant)
        {

        }
    }
}