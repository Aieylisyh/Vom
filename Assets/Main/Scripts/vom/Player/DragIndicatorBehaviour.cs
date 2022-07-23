using UnityEngine;
using DG.Tweening;

namespace vom
{
    public class DragIndicatorBehaviour : MonoBehaviour
    {
        CanvasGroup _cg;
        public float duration = 1.0f;
        public float alpha = 0.7f;
        Tween _tween;
        bool _show;

        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        public void Hide()
        {
            if (!_show)
                return;

            _show = false;
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }

            _cg.alpha = 0;
        }

        public void Show()
        {
            if (_show)
                return;

            _show = true;
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }

            _tween = DOTween.To(() => { return _cg.alpha; }, x => _cg.alpha = x, alpha, duration).SetDelay(0.35f).Play();
        }
    }
}