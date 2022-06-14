﻿using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ConfirmBoxPopup : PopupBehaviour
    {
        public Text title;
        public Text content;
        public GameObject btnRight;
        public GameObject btnLeft;
        public GameObject btnClose;
        public Text btnRightTxt;
        public Text btnLeftTxt;
        private ConfirmBoxData _data;

        private System.Action _closeAction;
        private System.Action _bgCloseAction;
        private System.Action _btnLeftAction;
        private System.Action _btnRightAction;
        public ResizeRectTransform resizer;

        public struct ConfirmBoxData
        {
            public System.Action closeAction;
            public System.Action bgCloseAction;
            public System.Action btnLeftAction;
            public System.Action btnRightAction;
            public string title;
            public string content;
            public bool btnRight;
            public bool btnLeft;
            public bool btnClose;
            public bool btnBgClose;
            public string btnRightTxt;
            public string btnLeftTxt;
        }

        public void Setup(ConfirmBoxData data)
        {
            _data = data;
            _closeAction = data.closeAction;
            _bgCloseAction = data.bgCloseAction;
            _btnLeftAction = data.btnLeftAction;
            _btnRightAction = data.btnRightAction;

            title.text = data.title;
            content.text = data.content;

            btnRight.SetActive(data.btnRight);
            btnLeft.SetActive(data.btnLeft);
            btnClose.SetActive(data.btnClose);

            if (data.btnRight)
            {
                btnRightTxt.text = data.btnRightTxt;
            }
            if (data.btnLeft)
            {
                btnLeftTxt.text = data.btnLeftTxt;
            }

            resizer.ResizeLater();
        }

        public void OnClickBgClose()
        {
            if (_data.btnBgClose)
            {
                _bgCloseAction?.Invoke();
                Hide();
                Sound();
            }
        }
        public override void OnClickBtnClose()
        {
            _closeAction?.Invoke();
            base.OnClickBtnClose();
        }

        public virtual void OnClickBtnLeft()
        {
            Hide();
            Sound();
            _btnLeftAction?.Invoke();
        }

        public virtual void OnClickBtnRight()
        {
            Hide();
            Sound();
            _btnRightAction?.Invoke();
        }
    }
}