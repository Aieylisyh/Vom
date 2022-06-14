using UnityEngine;
using com;

namespace game
{
    public class MerchantWindowBehaviour : WindowBehaviour
    {
        public WindowTabsBehaviour wtb;
        public WindowInventoryBehaviour invVipShop;
        public EventBoardBehaviour eventBoard;
        public WindowInventoryBehaviour invMerchantShop;

        public override void Setup()
        {
            base.Setup();

            wtb.SetTab(1);
            OnShowEventInfo();
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            MainHudBehaviour.instance.RefreshToDefault();
        }

        public override void Hide()
        {
            base.Hide();

            wtb.Off();
            invMerchantShop.ToggleCurrentDisplayingInstance(false);
            invVipShop.ToggleCurrentDisplayingInstance(false);
        }

        public void OnShowMerchantShop()
        {
            //Debug.Log("OnShowMerchantShop");
            MainHudBehaviour.instance.SetMode(MainHudBehaviour.MainHudMode.Tokens);

            invMerchantShop.Refresh();
            invMerchantShop.ToggleCurrentDisplayingInstance(true);
            invVipShop.ToggleCurrentDisplayingInstance(false);
        }

        public void OnShowEventInfo()
        {
            MainHudBehaviour.instance.SetMode(MainHudBehaviour.MainHudMode.Tokens);

            //Debug.Log("OnShowEventInfo");
            eventBoard.Refresh();
            invMerchantShop.ToggleCurrentDisplayingInstance(false);
            invVipShop.ToggleCurrentDisplayingInstance(false);
        }

        public void OnShowVipShop()
        {
            //Debug.Log("OnShowVipShop");
            MainHudBehaviour.instance.RefreshToDefault();

            invVipShop.Refresh();
            invMerchantShop.ToggleCurrentDisplayingInstance(false);
            invVipShop.ToggleCurrentDisplayingInstance(true);
        }
    }
}
