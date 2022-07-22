using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.UI;

namespace vom
{
    public class InvSlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public Text num;

        ItemData _data;

        public void Sync(ItemData data = null)
        {
            _data = data;

            if (_data == null)
            {
                SetEmpty();
                return;
            }

            num.text = data.n + "";
            icon.sprite = ItemService.GetPrototype(_data.id).sp;
            icon.enabled = true;
        }

        public void SetEmpty()
        {
            num.text = "";
            icon.enabled = false;
        }

        public void OnClick()
        {
            InventorySystem.instance.OnClickInvItem(_data);
        }
    }
}
