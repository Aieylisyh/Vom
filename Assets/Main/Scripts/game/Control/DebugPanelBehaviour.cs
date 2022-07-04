using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using com;
using vom;

namespace game
{
    public class DebugPanelBehaviour : MonoBehaviour
    {
        public GameObject[] eneBtns;
        public GameObject[] uiBtns;
        public GameObject[] combatBtns;
        public GameObject[] dataBtns;
        public GameObject[] toggleBtns;

        public static DebugPanelBehaviour instance;

        private void Awake()
        {
            instance = this;
            //gameObject.SetActive(false);
        }

        private void Update()
        {
            //CheckTorView();
        }

        void CheckTorView()
        {
            if (Input.GetKey(KeyCode.B))
            {
                Transform target = null;
                foreach (var e in CombatService.instance.units)
                {
                    if (e.IsAlive() && e.GetComponent<Torpedo>() != null)
                    {
                        if (e.collision.selfFlag == UnitCollision.CollisionFlag.PlayerProjectile)
                        {
                            target = e.transform.GetChild(0);
                        }
                    }
                }
                CameraControllerBehaviour.instance.SwitchToCinematicView(target);
            }

            if (Input.GetKey(KeyCode.V))
            {
                CameraControllerBehaviour.instance.SwitchToCinematicView(null);
            }
        }

        public void AddHour()
        {
            AddTime(-3600);
            sound();
        }

        public void AddDay()
        {
            AddTime(-86400);
            sound();
        }

        public void TogglePlayerFastMode()
        {
            var pm = PlayerBehaviour.instance.move;
            if (pm.speed < 5)
            {
                pm.speed = 15;
            }
            else
            {
                pm.speed = 4.4f;
            }
        }
        public void AddTime(float sec)
        {
            var d = UxService.instance.accountDataCache.cache.firstLaunchDate;
            UxService.instance.accountDataCache.cache.firstLaunchDate = d.AddSeconds(sec);
            UxService.instance.SaveAccountData();
            sound();
        }

        public void ToggleFastGame()
        {
            var state = GameFlowService.instance.pausedState;
            var targetState = GameFlowService.PausedState.Speed2;
            if (state == GameFlowService.PausedState.Speed2)
            {
                targetState = GameFlowService.PausedState.Normal;
            }

            GameFlowService.instance.SetPausedState(targetState);
            sound();
        }

        public void ChangeShip()
        {
            CombatService.instance.playerShip.shipModelSwitcher.SetModel(Random.Range(0, 3));
            sound();
        }

        public void PortView()
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main
                || GameFlowService.instance.gameFlowEvent == GameFlowService.GameFlowEvent.GoToPort)
            {
                return;
            }

            CameraControllerBehaviour.instance.EnterPort();
            LevelService.instance.ClearLevel();
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Main);

            //MainSceneManager.Instance.AppearIslands(false);
            sound();
        }

        public void CombatView()
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Gameplay
                   || GameFlowService.instance.gameFlowEvent == GameFlowService.GameFlowEvent.GoToCombat)
            {
                return;
            }

            LevelService.instance.SetupNextCompaignLevel();
            LevelService.instance.ClearLevel();
            CameraControllerBehaviour.instance.LeavePort();

            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Gameplay);

            //MainSceneManager.Instance.DisappearIslands(false);
            sound();
        }

        public void AppearIslands()
        {
            MainSceneManager.instance.AppearIslands(false);
            sound();
        }

        public void DisappearIslands()
        {
            MainSceneManager.instance.DisappearIslands(false);
            sound();
        }

        private void sound()
        {
            SoundService.instance.Play("tap");
        }

        public void TestEffect()
        {
            var effectId = "exp small";
            Debug.Log(effectId);
            var go = PoolingService.instance.GetInstance(effectId);
            go.transform.position = CombatService.instance.playerShip.transform.position + Vector3.down * 4;
        }

        public void PlayMusic()
        {
            MusicService.instance.TurnOffMusic(MusicService.Musics.MenuMusicVolume);
            MusicService.instance.TurnOffMusic(MusicService.Musics.BossMusicVolume);
            MusicService.instance.TurnOffMusic(MusicService.Musics.CombatMusicVolume);

            MusicService.instance.TurnOnMusic(MusicService.Musics.BossMusicVolume);
        }

        public GameObject water;
        public GameObject islands;

        public void ToggleWater()
        {
            water.SetActive(!water.activeSelf);
        }

        public void ToggleIslands()
        {
            islands.SetActive(!islands.activeSelf);
        }

        public void ToggleEne()
        {
            foreach (var b in eneBtns)
            {
                b.SetActive(!b.activeSelf);
            }
        }
        public GameObject debugBtnPanel;

        public void ToggleDebug()
        {
            debugBtnPanel.SetActive(!debugBtnPanel.activeSelf);
        }

        public void ToggleUi()
        {
            foreach (var b in uiBtns)
            {
                b.SetActive(!b.activeSelf);
            }
        }
        public void ToggleCombat()
        {
            foreach (var b in combatBtns)
            {
                b.SetActive(!b.activeSelf);
            }
        }
        public void ToggleData()
        {
            foreach (var b in dataBtns)
            {
                b.SetActive(!b.activeSelf);
            }
        }
        public void ToggleToggle()
        {
            foreach (var b in toggleBtns)
            {
                b.SetActive(!b.activeSelf);
            }
        }

        public void AddCurrency()
        {
            UxService.instance.AddItem("Diamond", 980);
            sound();
        }

        public void AddItems()
        {
            UxService.instance.AddItem("Vip1", 1);
            UxService.instance.AddItem("Vip3", 1);
            UxService.instance.AddItem("Vip10", 1);
            UxService.instance.AddItem("Course1", 1);
            UxService.instance.AddItem("Course2", 1);
            UxService.instance.AddItem("Course3", 1);
            UxService.instance.AddItem("Course4", 1);
            UxService.instance.AddItem("Course5", 1);
            UxService.instance.AddItem("Fish1", 2);
            UxService.instance.AddItem("Fish2", 3);
            UxService.instance.AddItem("Fish3", 2);
            UxService.instance.AddItem("Fish4", 2);
            UxService.instance.AddItem("Fish5", 12);
            UxService.instance.AddItem("Fish6", 12);


            UxService.instance.AddItem("Token1", 99);
            UxService.instance.AddItem("Token2", 99);
            UxService.instance.AddItem("Token3", 99);
            UxService.instance.AddItem("Token4", 99);

            UxService.instance.AddItem("Amethyst", 11);
            UxService.instance.AddItem("Ruby", 11);
            UxService.instance.AddItem("Emerald", 11);
            UxService.instance.AddItem("Metal", 666);
            UxService.instance.AddItem("VolcanicRock", 666);
            UxService.instance.AddItem("TurtleRock", 666);

            UxService.instance.AddItem("Salary", 1);
            UxService.instance.AddItem("Clover", 1);
            UxService.instance.AddItem("Compass", 1);
            UxService.instance.AddItem("Note", 1);

            UxService.instance.AddItem("KingOfWeed_1", 4);
            UxService.instance.AddItem("KingOfWeed_2", 4);
            UxService.instance.AddItem("KingOfWeed_3", 4);
            UxService.instance.AddItem("KingOfWeed_4", 4);
            UxService.instance.AddItem("KingOfWeed_5", 4);
            UxService.instance.AddItem("KingOfWeed_6", 4);
            UxService.instance.AddItem("KingOfWeed_0", 4);

            UxService.instance.AddItem("KingOfWeed", 4);
            UxService.instance.AddItem("FlagGold", 8);
            UxService.instance.AddItem("FlagExp", 8);

            UxService.instance.AddItem("Crown", 3);
            UxService.instance.AddItem("Purse", 100);
            UxService.instance.AddItem("Ring", 15);
            UxService.instance.AddItem("ChestDiamond", 22);
            UxService.instance.AddItem("ChestGold", 33);
            UxService.instance.AddItem("Recipe_Medal_ring", 1);
            UxService.instance.AddItem("Recipe_Medal_common", 1);
            UxService.instance.AddItem("Recipe_Medal_fish", 1);
            UxService.instance.AddItem("Recipe_Medal_precious", 1);
            UxService.instance.AddItem("Medal", 1);
            UxService.instance.AddItem("Gold", 4000);
            UxService.instance.AddItem("Diamond", 20);

            UxService.instance.AddItem("ExtraCab", 1);

            sound();
        }

        public void TestLocalize()
        {
            string test = LocalizationService.instance.GetLocalizedTextFormatted("testLvjuren", 3, 8, "Google");
            Debug.Log(test);
        }

        public void ChangeLanguage()
        {
            if (LocalizationService.instance.currentLanguage == LocalizationService.Language.en_US)
            {
                LocalizationService.instance.currentLanguage = LocalizationService.Language.ZHS;
            }
            else
            {
                LocalizationService.instance.currentLanguage = LocalizationService.Language.en_US;
            }
        }

        public void ClearSave()
        {
            Debug.Log("----ClearSave！");
            SaveLoadService.instance.storageService.Clear();
        }

        public void CabSelect()
        {
            LevelService.instance.StartCombatAbilitySelection();
        }

        public void PassAllLevel()
        {
            foreach (var cpLevel in ConfigService.instance.levelConfig.campaignLevels)
            {
                var item = LevelService.instance.GetLevelItem(cpLevel.id);
                item.saveData.highStar = 3;
                item.saveData.highScore = 100;
            }

            UxService.instance.SaveGameData();
        }

        public void TestSave()
        {
            Debug.Log("----start test storage");
            string key = "test";
            if (SaveLoadService.instance.storageService.HasKey(key))
            {
                Debug.Log("HasKey " + key);
                string a = SaveLoadService.instance.storageService.GetString("test");
                Debug.Log(a);
                a += "1";
                SaveLoadService.instance.storageService.SetString(key, a);
                SaveLoadService.instance.storageService.SaveImmiediate();
            }
            else
            {
                Debug.Log("Has no Key " + key);
                SaveLoadService.instance.storageService.SetString(key, "0");
                SaveLoadService.instance.storageService.Save();
            }
        }

        public void TestAllocate()
        {
            var p = new List<int>();
            p.Add(1);
            p.Add(2);
            p.Add(3);
            p.Add(4);

            ListUtil.FastRandomAllocate(4, p);
            ListUtil.FastRandomAllocate(10, p);
            ListUtil.FastRandomAllocate(100, p);
            ListUtil.FastRandomAllocate(1000, p);
            ListUtil.FastRandomAllocate(2000, p);
            ListUtil.FastRandomAllocate(5000, p);
            ListUtil.FastRandomAllocate(10000, p);
            ListUtil.FastRandomAllocate(50000, p);
            ListUtil.FastRandomAllocate(100000, p);
        }

        public InterstitialAdExample itadTester;
        public RewardedAdsButton vadTester;
        //public BannerAdExample bannerAd;

        public void LoadItAd()
        {
            itadTester.LoadAd();
        }

        public void ShowItAd()
        {
            itadTester.ShowAd();
        }

        public void LoadVad()
        {
            vadTester.LoadAd();
        }

        public void IapScene1()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public void IapScene2()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }

        public void IapScene3()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }


        public GameObject IapPanel;

        public void ToggleIapPanel()
        {
            IapPanel.SetActive(!IapPanel.activeSelf);
        }

        public void InitIapManuelly()
        {
            PayService.instance.InitBuilder();
        }
    }
}