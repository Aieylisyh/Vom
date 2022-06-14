using UnityEngine;
using UnityEngine.UI;

namespace game
{
    public class MapNodePath : MonoBehaviour
    {
        public MapNodeData node;
        public Image iconImage;
        private RectTransform _rect;

        public void Setup(Vector2 from, Vector2 to, float indexFactor, float curveFactor)
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();

            Vector2 dir = to - from;
            Vector2 pend = Vector2.Perpendicular(dir);
            pend *= curveFactor;
            var res = Vector2.Lerp(from, to, indexFactor);
            res += pend * Mathf.Lerp(1, 0, Mathf.Pow(2f * Mathf.Abs(indexFactor - 0.5f), 2));
            _rect.anchoredPosition = res;

            gameObject.SetActive(true);
        }

        public void SetPassed(bool passed)
        {
            gameObject.SetActive(passed);
        }

        public void SetFaded(bool faded)
        {
            iconImage.color = faded ? new Color(0.45f, 0.7f, 0.88f) : Color.white;
        }

        public void SetDisplayed(bool b)
        {
            gameObject.SetActive(b);
        }
    }
}