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
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Gameplay);

            TransitionBehaviour.instance.Opening(() =>
            {
                // ConfirmBoxPopup.ConfirmBoxData data = new ConfirmBoxPopup.ConfirmBoxData();
                // data.title = "What to Test";
                // data.content = "new bie\nVillage";
                // data.btnClose = false;
                // data.btnBgClose = true;
                // WindowService.instance.ShowConfirmBoxPopup(data);
            });
        }
    }
}