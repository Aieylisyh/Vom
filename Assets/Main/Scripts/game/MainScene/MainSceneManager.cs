using com;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

namespace game
{
    public class MainSceneManager : MonoBehaviour, IGameFlow
    {
        public static MainSceneManager instance { get; private set; }
        public int timesSetFogColor = 0;
        public Transform islandCenter;
        public AnimationCurve acAppear;

        public GlobalFog gf;

        public GameObject playIsland;
        public GameObject merchantIsland;
        public GameObject shipIsland;
        public GameObject townIsland;
        public GameObject workshopIsland;
        public GameObject fishingIsland;
        public GameObject airshipIsland;

        public bool testHasNewVersion;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            SetFogColor();
        }

        public void OnPausedState(GameFlowService.PausedState state)
        {
            //throw new System.NotImplementedException();
        }

        public void OnWindowState(GameFlowService.WindowState state)
        {
            if (state == GameFlowService.WindowState.Main)
            {
                //Refresh();
            }
        }

        public void Refresh()
        {
            FadeAllOutline();
            //build islands
            RefreshIslands();
        }

        void RefreshIslands()
        {
            //Debug.Log("RefreshIslands");
            var levelPassIndex = LevelService.instance.GetNextCampaignLevelIndex();
            var cfg = ConfigService.instance.tutorialConfig;
            //Debug.Log("li " + li);
            playIsland.SetActive(true);

            merchantIsland.SetActive(levelPassIndex >= cfg.minLevelIndexEnableFunctionsData.merchant);

            shipIsland.SetActive(true);

            townIsland.SetActive(levelPassIndex >= cfg.minLevelIndexEnableFunctionsData.town);

            workshopIsland.SetActive(levelPassIndex >= cfg.minLevelIndexEnableFunctionsData.workshop);

            fishingIsland.SetActive(levelPassIndex >= cfg.minLevelIndexEnableFunctionsData.fishing);

            bool hasNewVersion = false;
            hasNewVersion = testHasNewVersion|| (levelPassIndex >= cfg.minLevelIndexEnableFunctionsData.airship);
            airshipIsland.SetActive(hasNewVersion);
        }

        void FadeAllOutline()
        {
            foreach (var island in IslandBehaviour.islands)
            {
                island?.SetOutlineThin();
            }
        }

        public void SetFogColor()
        {
            var e = GlobalFog.FogColorType.Default;
            timesSetFogColor++;
            if (timesSetFogColor > 2)
            {
                int v = timesSetFogColor % 5;
                if (v == 0)
                {
                    e = GlobalFog.FogColorType.Danger;
                }
                else if (v == 1)
                {
                    e = GlobalFog.FogColorType.Dark;
                }
                else if (v == 2)
                {
                    e = GlobalFog.FogColorType.Dawn;
                }
                else if (v == 3)
                {
                    e = GlobalFog.FogColorType.Fine;
                }
                else
                {
                    // e = GlobalFog.FogColorType.Default;
                }
            }
            gf.SetColor(e);
        }

        private IslandBehaviour GetPointerIsland(PointerEventData eventData)
        {
            RaycastHit raycastHit;
            Ray ray = CameraControllerBehaviour.instance.portCam.ScreenPointToRay(eventData.position);  //Check for mouse click  touch?
            //Debug.Log("eventData.position! ");
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    //Debug.Log("transform! ");
                    var go = raycastHit.transform.gameObject;
                    //Debug.Log("CurrentClickedGameObject " + go + " " + go.tag);
                    foreach (var island in IslandBehaviour.islands)
                    {
                        if (go.transform.parent == island.transform)
                        {
                            return island;
                        }
                    }
                }
            }
            return null;
        }

        public void InputPanelClick(PointerEventData eventData)
        {
            if (GameFlowService.instance.windowState != GameFlowService.WindowState.Main)
            {
                return;
            }

            var island = GetPointerIsland(eventData);
            if (island != null)
            {
                island.OnClicked();
                return;
            }

            AppearIslands();
            CameraControllerBehaviour.instance.SetPortCamTarget(islandCenter.position);
        }

        public void InputPanelRelease(PointerEventData eventData)
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main)
            {
                FadeAllOutline();
            }
        }

        public void InputPanelDown(PointerEventData eventData)
        {
            if (GameFlowService.instance.windowState != GameFlowService.WindowState.Main)
            {
                return;
            }
            var island = GetPointerIsland(eventData);
            if (island != null)
            {
                island.SetOutlineThick();
            }
        }

        public void AppearIslands(bool hardReset = false)
        {
            foreach (var island in IslandBehaviour.islands)
            {
                island.Appear(hardReset);
            }
        }

        public void DisappearIslands(bool hardReset = false)
        {
            foreach (var island in IslandBehaviour.islands)
            {
                island.Disappear(hardReset);
            }
        }

        public void DisappearOtherIslands(IslandBehaviour exception = null)
        {
            foreach (var island in IslandBehaviour.islands)
            {
                if (exception == island)
                {
                    continue;
                }

                island.Disappear(false);
            }
            exception.Appear(false);
        }
    }
}
