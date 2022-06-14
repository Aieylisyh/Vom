using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine;

namespace game
{
    public class MainSceneRftTimer : Ticker
    {
        private float timer;
        public RectTransform rect;
        public Transform target;
        public Text Timer;
        public Vector3 offset;
        public RectTransform parentRect;

        protected override void Update()
        {
            base.Update();

            if (GameFlowService.instance.windowState != GameFlowService.WindowState.Main)
            {
                this.gameObject.SetActive(false);
                return;
            }

            SetPos();
        }

        public void ForceTick()
        {
            Tick();
        }

        protected override void Tick()
        {
            if (!FishingService.instance.HasRft())
            {
                Timer.text = "";
                return;
            }

            var timerValue = FishingService.instance.GetRftRestTimeSpan();

            if (FishingService.instance.HasFinishedRft() || timerValue.Ticks <= 0)
            {
                Timer.text = "";
                return;
            }


            //Timer.text = TextFormat.GetRestTimeStringFormated(timerValue);
            Timer.text = timerValue.ToString(@"hh\:mm\:ss");
        }

        public void SetPos()
        {
            //注意这里的写法，也不需要什么canvas scale
            var pp = CameraControllerBehaviour.instance.portCam.WorldToScreenPoint(target.position);
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pp, CameraControllerBehaviour.instance.portCam, out pos);
            rect.anchoredPosition = (Vector3)pos + offset;
        }
    }
}