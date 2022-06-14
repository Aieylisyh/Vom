using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ResizeRectTransform : MonoBehaviour
    {
        public RectTransform target;
        public RectTransform reference;
        public Text referenceOverrideText;
        public bool resizeHeight;
        public bool resizeWidth;
        public float heightOffset;
        public float widthOffset;
        public bool toResize = false;
        private bool _toResize2 = false;

        public void Update()
        {
            if (toResize)
            {
                toResize = false;
                Resize();
            }

            if (_toResize2)
            {
                toResize = true;
                _toResize2 = false;
            }
        }

        public void ResizeLater()
        {
            toResize = true;
            _toResize2 = true;
        }

        void Resize()
        {
            //Debug.Log("size");
            var size = target.sizeDelta;
            //Debug.Log(size);
            var sizeRef = reference.sizeDelta;
            //Debug.Log(sizeRef);
            if (referenceOverrideText != null)
            {
                //refText.autoSizeTextContainer = true;
                sizeRef.x = referenceOverrideText.renderedWidth;
                sizeRef.y = referenceOverrideText.renderedHeight;
               // Debug.Log(sizeRef);
            }

            if (resizeWidth)
            {
                sizeRef.x = Mathf.Max(0, sizeRef.x);
                size.x = sizeRef.x + widthOffset;
            }

            if (resizeHeight)
            {
                sizeRef.y = Mathf.Max(0, sizeRef.y);
                size.y = sizeRef.y + heightOffset;
            }
            //Debug.Log(size);
            target.sizeDelta = size;
        }
    }
}