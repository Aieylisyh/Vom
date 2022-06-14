using game;
using UnityEngine;

namespace com
{
    public class StartGameService : MonoBehaviour
    {
        public static StartGameService instance;

        public SpotLightBehaviour portlight;

        void Awake()
        {
            instance = this;
        }

        //checkUpdate/assetbundle
        void Start()
        {

        }

        //included in LoginFinishStartGame
        public void OnReEnterPort()
        {
            //Debug.Log("OnReEnterPort");
            //Debug.Log(GameFlowService.instance.windowState);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Main);
            CombatService.instance.playerShip.shipModelSwitcher.islandBehaviour.SetOutlineThin();
            MainSceneManager.instance.SetFogColor();
            WindowService.instance.OnPortView();
            CombatAbilityService.instance.RefreshAndReset();
            FishingService.instance.TryShowBoat();
            SharkIsland.instance.TryShowShark();
            PortalIsland.instance.TryShowPortal();
            BirdIsland.instance.TryShowBird();
            MissionService.instance.SyncMissions();
            StatuesBehaviour.instance.TryShowStatues();
            LevelService.instance.CheckLevelPassedMission();
            ShipService.instance.CheckShipUpgradeMissions();
            GameFlowService.instance.SetPausedState(GameFlowService.PausedState.Normal);
            //Debug.Log(GameFlowService.instance.windowState);
        }

        //once only
        public void LoginFinishStartGame()
        {
            //Debug.Log("LoginFinishStartGame");
            var data = UxService.instance.gameDataCache;
            var itemData = UxService.instance.gameItemDataCache;

            AdService.instance.Setup();

            PayService.instance.InitBuilder();

            MailService.instance.CheckDefaultMails();

            CombatService.instance.playerShip.shipModelSwitcher.SetModel(data.cache.currentShipId);

            UxService.instance.CheckShopCache();

            MainHudBehaviour.instance.Refresh();

            CameraControllerBehaviour.instance.OnStartGame();

            FishingBoatBehaviour.instance.mainSceneRftTimer.ForceTick();

            portlight.StartLights();
        }
    }
}