using UnityEngine;
using DG.Tweening;
using com;
using System.Collections.Generic;

namespace game
{
    public class LoginWindowBehaviour : WindowBehaviour
    {
        public RectTransform logo;
        public RectTransform panelSlots;
        public ParticleSystem ps;
        public List<LoginSlotBehaviour> lsbs;
        public static LoginWindowBehaviour instance;
        public GameObject tip;
        bool _showStartAnime;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Setup()
        {
            base.Setup();

            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.None);
            WindowService.instance.HideMainButtons();
            WindowService.instance.HideMissions();
            panelSlots.localScale = Vector3.zero;
            _showStartAnime = true;
        }

        void Update()
        {
            if (_showStartAnime)
            {
                _showStartAnime = false;
                ps.Play(true);
            
                logo.localScale = Vector3.one * 0.35f;
                logo.gameObject.SetActive(true);
                logo.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    ShowPanel();
                    logo.DOShakePosition(2.7f, 15, 10);
                });
            }
        }

        public void ShowPanel()
        {
            tip.SetActive(false);
            for (int i = 0; i < lsbs.Count; i++)
            {
                var lsb = lsbs[i];
                lsb.Hide();
            }
            panelSlots.localScale = Vector3.zero;
            panelSlots.DOScale(1, 0.35f).SetEase(Ease.OutCubic).OnComplete(ShowSlots);
        }

        private void ShowSlots()
        {
            tip.SetActive(true);
            var ux = UxService.instance;
            int e = 0;
            for (int i = 0; i < lsbs.Count; i++)
            {
                //Debug.Log("ShowSlots " + i);
                var lsb = lsbs[i];
                var data = ux.accountsDataCache[i];
                lsb.Setup(data, ux.gamesDataCache[i], i + 1);

                if (data.cache == null)
                {
                    if (e > 0)
                    {
                        lsb.Hide();
                    }
                    else
                    {
                        e++;
                        lsb.Refresh();
                        lsb.DoResetAnime();
                    }
                }
                else
                {
                    lsb.Refresh();
                    lsb.DoResetAnime();
                }
            }
        }

        public void OnClickSlot(int index)
        {
            Sound();
            ps.Stop(true);
            WaitingCircleBehaviour.instance.SetHideAction(() => { OnSelected(index); });
            WaitingCircleBehaviour.instance.Show(1f);
        }

        private void OnSelected(int index)
        {
            UxService.instance.SelectAccount(index);
            GameFlowService.instance.SetWindowState(GameFlowService.WindowState.Main);
            Hide();
        }
    }
}