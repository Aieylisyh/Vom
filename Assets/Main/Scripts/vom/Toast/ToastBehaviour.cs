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

        bool _fading;
        float _hideTimestamp;

        public void Show(ToastData toastData)
        {
            var cfg = game.ConfigService.instance.toastConfig;
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

            if (toastData.itemCount > 1)
                amount.text = toastData.itemCount + "";

            var targetSize = new Vector2(rect.sizeDelta.x, 0);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);

            rect.DOKill();
            rect.DOSizeDelta(targetSize, cfg.expandDuration);

            gameObject.SetActive(true);
            cg.alpha = 1;
            _fading = false;
            _hideTimestamp = GameTime.time + cfg.duration;
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