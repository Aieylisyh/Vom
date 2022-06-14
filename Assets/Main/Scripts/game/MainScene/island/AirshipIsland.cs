using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class AirshipIsland : IslandBehaviour
    {
        public override void ClickFunction()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("AirshipTempTitle");
            data.content = LocalizationService.instance.GetLocalizedText("AirshipTempContent");
            data.bgCloseAction = () =>
            {
                MainHudBehaviour.instance.RefreshToDefault();
                CameraControllerBehaviour.instance.SetPortCamTarget(MainSceneManager.instance.islandCenter.position);
            };
            WindowService.instance.ShowConfirmBoxPopup(data);
        }
    }
}