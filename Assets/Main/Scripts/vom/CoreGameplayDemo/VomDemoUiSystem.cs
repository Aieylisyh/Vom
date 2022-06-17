using UnityEngine;
using System.Collections.Generic;
using com;

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
        }

        public void OnClickFireBalls()
        {
            //charge 3 fire balls
            PlayerBehaviour.instance.attack.AddFireBalls();
        }
    }
}