using UnityEngine;
using DG.Tweening;

namespace vom
{
    public class FloatingTextPanelBehaviour : MonoBehaviour
    {
        public FloatingTextBehaviour ftb;
        public FloatingTextBehaviour ftbSlow;
        public FloatingTextBehaviour ftbDmg;
        public FloatingTextBehaviour ftbHeal;

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

        public void CreateHealValue(string text, Transform target, Vector2 offset)
        {
            var ins = GameObject.Instantiate(ftbHeal, parent);
            ins.gameObject.SetActive(true);
            ins.SetText(text);
            ins.SetPos(target, offset);
            ins.StartMove();
            ins.rect.DOScale(1, 0.7f);
        }

        public void CreateDamageValue(string text, Transform target, Vector2 offset)
        {
            var ins = GameObject.Instantiate(ftbDmg, parent);
            ins.gameObject.SetActive(true);
            ins.SetText(text);
            ins.SetPos(target, offset);
            ins.StartMove();
            ins.rect.DOScale(1, 0.5f);
            ins.speed += Random.Range(-50, 0);
            if (Random.value > 0.5f)
            {
                ins.speedX = Random.Range(-150, -70);
            }
            else
            {
                ins.speedX = Random.Range(70, 150);
            }
        }
    }
}
