using UnityEngine;
using System.Collections.Generic;
using com;
using game;

namespace vom
{
    public class VomDemoUiSystem : MonoBehaviour
    {
        public GameObject testTarget_standard;
        public PlayerBehaviour player;

        public GameObject orbPrefab;
        public Transform spawnSpace;



        private void Start()
        {
            GameFlowService.instance.SetInputState(GameFlowService.InputState.Allow);
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Gameplay);

            ConfirmBoxPopup.ConfirmBoxData data = new ConfirmBoxPopup.ConfirmBoxData();
            data.title = "What to Test";
            data.content = "Fall\n screen combat feeling";
            data.btnClose = true;
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickFireBalls()
        {
            PlayerBehaviour.instance.attack.AddFireBalls();
        }

        public void OnClickIceBalls()
        {
            PlayerBehaviour.instance.attack.AddIceBalls();
        }

        public void OnClickPoisonBalls()
        {
            PlayerBehaviour.instance.attack.AddPoisonBalls();
        }
    }
}