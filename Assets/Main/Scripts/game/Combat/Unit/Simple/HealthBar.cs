using UnityEngine;
using com;
using UnityEngine.UI;
using DG.Tweening;
namespace game
{
    public class HealthBar : MonoBehaviour
    {
        public float duration = 0.35f;
        public Image bar;

        private RectTransform _barTrans;
        public GameObject target;
        public CanvasGroup cg;

        public bool useScaleMode = false;

        private void Awake()
        {
            _barTrans = bar.GetComponent<RectTransform>();
        }

        public void Hide()
        {
            if (cg != null)
            {
                cg.alpha = 0;
            }
            else
            {
                target.SetActive(false);
            }
        }

        public void Set(float percentage, bool instant = false)
        {
            //Debug.Log("HealthBar set "+percentage);
            if (useScaleMode)
            {
                if (!instant && duration > 0)
                {
                    _barTrans.DOScaleX(percentage, duration).SetEase(Ease.OutCubic);
                }
                else
                {
                    _barTrans.localScale = new Vector3(percentage, 1, 1);
                }
            }
            else
            {
                if (!instant && duration > 0)
                {
                    bar.DOFillAmount(percentage, duration).SetEase(Ease.OutCubic);
                }
                else
                {
                    bar.fillAmount = percentage;
                }
            }

            if (cg != null)
            {
                cg.alpha = 1;
            }
            else
            {
                target.SetActive(true);
            }
        }
    }
}
