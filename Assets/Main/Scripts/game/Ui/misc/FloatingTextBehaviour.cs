using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

namespace game
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

        public void SetText(string s)
        {
            txt.text = s;
        }

        public void StartMove()
        {
            timer = 0;
        }

        public void SetPos(Transform trans)
        {
            var r = (float)Screen.width / (float)Screen.height;
            canvasScale = (float)Screen.width / 720;
            var pos = rect.anchoredPosition;
            //Debug.Log("SetPos");
            pos = com.Convert2DAnd3D.GetScreenPosition(cam, trans.position, canvasScale);
            //Debug.Log(pos);
            rect.anchoredPosition = pos;
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
            timer += Time.deltaTime;
            if (timer > durationDestory)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            if (timer > durationMove)
            {
                return;
            }

            var pos = rect.anchoredPosition;
            var y = pos.y;
            y += speed * Time.deltaTime;
            pos.y = y;
            rect.anchoredPosition = pos;
        }
    }

}
