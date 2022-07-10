using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using com;
using vom;

namespace game
{
    public class InputPanel : UIBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerClickHandler
    {
        public static InputPanel instance { get; private set; }
        public float ignoreDeltaMagnitude = 15;
        public RectTransform canvasTrans;

        private float _timestampTap;
        public float canvasScale { get; private set; }

        private bool _disabledInput;

        protected InputPanel()
        {
            //Debug.Log("InputPanel");
        }

        public void DisableInput()
        {
            _disabledInput = true;
            PlayerBehaviour.instance.move.EndDrag();
        }

        public void EnableInput()
        {
            _disabledInput = false;
        }
        protected override void Awake()
        {
            base.Awake();
            instance = this;
            canvasScale = canvasTrans.localScale.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_disabledInput)
                return;

            PlayerBehaviour.instance.move.UpdateDrag(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_disabledInput)
                return;

            //Debug.Log("OnPointerDown");
            _timestampTap = Time.unscaledTime;
            PlayerBehaviour.instance.move.StartDrag(eventData.position);
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
            if (_disabledInput)
                return;

            //Debug.Log("OnPointerUp");

            PlayerBehaviour.instance.move.EndDrag();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_disabledInput)
                return;
        }
    }
}