using UnityEngine;
using DG.Tweening;

namespace vom
{
    public class DragIndicatorBehaviour : MonoBehaviour
    {
        CanvasGroup _cg;
        public float duration = 1.0f;
        public float alpha = 0.7f;

        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        public void Hide()
        {
            _cg.DOKill();
            _cg.alpha = 0;
        }

        public void Show()
        {
            _cg.DOKill();
            _cg.DOFade(alpha, duration);
        }
    }
}