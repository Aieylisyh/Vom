using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class HpBarSystem : MonoBehaviour
    {
        //public GameObject prefab;
        public static HpBarSystem instance { get; private set; }

        public string prefabId = "hpbar";

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
        }

        public HpBarBehaviour Create(Transform host, float offset = 150f, float scale = 1f)
        {
            var newHpBarGo = PoolingService.instance.GetInstance(prefabId);
            newHpBarGo.transform.parent = host;
            newHpBarGo.transform.localPosition = Vector3.zero;
            newHpBarGo.SetActive(true);

            var newHpBar = newHpBarGo.GetComponent<HpBarBehaviour>();
            //newHpBar.host = host;
            var ap = newHpBar.rectTrans.anchoredPosition;
            ap.y = offset;
            newHpBar.rectTrans.anchoredPosition = ap;
            newHpBar.rectTrans.localScale = Vector3.one * scale;
            return newHpBar;
        }
    }
}