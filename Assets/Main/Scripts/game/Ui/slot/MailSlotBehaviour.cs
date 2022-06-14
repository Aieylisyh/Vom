using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MailSlotBehaviour : MonoBehaviour
    {
        public MailItem item;
        public GameObject view;

        public Text title;
        public GameObject tagNew;

        public void Refresh()
        {
            if (item == null)
            {
                view.SetActive(false);
                return;
            }

            view.SetActive(true);
            title.text = item.saveData.mailData.GetTitle();
            tagNew.SetActive(!item.saveData.hasRead);
        }

        public void OnClick()
        {
            MailBoxPopup.instance.OnMailClicked(this);
        }
    }
}