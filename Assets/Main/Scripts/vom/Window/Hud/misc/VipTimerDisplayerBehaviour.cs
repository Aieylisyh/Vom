using UnityEngine;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace vom
{
    public class VipTimerDisplayerBehaviour : TimerDisplayerBehaviour
    {
        public GameObject timeEnd;
        public GameObject timer;

        protected override void Tick()
        {
            var rest = UxService.instance.GetRestTimeVip();
            if (rest.Ticks <= 0)
            {
                timer.SetActive(false);
                timeEnd.SetActive(true);
            }
            else
            {
                timer.SetActive(true);
                timeEnd.SetActive(false);
                Timer.text = TextFormat.GetRestTimeStringFormated(UxService.instance.GetRestTimeVip(), true);
            }
        }
    }
}
