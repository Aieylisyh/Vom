using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using com;
using System;

namespace game
{
    public class ConfirmUpgradeBoatPopup : PopupBehaviour
    {
        public Text priceText;
        public Text attriText;
        public Text attriValueText;

        public void Setup()
        {
            var price = FishingService.instance.GetLevelupPrice();
            priceText.text = TextFormat.GetItemText(price, true, true);
            DisplayAttributes();
        }

        public void OnClickBtnOk()
        {
            Sound();
            Hide();

            var suc = Levelup();
        }

        private bool Levelup()
        {
            var res = FishingService.instance.IsLevelupAffordable(true);
            if (res)
            {
                FishingService.instance.LevelupBoat();
                SoundService.instance.Play("btn big");
                FishingWindowBehaviour.instance.OnShipLeveluped();
                return true;
            }

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");
            data.title = LocalizationService.instance.GetLocalizedText("LevelupNonAffordableTitle");
            data.content = LocalizationService.instance.GetLocalizedText("LevelupNonAffordableContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
            return false;
        }

        private string GetAttriLocalizedLabel(string prefixCode)
        {
            return LocalizationService.instance.GetLocalizedText(prefixCode);
        }

        private string GetAttriText(string prefixCode, float value1, float value2, bool addReturn = true, string color1 = "BBBBBB", string color2 = "FFFF99")
        {
            int v1 = (int)value1;
            int v2 = (int)value2;
            if (v1 == v2)
                return "";

            var res = "<size=85%> " + GetAttriLocalizedLabel(prefixCode) + "</size>\t\t<color=#" + color1 + ">";
            res += v1 + "</color>";
            res += "-> <color=#" + color2 + "><size=110%> ";
            res += (int)v2 + "</size></color>";

            if (addReturn)
                res += "\n";

            return res;
        }

        private void SetAttriText(out string titleText, out string valueText, string prefixCode, float value1, float value2, bool addReturn = true, string color1 = "BBBBBB", string color2 = "FFFF99")
        {
            titleText = "";
            valueText = "";
            int v1 = (int)value1;
            int v2 = (int)value2;
            if (v1 == v2)
                return;

            titleText = GetAttriLocalizedLabel(prefixCode) + ":";
            valueText = " <color=#" + color1 + ">" + v1 + "</color> -> <color=#" + color2 + ">" + v2 + "</color>";

            if (addReturn)
            {
                titleText += "\n";
                valueText += "\n";
            }
        }

        private void SetAttriText(out string titleText, out string valueText, string prefixCode, string value1, string value2, bool addReturn = true, string color1 = "BBBBBB", string color2 = "FFFF99")
        {
            titleText = "";
            valueText = "";

            titleText = GetAttriLocalizedLabel(prefixCode) + ":";
            valueText = " <color=#" + color1 + ">" + value1 + "</color> -> <color=#" + color2 + ">" + value2 + "</color>";

            if (addReturn)
            {
                titleText += "\n";
                valueText += "\n";
            }
        }

        private void DisplayAttributes()
        {
            var item = FishingService.instance.GetItem();
            var boatLevel = FishingService.instance.GetBoatLevel();

            var rftDurationTicks1 = FishingService.instance.GetRftDuration_TimeSpanTicks();
            var rftAmount1 = FishingService.instance.GetRftAmountRaw();
            var durationStimeSpan1 = TimeSpan.FromTicks(rftDurationTicks1);
            var rftDurationString1 = durationStimeSpan1.ToString(@"hh\:mm\:ss");

            item.saveData.boatLevel = item.saveData.boatLevel + 1;

            var rftDurationTicks2 = FishingService.instance.GetRftDuration_TimeSpanTicks();
            var rftAmount2 = FishingService.instance.GetRftAmountRaw();
            var durationStimeSpan2 = TimeSpan.FromTicks(rftDurationTicks2);
            var rftDurationString2 = durationStimeSpan2.ToString(@"hh\:mm\:ss");

            item.saveData.boatLevel = boatLevel;

            string attri_lv_title, attri_amount_title, attri_duration_title;
            string attri_lv_value, attri_amount_value, attri_duration_value;

            SetAttriText(out attri_lv_title, out attri_lv_value, "BoatLv", boatLevel, boatLevel + 1, true, "C0FFD0", "77FFAA");
            SetAttriText(out attri_amount_title, out attri_amount_value, "RftAmount", rftAmount1, rftAmount2, true, "C0C0C0", "CCFFAA");
            SetAttriText(out attri_duration_title, out attri_duration_value, "RftDuration", rftDurationString1, rftDurationString2, true, "C0C0C0", "FFCCAA");
            attriText.text = attri_lv_title + "\n" + attri_duration_title + attri_amount_title;
            attriValueText.text = attri_lv_value + "\n" + attri_duration_value + attri_amount_value;
        }
    }
}