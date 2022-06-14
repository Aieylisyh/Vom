using UnityEngine;
using DG.Tweening;
using com;

namespace game
{
    public class PopupBehaviour : MonoBehaviour
    {
        public CanvasGroup cg;
        public GameObject bgBlocker;
        public RectTransform mainPopup;
        public float startScale = 0.75f;
        public float duration = 0.35f;

        private void Start()
        {
            Hide();
        }

        public virtual void Show()
        {
            bgBlocker.SetActive(true);
            mainPopup.gameObject.SetActive(true);
            cg.alpha = 0;
            cg.interactable = true;
            cg.blocksRaycasts = true;

            mainPopup.localScale = Vector3.one * startScale;
            mainPopup.DOScale(1, duration);

            //cg.DOFade(1, 0.5f);
            cg.alpha = 1;
            transform.SetAsLastSibling();
        }

        public virtual void Hide()
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            bgBlocker.SetActive(false);
            mainPopup.gameObject.SetActive(false);
        }

        public virtual void OnClickBtnClose()
        {
            //Debug.Log("OnClickBtnClose");
            Sound();
            Hide();
        }

        protected virtual void Sound()
        {
            SoundService.instance.Play("tap");
        }
    }
}