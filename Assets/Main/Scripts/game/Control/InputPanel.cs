using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using com;

namespace game
{
    public class InputPanel : UIBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler
    {
        public static InputPanel instance { get; private set; }
        public float ignoreDeltaMagnitude = 15;
        public RectTransform canvasTrans;

        private float _timestampTap;
        public float canvasScale { get; private set; }

        private Vector2 _delta;
        private Vector2 _dragStartPos;
        public bool IsDraging { get; private set; }
        private Vector2 startPos;

        protected InputPanel()
        {
            //Debug.Log("InputPanel");
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            canvasScale = canvasTrans.localScale.x;
        }

        public Vector2 GetDelta()
        {
            //if (!IsDraging)
            //{
            //    return Vector2.zero;
            //}
            return _delta;
        }

        public bool DragRangeValid()
        {
            return _delta.magnitude > ignoreDeltaMagnitude;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!GameFlowService.instance.IsGameplayControlEnabled())
                return;

            IsDraging = true;
            _delta += eventData.delta;
            Debug.Log(_delta);
            //OnMoved
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SceneInputSystem.instance.InputPanelDown(eventData);
            if (!GameFlowService.instance.IsGameplayControlEnabled())
                return;
            //Debug.Log("OnPointerDown");
            _timestampTap = Time.unscaledTime;
            _delta = Vector2.zero;
            _dragStartPos = eventData.position;
        }

        public Vector2 GetStartPos()
        {
            return _dragStartPos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit");
            // IsDraging = false;
            //   _delta = Vector2.zero;
            //   longPressController.OnRelease();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SceneInputSystem.instance.InputPanelRelease(eventData);
            //Debug.Log("OnPointerUp");
            if (!GameFlowService.instance.IsGameplayControlEnabled())
                return;

            IsDraging = false;
            _delta = Vector2.zero;

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SceneInputSystem.instance.InputPanelClick(eventData);
        }
    }
}