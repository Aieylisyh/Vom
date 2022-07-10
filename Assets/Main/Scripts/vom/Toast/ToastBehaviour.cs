using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using DG.Tweening;
using com;

namespace vom
{
    public class ToastBehaviour : MonoBehaviour
    {
        public Image itemSp;
        public GameObject itemView;
        public Text title;
        public RectTransform rect;
        public Image bg;
        public Text amount;
        public CanvasGroup cg;
        public GameObject view;
        bool _fading;
        float _hideTimestamp;

        public void Show(ToastData toastData)
        {
            var cfg = ConfigSystem.instance.toastConfig;
            bg.color = toastData.bgColor;
            title.text = toastData.title;
            var targetHeight = cfg.heightPureText;

            if (toastData.sp == null)
            {
                itemView.SetActive(false);
            }
            else
            {
                itemView.SetActive(true);
                itemSp.sprite = toastData.sp;
                targetHeight = cfg.heightWithSp;
            }

            view.SetActive(false);
            if (toastData.itemCount > 0)
                amount.text = toastData.itemCount + "";
            else
                amount.text = "";

            var x = rect.sizeDelta.x;
            var targetSize = new Vector2(x, targetHeight);
            rect.DOKill();
            rect.sizeDelta = new Vector2(x, 0);

            rect.DOSizeDelta(targetSize, cfg.expandDuration).OnComplete(() =>
            {
                view.SetActive(true);
            });

            gameObject.SetActive(true);
            cg.DOKill();
            cg.alpha = 1;
            _fading = false;
            _hideTimestamp = GameTime.time + cfg.duration;
        }

        public bool IsExpanding()
        {
            return gameObject.activeSelf && !view.activeSelf;
        }

        private void Update()
        {
            if (!_fading && GameTime.time > _hideTimestamp)
            {
                _fading = true;
                cg.DOFade(0, 2f).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            }
        }
    }
}