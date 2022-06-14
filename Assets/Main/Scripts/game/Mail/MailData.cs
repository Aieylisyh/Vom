using com;
using System.Collections.Generic;
using System;

namespace game
{
    //titleKey contentKey don't contain dynamic text
    //mail titleKey & contentKey use localized key! if the user change language, the showing texts will also change!
    //titleLocalized & contentLocalized are localized, if isLocalized==true, use them;
    //each mail creates a new MailData
    [System.Serializable]
    public class MailData
    {
        public string titleKey;
        public string contentKey;
        // public string tltleLclz;
        //public bool isTitleLclz;
        public List<Item> rewards;
        public long date;
        public DateTime sendDate
        {
            get
            {
                return new DateTime(date);
            }
            set
            {
                date = value.Ticks;
            }
        }

        public void SetSendNow()
        {
            sendDate = DateTime.Now;
        }

        public void SetSendAfterDays(int days)
        {
            DateTime date = DateTime.Now;
            date = date.AddDays(days);
            sendDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public void SetSendMonthDayFromNowOn(int month, int day)
        {
            var now = DateTime.Now;
            var year = now.Year;
            if (month < now.Month || (month == now.Month && day < now.Day))
            {
                year += 1;
            }

            sendDate = new DateTime(year, month, day, 0, 0, 0);
        }

        public void SetTexts(string pTitle, string pContent)
        {
            titleKey = pTitle;
            contentKey = pContent;
        }

        public string GetTitle()
        {
            //if (isTitleLclz)
            //return tltleLclz;
            return LocalizationService.instance.GetLocalizedText(titleKey);
        }

        public string GetContent()
        {
            return LocalizationService.instance.GetLocalizedText(contentKey);
        }

        public bool ShouldSendNow()
        {
            var res = DateTime.Now.CompareTo(sendDate);//>=0 is ok, is DateTime.Now>=sendDate
            //UnityEngine.Debug.Log("ShouldSendNow " + res);
            //UnityEngine.Debug.Log(DateTime.Now + " - " + sendDate);
            return res >= 0;
        }
    }
}