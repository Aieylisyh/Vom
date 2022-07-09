using UnityEngine;

namespace game
{
    public class FloatingTextPanelBehaviour : MonoBehaviour
    {
        public FloatingTextBehaviour ftb;
        public FloatingTextBehaviour ftbSlow;
        public Transform parent;
        public static FloatingTextPanelBehaviour instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void Create(string text, float xRatio = 0.5f, float yRatio = 0.5f, bool isSlow = false)
        {
            var ins = GameObject.Instantiate(isSlow ? ftbSlow : ftb, parent);
            ins.gameObject.SetActive(true);
            ins.SetText(text);
            ins.SetPos(xRatio, yRatio);
            ins.StartMove();
        }

        public void Create(string text, Transform target, bool isSlow = false)
        {
            var ins = GameObject.Instantiate(isSlow ? ftbSlow : ftb, parent);
            ins.gameObject.SetActive(true);
            ins.SetText(text);
            ins.SetPos(target);
            ins.StartMove();
        }
    }

}
