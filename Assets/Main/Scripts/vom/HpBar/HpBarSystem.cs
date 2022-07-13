using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class HpBarSystem : MonoBehaviour
    {
        public static HpBarSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
        }

        public HpBarBehaviour Create(Transform host, float offset = 150f, float scale = 1f)
        {
            var newHpBar = Instantiate(CombatSystem.instance.hpBarPrefab);
            newHpBar.gameObject.SetActive(true);
            newHpBar.transform.SetParent(host);
            newHpBar.transform.localPosition = Vector3.zero;

            var ap = newHpBar.rectTrans.anchoredPosition;
            ap.y = offset;
            newHpBar.rectTrans.anchoredPosition = ap;
            newHpBar.rectTrans.localScale = Vector3.one * scale;
            return newHpBar;
        }
    }
}