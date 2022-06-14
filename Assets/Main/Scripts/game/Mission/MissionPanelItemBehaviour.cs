using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MissionPanelItemBehaviour : MonoBehaviour
    {
        public Color completeColor;
        public Color defaultColor;
        public CanvasGroup cg;
        public Text text;

        public void Set(string ctt, int crtQuota, int quota, bool done)
        {
            text.color = done ? completeColor : defaultColor;
            text.text = LocalizationService.instance.GetLocalizedTextFormatted("MissionItem", ctt, crtQuota, quota);
        }
    }
}