using System;
using com;

namespace vom
{
    [System.Serializable]
    public class MailItem
    {
        public string id;
        public MailSaveData saveData;

        public MailItem(MailData data)
        {
            id = "m" + data.GetHashCode();
            //UnityEngine.Debug.Log("new MailItem " + id);
            saveData = new MailSaveData(data);
        }

        public bool CanClaim()
        {
            if (saveData.hasClaimed)
                return false;

            if (saveData.mailData.rewards == null || saveData.mailData.rewards.Count < 1)
            {
                return false;
            }
            return true;
        }

        public string GetClaimText()
        {
            if (saveData.mailData.rewards != null || saveData.mailData.rewards.Count > 0)
            {
                return TextFormat.GetItemText(saveData.mailData.rewards, true);
            }

            return "";
        }

        [System.Serializable]
        public class MailSaveData
        {
            public MailData mailData;
            public bool hasSent;
            public bool hasRead;
            public bool hasClaimed;
            public bool hasTrashed;

            public MailSaveData(MailData data)
            {
                mailData = data;
                hasSent = false;
                hasRead = false;
                hasClaimed = false;
                hasTrashed = false;
            }
        }
    }
}
