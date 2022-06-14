using UnityEngine.UI;
using UnityEngine;
namespace com
{
    public class CanvasAdapter : MonoBehaviour
    {
        public CanvasScaler cs;
        public RectTransform rect;
        public RectTransform rectCanvas;
        public bool dirty;
        public int perferredWidth;
        public int perferredHeight;
        private float canvasScale
        {
            get
            {
                return rectCanvas.localScale.x;
            }
        }
        // Use this for initialization
        void Start()
        {
            dirty = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (dirty)
            {
                dirty = false;
                Adapt();
            }
        }

        void Adapt()
        {
            float w = Screen.width;
            float h = Screen.height;
            //Debug.Log("w " + w);
            //Debug.Log("h " + h);

            float ratio = w / h;
            float perferredRatio = (float)perferredWidth / perferredHeight;
            //Debug.Log("ratio " + ratio);
            //Debug.Log("perferredRatio " + perferredRatio);

            if (ratio <= perferredRatio)
            {
                //Debug.Log("narrow screen");
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = Vector2.zero;
                //rect.offsetMax = new Vector2(0, 0);
                // rect.offsetMin = new Vector2(0, 0);
                return;
            }

            //Debug.Log("wide screen");
            float perferW = ratio * perferredHeight;
            cs.referenceResolution = new Vector2(perferW, (float)perferredHeight);

            //Debug.Log("perferW " + perferW);
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 0.5f);
            var rectSize = rect.sizeDelta;
            rectSize.x = (float)perferredWidth;
            rect.sizeDelta = rectSize;
        }
    }

}
