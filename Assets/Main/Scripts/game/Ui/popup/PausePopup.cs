using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class PausePopup : PopupBehaviour
    {
        public void Setup()
        {
            SoundService.instance.Play("tap");
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Paused);
        }

        public virtual void OnClickBtnResume()
        {
            Hide();
            Sound();
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
        }

        public virtual void OnClickBtnHtp()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnBgClose = true;
            data.btnRight = false;
            data.btnLeft = false;
            data.title = LocalizationService.instance.GetLocalizedText("HtpTitle");
            data.content = LocalizationService.instance.GetLocalizedText("HtpContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public virtual void OnClickBtnLeave()
        {
            Sound();

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("leaveTitle");
            data.content = LocalizationService.instance.GetLocalizedText("leaveContent");
            data.btnLeft = true;
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("leave");
            data.btnLeftAction = () =>
            {
                Hide();
                Sound();
                WindowService.instance.ShowRoundEnd(false);
                //var gameFlow = GameFlowService.instance;
                //if (gameFlow.windowState == GameFlowService.WindowState.Main || gameFlow.gameFlowEvent == GameFlowService.GameFlowEvent.GoToPort)
                //{
                //    return;
                //}
                //
                //CameraController.instance.EnterPort();
                //LevelService.instance.ClearLevel();
                //gameFlow.SetInputState(GameFlowService.InputState.Forbidden);
                //gameFlow.SetWindowState(GameFlowService.WindowState.Main);

            };
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickBtnBuffs()
        {
            Sound();
            WindowService.instance.ShowCombatAbilitiesPopup(false);
        }

        public void OnClickBtnLoots()
        {
            Sound();
            ItemsPopup.ItemsPopupData data = new ItemsPopup.ItemsPopupData();
            data.clickBgClose = true;
            data.hasBtnOk = false;
            data.title = LocalizationService.instance.GetLocalizedText("LootsTitle");
            // var exp = LevelService.instance.runtimeLevel.exp;
            //data.content = LocalizationService.instance.GetLocalizedTextFormatted("LootsContent", exp);
            // data.content = LocalizationService.instance.GetLocalizedText("LootsContent");
            var score = LevelService.instance.runtimeLevel.score;
            data.content = LocalizationService.instance.GetLocalizedTextFormatted("LootsContent", score);
            data.items = LevelService.instance.runtimeLevel.totalLoot;

            WindowService.instance.ShowItemsPopup(data);
        }
    }
}