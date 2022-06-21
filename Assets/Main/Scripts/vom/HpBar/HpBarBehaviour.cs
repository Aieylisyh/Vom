using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace vom
{
    public class HpBarBehaviour : MonoBehaviour
    {
        public float duration = 0.6f;
        public Image bar;
        public Image bar_shadow;

        public RectTransform rectTrans;
        public CanvasGroup cg;

        public void Hide()
        {
            cg.alpha = 0;
        }

        public void Show()
        {
            cg.alpha = 1;
        }

        public void Set(float percentage, bool instant = false)
        {
            bar.fillAmount = percentage;

            if (bar_shadow != null)
            {
                if (!instant && duration > 0)
                {
                    bar_shadow.DOFillAmount(percentage, duration).SetEase(Ease.OutCubic);
                }
                else
                {
                    bar_shadow.fillAmount = percentage;
                }
            }
        }
    }
}
