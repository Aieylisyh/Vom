using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class MailService : MonoBehaviour
    {
        public static MailService instance { get; private set; }

        public string customMailAddress = "ladimelaxi@gmail.com";

        private void Awake()
        {
            instance = this;
        }

        public List<MailItem> FetchMailBoxSorted()
        {
            List<MailItem> mails = FetchMailBox();
            mails.Sort((MailItem a, MailItem b) =>
            {
                var scoreA = 0;
                var scoreB = 0;
                if (a.saveData.hasClaimed)
                    scoreA -= 10;
                if (b.saveData.hasClaimed)
                    scoreB -= 10;
                if (a.saveData.hasRead)
                    scoreA -= 10;
                if (b.saveData.hasRead)
                    scoreB -= 10;
                return scoreB - scoreA;
            });

            return mails;
        }

        public List<MailItem> FetchMailBox()
        {
            var res = new List<MailItem>();
            var mails = FetchMails();
            foreach (var mail in mails)
            {
                if (mail.saveData.hasSent && !mail.saveData.hasTrashed)
                {
                    res.Add(mail);
                }
            }
            return res;
        }

        public List<MailItem> FetchMails()
        {
            return UxService.instance.gameItemDataCache.cache.mails;
        }

        public void Add(MailData data)
        {
            MailItem item = new MailItem(data);
            UxService.instance.gameItemDataCache.cache.mails.Add(item);
            UxService.instance.SaveGameItemData();
        }

        public bool CheckExist(MailData data)
        {
            return CheckExist("m" + data.GetHashCode());
        }

        public bool CheckExist(string id)
        {
            var mails = FetchMails();
            foreach (var mail in mails)
            {
                if (mail.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public void CheckAllSend()
        {
            var mails = FetchMails();
            foreach (var mail in mails)
            {
                if (!mail.saveData.hasSent)
                {
                    if (mail.saveData.mailData.ShouldSendNow())
                    {
                        mail.saveData.hasSent = true;
                        UxService.instance.SaveGameItemData();
                    }
                }
            }
        }

        public void AddMailLevelPassed(string LevelTitleKey, List<Item> rewards)
        {
            MailData res = new MailData();
            res.SetTexts(LevelTitleKey, "MailLevelPassedContent");
            res.SetSendNow();
            res.rewards = rewards;
            Add(res);
        }

        public void CheckDefaultMails()
        {
            var mails = FetchMails();
            if (UxService.instance.GetPlayedHours() > 1)
            {
                return;
            }
            if (mails.Count > 0)
            {
                //Debug.LogError("AddDefaultMails dup");
                return;
            }

            AddWelcomeMail();
            AddCreditMail();
            AddWeek1Mail();
            AddWeek2Mail();
            AddDay1Mail();
            AddDay2Mail();
            AddDay3Mail();

            AddHolidayMail(1, 1);
            AddHolidayMail(1, 15);
            AddHolidayMail(2, 1);
            AddHolidayMail(2, 14);// valentine
            AddHolidayMail(3, 1);
            AddHolidayMail(3, 17);
            AddHolidayMail(4, 1);
            AddHolidayMail(4, 15);
            AddHolidayMail(5, 1);
            AddHolidayMail(5, 15);
            AddHolidayMail(6, 1);
            AddHolidayMail(6, 15);
            AddHolidayMail(7, 1);
            AddHolidayMail(7, 15);
            AddHolidayMail(8, 1);
            AddHolidayMail(8, 15);
            AddHolidayMail(9, 1);
            AddHolidayMail(9, 15);
            AddHolidayMail(10, 1);
            AddHolidayMail(10, 15);
            AddHolidayMail(10, 30);
            AddHolidayMail(11, 1);
            AddHolidayMail(11, 15);
            AddHolidayMail(12, 1);
            AddHolidayMail(12, 24);//cristmas
            AddHolidayMail(12, 25);
        }

        private void AddWelcomeMail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailWelcomeTitle", "MailWelcomeContent");
            mail.SetSendNow();
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(100, "Gold"));
            //mail.rewards.Add(new Item(1, "Diamond"));
            Add(mail);
        }

        private void AddCreditMail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailCreditTitle", "MailCreditContent");
            mail.SetSendNow();
            mail.rewards = new List<Item>();
            //mail.rewards.Add(new Item(300, "Gold"));
            mail.rewards.Add(new Item(1, "Emerald"));
            //mail.rewards.Add(new Item(10, "TurtleRock"));
            mail.rewards.Add(new Item(1, "Diamond"));
            Add(mail);
        }

        private void AddWeek1Mail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailWeek1Title", "MailWeek1Content");
            mail.SetSendAfterDays(7);
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(1, "Fish6"));
            Add(mail);
        }

        private void AddWeek2Mail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailWeek2Title", "MailWeek2Content");
            mail.SetSendAfterDays(14);
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(1, "Fish6"));
            //mail.rewards.Add(new Item(1, "Fish1"));
            Add(mail);
        }

        private void AddDay1Mail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailDay1Title", "MailDay1Content");
            mail.SetSendAfterDays(1);
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(1, "Emerald"));
            mail.rewards.Add(new Item(3000, "Gold"));
            mail.rewards.Add(new Item(1, "Diamond"));
            Add(mail);
        }
        private void AddDay2Mail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailDay2Title", "MailDay2Content");
            mail.SetSendAfterDays(2);
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(5, "Diamond"));
            mail.rewards.Add(new Item(5000, "Gold"));
            Add(mail);
        }
        private void AddDay3Mail()
        {
            MailData mail = new MailData();
            mail.SetTexts("MailDay3Title", "MailDay3Content");
            mail.SetSendAfterDays(3);
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(1, "Fish6"));
            //mail.rewards.Add(new Item(9, "Fish5"));
            Add(mail);
        }

        private void AddHolidayMail(int month, int day)
        {
            MailData mail = new MailData();
            mail.SetTexts("MailHolidayTitle", "MailHolidayContent");
            mail.SetSendMonthDayFromNowOn(month, day);
            mail.rewards = new List<Item>();
            mail.rewards.Add(new Item(7, "ChestGold"));
            Add(mail);
        }
    }
}