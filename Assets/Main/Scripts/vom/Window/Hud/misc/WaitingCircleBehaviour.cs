using UnityEngine;
using System;

namespace vom
{
    public class WaitingCircleBehaviour : MonoBehaviour
    {
        public GameObject view;
        private float _timer;
        public static WaitingCircleBehaviour instance;
        private Action _action;

        private void Awake()
        {
            instance = this;
        }

        public void Show()
        {
            view.SetActive(true);
        }

        public void SetHideAction(Action action = null)
        {
            _action = action;
        }

        public void Hide()
        {
            view.SetActive(false);
            _action?.Invoke();
            _action = null;
        }

        public void Show(float time)
        {
            _timer = time;
            Show();
        }

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    Hide();
                }
            }
        }
    }
}