using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace game
{
    public class TouchViewBehaviour : MonoBehaviour
    {
        public RectTransform View;
        public RectTransform ArrowRotateTrans;
        public RectTransform ArrowSizeTrans;

        public float widthArrow = 15;
        public float arrowSizeFactor = 0.50f;

        private void Start()
        {
            Hide();
        }

        public void Hide()
        {
            View.gameObject.SetActive(false);
        }

        public void Show(Vector2 vec2)
        {
            if (!View.gameObject.activeSelf)
            {
                // var pos = InputPanel.instance.GetStartPos();
                var pos = Vector2.zero;


                //Debug.Log(pos);
                //Debug.Log(pos + "  " + pos / canvasTrans.localScale.x);
                //Debug.Log(Screen.height);
                //Debug.Log(Screen.width);
                //pos /= InputPanel.instance.canvasScale;
                pos.x *= 720f / Screen.width;
                //pos.y *= 1280f / Screen.height;
                pos.y *= 720f / Screen.width;


                View.anchoredPosition = pos;
                View.gameObject.SetActive(true);
            }

            float sizeFactor = vec2.magnitude;
            float rotDeg = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;

            ArrowRotateTrans.localEulerAngles = new Vector3(0, 0, rotDeg - 90);
            ArrowSizeTrans.sizeDelta = new Vector2(widthArrow, arrowSizeFactor * sizeFactor);
        }
    }
}