using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using DG.Tweening;

namespace vom
{
    public class ChatBubbleBehaviour : MonoBehaviour
    {
        public CanvasGroup cg;
        public Text txt;

        public void Setup(string s)
        {
            txt.text = s;
            cg.DOFade(1, 0.5f);
        }
    }
}