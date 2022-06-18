using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace game
{
    public class TouchViewBehaviour : MonoBehaviour
    {
        public RectTransform arrowRotateTrans;
        public RectTransform ringSizeTrans;
        public RectTransform arrowSizeTrans;
        public GameObject view;
        public float arrowSizeFactor = 1f;
        public float ringSizeFactor = 2f;
        public float maxSize = 100;

        private void Start()
        {
            Hide();
        }

        public void Hide()
        {
            view.SetActive(false);
        }

        public void Show(Vector2 vec2)
        {
            if (!view.activeSelf)
                view.SetActive(true);

            float sizeFactor = Mathf.Min(vec2.magnitude, maxSize);
            float rotDeg = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;

            arrowRotateTrans.localEulerAngles = new Vector3(0, 0, rotDeg - 90);
            arrowSizeTrans.sizeDelta = new Vector2(1, arrowSizeFactor * sizeFactor);
            //ringSizeTrans.sizeDelta = new Vector2(ringSizeFactor * sizeFactor, ringSizeFactor * sizeFactor);
        }
    }
}