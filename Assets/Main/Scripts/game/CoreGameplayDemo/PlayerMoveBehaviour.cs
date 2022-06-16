using UnityEngine;
using System.Collections;

namespace vom
{
    public class PlayerMoveBehaviour : MonoBehaviour
    {
        public static PlayerMoveBehaviour instance { get; private set; }

        private bool _draging;
        private Vector2 _startPos;
        private Vector2 _lastPos;
        public float ignoreThreshold = 10;

        private void Awake()
        {
            instance = this;
        }

        public void StartDrag(Vector2 pos)
        {
            _draging = true;
            _startPos = pos;
            _lastPos = pos;
        }

        public void UpdateDrag(Vector2 pos)
        {
            _lastPos = pos;
        }

        public void EndDrag()
        {
            _draging = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_draging)
            {
                var delta = _lastPos - _startPos;
                if (delta.magnitude > ignoreThreshold)
                {
                    PlayerBehaviour.instance.ReceiveMoveInput(delta);
                }
            }
        }
    }
}