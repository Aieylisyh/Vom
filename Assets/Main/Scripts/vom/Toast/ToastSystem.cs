using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class ToastSystem : MonoBehaviour
    {
        public static ToastSystem instance { get; private set; }

        public List<ToastBehaviour> toasts;
        public Transform toastsParent;

        private void Awake()
        {
            instance = this;
        }

        public void Add(string s, int amount = 0)
        {
            var data = new ToastData();
            data.title = s;
            data.itemCount = amount;
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