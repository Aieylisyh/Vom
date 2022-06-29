using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using com;

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
        bool _started;
        float _passedTimer;

        public void Init(SceneInteractionTargetBehaviour target, SceneInteractionData data)
        {
            _rect = GetComponent<RectTransform>();
            _data = data;
            target.ui = this;
            this.host = target;
            _passedTimer = 0;
            _started = false;

            icon.sprite = data.sp;
            SyncProgress(0);
            view.localScale = Vector3.zero;
            view.DOScale(1, 0.5f).SetEase(Ease.OutBack);

            _canvasScale = (float)Screen.width / 720;
            if (Screen.width > (float)Screen.height)
            {
                _canvasScale = (float)Screen.height / 1280;
            }
        }

        private void Update()
        {
            var pos = com.Convert2DAnd3D.GetScreenPosition(cam, host.transform.position, _canvasScale);
            _rect.anchoredPosition = pos;
            // _rect.position = host.transform.position;
            // var p = _rect.anchoredPosition3D;
            // p.z = 0;
            // _rect.anchoredPosition3D = p;
            if (_started)
            {
                _passedTimer += GameTime.deltaTime;
                if (_passedTimer > _data.duration)
                {
                    _passedTimer = _data.duration;
                }
                SyncProgress(_passedTimer / _data.duration);
                if (_passedTimer == _data.duration)
                {
                    OnFinish();
                }
            }
        }

        void OnFinish()
        {
            Debug.Log("OnFinish");
            _started = false;
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
            //Debug.Log("onClick");
            _started = true;
        }
    }
}