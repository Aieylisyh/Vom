using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace vom
{
    public class SceneInteractionUiBehaviour : MonoBehaviour
    {
        public Image progress;
        public Image icon;
        [HideInInspector]
        public SceneInteractionTargetBehaviour host;
        public RectTransform view;
        public Camera cam;

        RectTransform _rect;
        float _canvasScale;
        SceneInteractionData _data;

        public void Init(SceneInteractionTargetBehaviour target, SceneInteractionData data)
        {
            _rect = GetComponent<RectTransform>();
            _data = data;
            this.host = target;

            icon.sprite = data.sp;
            SyncProgress(0);
            view.localScale = Vector3.zero;
            view.DOScale(1, 0.5f).SetEase(Ease.OutBack);

            _canvasScale = (float)Screen.width / 720;
            if (Screen.width > (float)Screen.height)
                _canvasScale = (float)Screen.height / 1280;
        }

        private void Update()
        {
            if (host == null)
                return;

            var pos = com.Convert2DAnd3D.GetScreenPosition(cam, host.transform.position, _canvasScale);
            _rect.anchoredPosition = pos;
            // _rect.position = host.transform.position;
            // var p = _rect.anchoredPosition3D;
            // p.z = 0;
            // _rect.anchoredPosition3D = p;
        }

        public void Remove()
        {
            host = null;
            Destroy(gameObject);
        }

        public void SyncProgress(float p)
        {
            progress.fillAmount = p;
        }

        public void OnClick()
        {
            PlayerBehaviour.instance.interaction.Interact(this.host);
        }
    }
}