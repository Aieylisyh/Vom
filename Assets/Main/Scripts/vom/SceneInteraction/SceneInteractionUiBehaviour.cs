using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace vom
{
    public class SceneInteractionUiBehaviour : MonoBehaviour
    {
        public Image progress;
        public Image icon;
        public SceneInteractionTargetBehaviour host;
        public RectTransform view;

        public void Init(SceneInteractionTargetBehaviour target, SceneInteractionData data)
        {
            target.ui = this;
            this.host = target;
            this.transform.position = target.transform.position;

            icon.sprite = data.sp;
            SyncProgress(0);
            view.localScale = Vector3.zero;
            view.DOScale(1, 0.5f).SetEase(Ease.OutBack);
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
            Debug.Log("onClick");
        }
    }
}