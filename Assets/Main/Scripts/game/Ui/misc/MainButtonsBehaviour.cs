using UnityEngine;

namespace game
{
    public class MainButtonsBehaviour : MonoBehaviour
    {
        public static MainButtonsBehaviour instance;
        public GameObject mailAlert;

        public GameObject mailBtn;
        public GameObject settingsBtn;
        public GameObject rankBtn;
        public GameObject invBtn;
        public GameObject activityBtn;

        private void Awake()
        {
            instance = this;
        }

        public void Refresh()
        {
            CheckMailAlert();
            RefreshDisplay();
        }

        void RefreshDisplay()
        {
            //Debug.Log("RefreshDisplay");
            var li = LevelService.instance.GetNextCampaignLevelIndex();
            var cfg = ConfigService.instance.tutorialConfig.minLevelIndexEnableFunctionsData;
            //Debug.Log("li " + li);
            settingsBtn.SetActive(true);
            rankBtn.SetActive(li >= cfg.rank);
            invBtn.SetActive(li >= cfg.inv);
            mailBtn.SetActive(li >= cfg.mail);
            //activityBtn.SetActive(li >= cfg.activity);
        }

        public void HideMailAlert()
        {
            mailAlert.SetActive(false);
        }

        private void CheckMailAlert()
        {
            var c = GetUnreadMailCount();
            mailAlert.SetActive(c > 0);
        }

        public int GetUnreadMailCount()
        {
            int count = 0;
            MailService.instance.CheckAllSend();
            var mails = MailService.instance.FetchMailBox();

            foreach (var m in mails)
            {
                if (!m.saveData.hasRead)
                    count++;
            }

            return count;
        }
    }
}
