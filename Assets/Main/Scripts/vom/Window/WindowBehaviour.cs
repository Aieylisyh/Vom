using UnityEngine;
using DG.Tweening;
using com;
using System.Collections.Generic;

namespace vom
{
    public class WindowBehaviour : MonoBehaviour
    {
        CanvasGroup _cg;
        public RectTransform mainWindow;

        public static List<WindowBehaviour> instances = new List<WindowBehaviour>();

        protected virtual void Awake()
        {
            instances.Add(this);

            _cg = GetComponent<CanvasGroup>();
            Hide();
        }

        public virtual void Show()
        {
            Setup();
            mainWindow.gameObject.SetActive(true);
            _cg.alpha = 1;
            _cg.blocksRaycasts = true;
            _cg.interactable = true;
        }

        public void ReOpen()
        {
            if (_cg.alpha == 1)
                Setup();
        }

        public virtual void Setup()
        {
            SoundService.instance.Play("btn dialog");
        }

        public virtual void Hide()
        {
            _cg.alpha = 0;
            _cg.blocksRaycasts = false;
            _cg.interactable = false;
            mainWindow.gameObject.SetActive(false);
        }

        public virtual void OnClickBtnClose()
        {
            SoundService.instance.Play("btn small");
            Hide();
        }

        protected virtual void Sound()
        {
            SoundService.instance.Play("tap");
        }
    }
}
