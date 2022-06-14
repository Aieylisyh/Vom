using UnityEngine;
using com;

namespace game
{
    public class TownWindowBehaviour : WindowBehaviour
    {
        public WindowTabsBehaviour wtb;
        public WindowInventoryBehaviour invShop;
        //public LoreBoardBehaviour loreBoard;
        public WindowInventoryBehaviour invShopMat;
        public WindowInventoryBehaviour invPurchase;

        private bool sessionViewed_shop;
        private bool sessionViewed_mat;
        private bool sessionViewed_purchase;
        private int _sessionOpenCount = 0;

        private void Start()
        {
            sessionViewed_shop = false;
            sessionViewed_mat = false;
            sessionViewed_purchase = false;
            _sessionOpenCount = 0;
        }

        public override void Setup()
        {
            base.Setup();

            _sessionOpenCount++;

            MainHudBehaviour.instance.RefreshToDefault();
            if (_sessionOpenCount == 2 || _sessionOpenCount == 8)
            {
                wtb.SetTab(2);
                OnShowPurchase();
            }
            else if (_sessionOpenCount == 5)
            {
                wtb.SetTab(1);
                OnShowShpMat();
            }
            else
            {
                wtb.SetTab(0);
                OnShowShop();
            }
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            MainHudBehaviour.instance.Show();
        }

        public override void Hide()
        {
            base.Hide();

            wtb.Off();
            invPurchase.ToggleCurrentDisplayingInstance(false);
            invShop.ToggleCurrentDisplayingInstance(false);
            invShopMat.ToggleCurrentDisplayingInstance(false);
        }

        public void OnShowShop()
        {
            invShop.Refresh();
            invPurchase.ToggleCurrentDisplayingInstance(false);
            invShop.ToggleCurrentDisplayingInstance(true);
            invShopMat.ToggleCurrentDisplayingInstance(false);

            sessionViewed_shop = true;
            SetDlMissionViewTown();
        }

        public void OnShowShpMat()
        {
            invShopMat.Refresh();
            invPurchase.ToggleCurrentDisplayingInstance(false);
            invShop.ToggleCurrentDisplayingInstance(false);
            invShopMat.ToggleCurrentDisplayingInstance(true);

            sessionViewed_mat = true;
            SetDlMissionViewTown();
        }

        public void OnShowPurchase()
        {
            invPurchase.Refresh();
            invPurchase.ToggleCurrentDisplayingInstance(true);
            invShop.ToggleCurrentDisplayingInstance(false);
            invShopMat.ToggleCurrentDisplayingInstance(false);

            sessionViewed_purchase = true;
            SetDlMissionViewTown();
        }

        void SetDlMissionViewTown()
        {
            int quota = (sessionViewed_shop ? 1 : 0) + (sessionViewed_mat ? 1 : 0) + (sessionViewed_purchase ? 1 : 0);
            MissionService.instance.PushDl("town", quota, false);
        }
    }
}
