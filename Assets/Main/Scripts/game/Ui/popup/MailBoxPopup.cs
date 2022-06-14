using UnityEngine;
using UnityEngine.UI;
using com;
using System.Collections.Generic;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class MailBoxPopup : PopupBehaviour
    {
        public static MailBoxPopup instance { get; private set; }

        public Text title;
        public Text content;

        public GameObject btnDelete;
        public GameObject btnClaim;
        public Text btnClaimTxt;
        public List<MailSlotBehaviour> slots;
        private MailSlotBehaviour _crtMail;

        private void Awake()
        {
            instance = this;
        }

        public void Setup()
        {
            Refresh();
            MainHudBehaviour.instance.RefreshToDefault();
            MainButtonsBehaviour.instance.HideMailAlert();
        }

        private void Refresh()
        {
            //Debug.Log("LoadMails");
            _crtMail = null;
            ShowMail(null);
            MailService.instance.CheckAllSend();
            var mails = MailService.instance.FetchMailBoxSorted();

            int i = 0;
            foreach (var slot in slots)
            {
                MailItem mail = null;
                if (mails.Count > i)
                {
                    mail = mails[i];
                }
                slot.item = mail;
                slot.Refresh();
                i++;
            }
        }

        public void OnMailClicked(MailSlotBehaviour slot)
        {
            ShowMail(slot.item);
            _crtMail = slot;

            UxService.instance.SaveGameItemData();
            Sound();
            _crtMail.Refresh();
        }

        private void ShowMail(MailItem mail)
        {
            if (mail == null)
            {
                title.text = "";
                content.text = "";
                btnClaim.SetActive(false);
                btnDelete.SetActive(false);
                return;
            }

            mail.saveData.hasRead = true;
            title.text = mail.saveData.mailData.GetTitle();
            content.text = mail.saveData.mailData.GetContent();

            if (mail.CanClaim())
            {
                btnClaim.SetActive(true);
                btnClaimTxt.text = mail.GetClaimText();
                btnDelete.SetActive(false);
            }
            else
            {
                btnClaim.SetActive(false);
                btnDelete.SetActive(true);
            }
        }

        public virtual void OnClickBtnClaim()
        {
            //Debug.Log("OnClickBtnClaim");
            _crtMail.item.saveData.hasClaimed = true;
            ItemService.instance.GiveReward(_crtMail.item.saveData.mailData.rewards, false);

            UxService.instance.SaveGameItemData();
            _crtMail.Refresh();
            ShowMail(_crtMail.item);
            Sound();
        }

        public virtual void OnClickBtnDelete()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("MailTrashTitle");
            data.content = LocalizationService.instance.GetLocalizedText("MailTrashContent");
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            data.btnLeftAction = () =>
            {
                _crtMail.item.saveData.hasTrashed = true;
                UxService.instance.SaveGameItemData();
                Refresh();
                SoundService.instance.Play("btn small");
            };
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            MainHudBehaviour.instance.RefreshToDefault();
        }
    }
}