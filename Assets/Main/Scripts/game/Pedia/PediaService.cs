using com;
using UnityEngine;
using DG.Tweening;

namespace game
{
    public class PediaService : MonoBehaviour
    {
        private struct PediaView
        {
            public GameObject go;
            public string id;
        }

        public static PediaService instance { get; private set; }
        private PediaView _view;
        public GameObject bgLight;

        private void Awake()
        {
            instance = this;
        }

        public PediaPrototype GetPrototype(string id)
        {
            var cfg = ConfigService.instance.pediaConfig;
            foreach (var p in cfg.pedias)
            {
                if (p.id == id)
                    return p;
            }

            return null;
        }

        public void SetupPedia()
        {
            //Debug.Log("SetupPedia");
            WindowService.instance.ShowPediaPanelPopup();
            LevelService.instance.mmt.SetRandomSpeed();
            CombatService.instance.playerShip.OpenLights();
            bgLight.SetActive(true);
        }

        public void ExitPedia()
        {
            //Debug.Log("ExitPedia");
            WindowService.instance.HideAllWindows();
            var gameFlow = GameFlowService.instance;
            CameraControllerBehaviour.instance.EnterPort();
            LevelService.instance.ClearLevel();
            gameFlow.SetPausedState(GameFlowService.PausedState.Normal);
            gameFlow.SetInputState(GameFlowService.InputState.Forbidden);
            gameFlow.SetWindowState(GameFlowService.WindowState.Main);
            bgLight.SetActive(false);
            DeleteView();
        }

        public void DeleteView()
        {
            GameObject.Destroy(_view.go);
        }

        public void SetView(EnemyPrototype e)
        {
            var id = e.id;
            if (id == "Ghost")
                id = "ghost";//ghost is a projectile, while Ghost is the launcher with no view!

            if (_view.id == id)
                return;

            DeleteView();
            var cfg = ConfigService.instance.pediaConfig;
            var prefab = PoolingService.instance.GetInstantiateData(id).prefab;
            _view.go = Instantiate(prefab, cfg.enemyPos, Quaternion.Euler(0, 90, 0), prefab.transform.parent);
            //_view.go.transform.DOPunchRotation(new Vector3(0, 60, 0), 3f, 10, 1);
            //_view.go.transform.DOSpiral(5, Vector3.up, SpiralMode.Expand, 1, 2, 0);
            _view.go.transform.DOBlendableRotateBy(new Vector3(0, 360, 0), 8f, RotateMode.FastBeyond360);

            var comp1 = _view.go.GetComponents<UnitComponent>();
            var comp2 = _view.go.GetComponents<Unit>();
            foreach (var c in comp1)
                c.enabled = false;
            foreach (var c in comp2)
                c.enabled = false;
            _view.go.SetActive(true);
        }
    }
}
