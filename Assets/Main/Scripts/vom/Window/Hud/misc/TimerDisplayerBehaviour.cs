using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace vom
{
    public class TimerDisplayerBehaviour : Ticker
    {
        public Text Timer;
        public TimerType timerType;

        public enum TimerType
        {
            None,
            ShopRefresh,
            EventChange,
            Rft,
            //vip please use VipTimerDisplayerBehaviour
        }

        protected override void Tick()
        {
            if (timerType == TimerType.ShopRefresh)
            {
                Timer.text = TextFormat.GetRestTimeStringFormated(UxService.instance.GetShopRefreshTimer());
            }
            else if (timerType == TimerType.EventChange)
            {
                var timerValue = UxService.instance.GetEventTimer(UxService.instance.GetEventCount() + 1);
                Timer.text = TextFormat.GetRestTimeStringFormated(timerValue);
            }
        }
    }
}
