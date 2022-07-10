using UnityEngine;
using DG.Tweening;
using com;
using System.Collections.Generic;

namespace vom
{
    public class WindowBehaviour : MonoBehaviour
    {
        public CanvasGroup cg;
        public RectTransform mainWindow;

        public static List<WindowBehaviour> instances = new List<WindowBehaviour>();

        protected virtual void Awake()
        {
            instances.Add(this);
            Hide();
        }

        public virtual void Show()
        {
            Setup();
            //mainWindow.gameObject.SetActive(true);
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
            //Debug.Log("Show");
        }

        public void ReOpen()
        {
            if (cg.alpha==1)
            {
                Setup();
            }
        }

        public virtual void Setup()
        {
            SoundService.instance.Play("btn dialog");
        }

        public virtual void Hide()
        {
            //Debug.Log("Hide");
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
            //mainWindow.gameObject.SetActive(false);
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
