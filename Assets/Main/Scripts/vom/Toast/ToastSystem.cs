using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace vom
{
    public class ToastSystem : MonoBehaviour
    {
        public static ToastSystem instance { get; private set; }

        public List<ToastBehaviour> toasts;
        public Transform toastsParent;
        RectTransform _rect;

        private void Update()
        {
            foreach (var t in toasts)
            {
                if (t.IsExpanding())
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
                    break;
                }
            }
        }

        private void Awake()
        {
            instance = this;
            _rect = GetComponent<RectTransform>();
        }

        public void Add(string s, int amount = 0)
        {
            var data = new ToastData();
            data.title = s;
            data.itemCount = amount;
            data.bgColor = Color.white;
            Add(data);
        }

        public void Add(ItemData item)
        {
            var data = new ToastData();
            var itemPrototype = ItemService.GetPrototype(item.id);

            data.title = itemPrototype.title;
            data.bgColor = ItemService.GetColorByRarity(itemPrototype.rarity);
            data.sp = itemPrototype.sp;
            data.itemCount = item.n;
            Add(data);
        }

        public void Add(ToastData toastData)
        {
            var toast = GetAvailableToast();
            toast.Show(toastData);
            toast.transform.SetAsFirstSibling();
        }

        public ToastBehaviour GetAvailableToast()
        {
            foreach (var t in toasts)
            {
                if (!t.gameObject.activeSelf)
                    return t;
            }

            return toastsParent.GetChild(toastsParent.childCount - 1).GetComponent<ToastBehaviour>();
        }
    }
}