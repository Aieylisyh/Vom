using UnityEngine;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class DisplaySlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public Text label;
        private Item _item;
        public ScaleBehaviour scaleBehaviour;
        public GameObject checkmark;

        public float imageSize = 100;

        public void Setup(Item param, bool isChecked)
        {
            _item = param;
            if (param == null)
            {
                SetEmpty();
                return;
            }

            ItemPrototype itemProto = ItemService.instance.GetPrototype(_item.id);
            icon.enabled = true;
            icon.sprite = itemProto.sp;
            label.text =LocalizationService.instance.GetLocalizedText(itemProto.title);

            checkmark.SetActive(isChecked);
            if (isChecked)
            {
                scaleBehaviour.Play();
            }
            else
            {
                scaleBehaviour.Stop();
            }
        }

        public void SetEmpty()
        {
            _item = null;
            icon.enabled = false;
            label.text = "";
        }

        public void Awake()
        {
            icon.rectTransform.sizeDelta = new Vector2(imageSize, imageSize);
            SetEmpty();
        }

        public void OnClick()
        {
            if (_item != null)
            {
                var data = new ItemPopup.ItemPopupData();
                data.enableUse = false;
                data.item = _item;
                WindowService.instance.ShowItemPopup(data);
                SoundService.instance.Play("tap");
            }
        }
    }
}
