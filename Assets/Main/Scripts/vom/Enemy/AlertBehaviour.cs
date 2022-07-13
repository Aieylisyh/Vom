using UnityEngine;
using DG.Tweening;
using com;

namespace vom
{
    public class AlertBehaviour : MonoBehaviour
    {
        public RectTransform rectTrans;
        public CanvasGroup cg;

        private bool _hiding;
        public float hideTime = 0.5f;
        float _hideTimestamp;

        void Start()
        {
            _hiding = false;
            cg.alpha = 1;
            _hideTimestamp = GameTime.time + hideTime;
        }

        private void Update()
        {
            if (_hiding)
                return;

            if (GameTime.time > _hideTimestamp)
                Hide();
        }

        public void Hide()
        {
            if (_hiding)
                return;

            _hiding = true;
            cg.DOKill();
            cg.DOFade(0, 0.4f).OnComplete(() => { Destroy(gameObject); });
        }
    }
}