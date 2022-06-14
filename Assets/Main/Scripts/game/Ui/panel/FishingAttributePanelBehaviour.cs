using System.Collections.Generic;
using UnityEngine;
using System;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class FishingAttributePanelBehaviour : MonoBehaviour
    {
        public Text attri_amount;
        public Text attri_duration;
        public Text attri_ot;
        public Text attri_otef;

        public Text btnLevelupPrice;
        public GameObject btnLevelup;
        public GameObject maxLevelLabel;

        public GameObject particleGo;

        public void Refresh()
        {
            MainHudBehaviour.instance.SetMode(MainHudBehaviour.MainHudMode.ExpGoldDiamond);

            FishingWindowBehaviour.instance.ShowBoatLevel();
            AssignAttributes();
            btnLevelup.SetActive(false);
            maxLevelLabel.SetActive(false);
            FishingBoatBehaviour.instance.UpdatePartViews();
            FishingBoatBehaviour.instance.StopPartViews();

            if (FishingService.instance.IsLevelupPossible())
            {
                FishingBoatBehaviour.instance.ShowPartViewLevelupTarget();
                btnLevelup.SetActive(true);
                var price = FishingService.instance.GetLevelupPrice();
                btnLevelupPrice.text = TextFormat.GetItemText(price, true, true);
            }
            else
            {
                btnLevelup.SetActive(false);
                maxLevelLabel.SetActive(true);
            }
        }

        private string GetAttriLocalizedLabel(string prefixCode)
        {
            return LocalizationService.instance.GetLocalizedText(prefixCode);
        }

        private string GetAttriText(string prefixCode, float value1, float value2, int multiplier = 1)
        {
            var valueDelta = value2 - value1;
            int v1 = (int)value1;
            int v2 = (int)valueDelta;
            var baseValueString = (v1 * multiplier) + "";
            var res = GetAttriText(prefixCode, baseValueString);

            if (v2 != 0)
            {
                res += "<color=#AAFFAA> ";
                if (v2 > 0)
                    res += "+";

                res += (int)(v2 * multiplier) + "</color>";
            }
            return res;
        }

        private string GetAttriText(string prefixCode, string value)
        {
            var valueWithColor = "<color=#FFF0C0>" + value + "</color>";
            var res = LocalizationService.instance.GetLocalizedTextFormatted(prefixCode, valueWithColor);
            return res;
        }

        private void AssignAttributes()
        {
            var rftDurationTicks = FishingService.instance.GetRftDuration_TimeSpanTicks();
            var overtimeMaxTicks = FishingService.instance.GetOvertimeMax_TimeSpanTicks();
            var overtimeEfficiencyPercent = FishingService.instance.GetOvertimeEfficiencyPercent();

            var durationStimeSpan = TimeSpan.FromTicks(rftDurationTicks);
            var rftDurationString = durationStimeSpan.ToString(@"hh\:mm\:ss");
            attri_duration.text = GetAttriText("RftAttriDuration", rftDurationString);
            attri_amount.text = GetAttriText("RftAttriAmount", FishingService.instance.GetRftAmountEstimationString());

            if (overtimeMaxTicks > 0 && overtimeEfficiencyPercent > 0)
            {
                var overtimeMaxString = TimeSpan.FromTicks(overtimeMaxTicks).ToString(@"hh\:mm\:ss");
                attri_ot.text = GetAttriText("RftAttriOtDuration", overtimeMaxString);
                var overtimeEfficiencyPercentString = overtimeEfficiencyPercent.ToString();
                attri_otef.text = GetAttriText("RftAttriOtEff", overtimeEfficiencyPercentString);
            }
            else
            {
                attri_ot.text = "";
                attri_otef.text = "";
            }
        }

        public void OnClickLevelup()
        {
            if (FishingService.instance.IsLevelupPossible())
            {
                WindowService.instance.ShowConfirmUpgradeBoatPopup();
            }
            else
            {
                Debug.Log("OnClickLevelup IsLevelupPossible false");
            }
        }
    }
}