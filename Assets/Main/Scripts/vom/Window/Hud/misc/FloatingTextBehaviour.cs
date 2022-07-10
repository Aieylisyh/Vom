using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

namespace vom
{
    public class FloatingTextBehaviour : MonoBehaviour
    {
        public float speed;
        public float durationMove;
        public float durationDestory;
        public Text txt;
        private float timer;
        public float canvasScale = 1;
        public Camera cam;
        public RectTransform rect;
        public float speedX;

        public void SetText(string s)
        {
            txt.text = s;
        }

        public void StartMove()
        {
            timer = 0;
        }

        public void SetPos(Transform trans, Vector2 offset)
        {
            var r = (float)Screen.width / (float)Screen.height;
            canvasScale = (float)Screen.width / 720;
            var pos = rect.anchoredPosition;
            //Debug.Log("SetPos");
            pos = com.Convert2DAnd3D.GetScreenPosition(cam, trans.position, canvasScale);
            //Debug.Log(pos);
            rect.anchoredPosition = pos + offset;
        }

        public void SetPos(Transform trans)
        {
            SetPos(trans, Vector2.zero);
        }

        public void SetPos(float xRatio = 0.5f, float yRatio = 0.5f)
        {
            canvasScale = (float)Screen.width / 720;
            //float r = ((float)Screen.width / (float)Screen.height) / (720f / 1280f);
            var pos = new Vector3(0, 0, 0);

            //Debug.Log("SetPos");
            pos.x = Screen.width * xRatio / canvasScale;
            pos.y = Screen.height * yRatio / canvasScale;
            //Debug.Log(pos);
            rect.anchoredPosition = pos;
        }

        void Update()
        {
            if (timer > durationDestory)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            timer += Time.deltaTime;
            if (timer > durationMove)
                return;

            var pos = rect.anchoredPosition;
            var y = pos.y;
            y += speed * Time.deltaTime;
            if (speedX != 0)
            {
                var x = pos.x;
                x += speedX * Time.deltaTime;
                pos.x = x;
            }

            pos.y = y;
            rect.anchoredPosition = pos;
        }
    }
}
