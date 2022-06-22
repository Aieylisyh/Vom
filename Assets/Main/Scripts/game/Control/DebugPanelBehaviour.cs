using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using com;

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

        static string enemyName;
        string GetEnemyName()
        {
            if (enemyName == "Normal")
            {
                enemyName = "Fast";
            }
            else if (enemyName == "Fast")
            {
                enemyName = "Weak";
            }
            else if (enemyName == "Weak")
            {
                enemyName = "Common";
            }
            else if (enemyName == "Common")
            {
                enemyName = "Stealth";
            }
            else if (enemyName == "Stealth")
            {
                enemyName = "Lurker";
            }
            else if (enemyName == "Lurker")
            {
                enemyName = "Veteran";
            }
            else if (enemyName == "Veteran")
            {
                enemyName = "Marshal";
            }
            else if (enemyName == "Marshal")
            {
                enemyName = "Defender";
            }
            else if (enemyName == "Defender")
            {
                enemyName = "Angel";
            }
            else if (enemyName == "Angel")
            {
                enemyName = "Tiny";
            }
            else if (enemyName == "Tiny")
            {
                enemyName = "Bonus";
            }
            else if (enemyName == "Bonus")
            {
                enemyName = "Phantom";
            }
            else if (enemyName == "Phantom")
            {
                enemyName = "Laser";
            }
            else if (enemyName == "Laser")
            {
                enemyName = "Shield";
            }
            else if (enemyName == "Shield")
            {
                enemyName = "Summoner";
            }
            else if (enemyName == "Summoner")
            {
                enemyName = "Ghost";
            }
            else
            {
                enemyName = "Normal";
            }

            return enemyName;
        }

        void AddSubmarine(string enemyId, float height, bool right)
        {
            var enemyGo = PoolingService.instance.GetInstance(enemyId);
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            enemy.Init(right, height, enemyId);
        }

        public void AddSubmarine1()
        {
            bool right = true;
            AddSubmarine(GetEnemyName(), 4f, right);
            //AddSubmarine(GetEnemyName(), 6f, right);
            //AddSubmarine(GetEnemyName(), 8f, right);
            //AddSubmarine(GetEnemyName(), 10f, right);
            AddSubmarine(GetEnemyName(), 12f, right);
            sound();
        }

        public void AddSubmarine2()
        {
            bool right = false;
            AddSubmarine(GetEnemyName(), 4f, right);
            // AddSubmarine(GetEnemyName(), 6f, right);
            // AddSubmarine(GetEnemyName(), 8f, right);
            // AddSubmarine(GetEnemyName(), 10f, right);
            AddSubmarine(GetEnemyName(), 12f, right);
            sound();
        }
        public void AddSubmarinesRandom()
        {
            var e = GetEnemyName();
            AddSubmarine(e, Random.Range(4f, 12f), Random.value < 0.5f);
            AddSubmarine(e, Random.Range(4f, 12f), Random.value < 0.5f);
            AddSubmarine(e, Random.Range(4f, 12f), Random.value < 0.5f);
            sound();
        }

        public void AddSubmarine(string name)
        {
            AddSubmarine(name, Random.Range(4f, 12f), Random.value < 0.5f);
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
            //UxService.instance.AddItem("Gold", 123456);
            //UxService.instance.AddItem("Diamond", 1080);
            ////UxService.instance.AddItem("Exp", Mathf.FloorToInt(UxService.instance.GetExpTop() * 0.33f));
            //UxService.instance.AddItem("Amethyst", 5);
            //UxService.instance.AddItem("Ruby", 5);
            //UxService.instance.AddItem("Emerald", 5);
            //UxService.instance.AddItem("Metal", 1111);
            //UxService.instance.AddItem("VolcanicRock", 1111);
            //UxService.instance.AddItem("TurtleRock", 1111);
            //UxService.instance.AddItem("Medal", 3);
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
            /*  foreach (var i in ConfigService.instance.enemyConfig.list)
              {
                  if (i.hp==0)
                  {
                      i.hp = i.enemyModel.hp;
                      i.attack = i.enemyModel.attack;
                      i.speed = i.enemyModel.speed;
                      i.dropData = i.enemyModel.dropData;
                      i.Category = i.enemyModel.Category;
                      i.levelupData = i.enemyModel.levelupData;
                      i.title = i.enemyModel.title;
                  }
              }*/
            /*
         var ic = ConfigService.instance.itemConfig;
         foreach (var i in ConfigService.instance.enemyConfig.list)
         {
             foreach (var v in i.dropData.dropItems)
             {
                 if (v.item.amount != 0)
                 {
                     Debug.Log("change n amount " + v.item.n + " "  + v.item.amount);
                     v.item.n = v.item.amount;
                 }

             }
         }
         foreach (var i in ConfigService.instance.factoryConfig.ships)
         {
             foreach (var v in i.abilityUnlocks)
             {
                 foreach (var k in v.price)
                 {
                     if (k.amount != 0)
                     {
                         Debug.Log("change n amount " +k.n + " " + k.amount);
                         k.n = k.amount;
                     }
                 }
             }
         }
         foreach (var i in ic.commodityList)
         {
             if (i.itemOutPut.amount != 0)
             {
                 i.itemOutPut.n = i.itemOutPut.amount;
             }

             foreach (var v in i.itemValue)
             {
                 var k = v;
                 if (k.amount != 0)
                 {
                     Debug.Log("change n amount " + k.n + " " + k.amount);
                     k.n = k.amount;
                 }
             }
         }
         foreach (var i in ic.list)
         {
             foreach (var v in i.itemValue)
             {
                 var k = v;
                 if (k.amount != 0)
                 {
                     Debug.Log("change n amount " + k.n + " " + k.amount);
                     k.n = k.amount;
                 }
             }
         }
         foreach (var i in ic.complexItemList)
         {
             foreach (var v in i.list)
             {
                 var k = v;
                 if (k.amount != 0)
                 {
                     Debug.Log("change n amount " + k.n + " " + k.amount);
                     k.n = k.amount;
                 }
             }
         }*/
        }

        public void ShowFloatText()
        {
            FloatingTextPanelBehaviour.instance.Create("playerShip", CombatService.instance.playerShip.transform);
            FloatingTextPanelBehaviour.instance.Create("M", 0.5f, 0.5f);
            FloatingTextPanelBehaviour.instance.Create("BL", 0.1f, 0.1f);
            FloatingTextPanelBehaviour.instance.Create("BR", 0.9f, 0.1f);
            FloatingTextPanelBehaviour.instance.Create("TR", 0.9f, 0.9f);
            FloatingTextPanelBehaviour.instance.Create("TL", 0.1f, 0.9f);
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