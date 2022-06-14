using UnityEngine;
using DG.Tweening;
using com;

namespace game
{
    public class MapWindowBehaviour : WindowBehaviour
    {
        public MapNodeInfoPanelBehaviour nodeInfo;

        public MapPanelBehaviour map;

        public static MapWindowBehaviour instance;

        private bool _generated;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Setup()
        {
            base.Setup();
            Generate();

            ShowMap();
        }

        void Generate()
        {
            if (!_generated)
            {
                _generated = true;
                map.GenerateMap();
            }

            map.RefreshMap();
            map.ScrollMap();
        }

        public void ShowMap()
        {
            map.gameObject.SetActive(true);
            nodeInfo.gameObject.SetActive(false);
            map.ShowParticles();
        }

        void ShowInfoPanel()
        {
            map.gameObject.SetActive(false);
            nodeInfo.gameObject.SetActive(true);
            map.HideParticles();
        }

        public void OnSelectNode(string id)
        {
            ShowInfoPanel();
            nodeInfo.Setup(id);
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            MainHudBehaviour.instance.Show();
        }

        public override void Hide()
        {
            base.Hide();
            map.HideParticles();
        }
    }
}
