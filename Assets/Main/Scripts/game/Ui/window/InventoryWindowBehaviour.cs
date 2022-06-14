using UnityEngine;
using DG.Tweening;
using com;

namespace game
{
    public class InventoryWindowBehaviour : WindowBehaviour
    {
        public WindowInventoryBehaviour inv;

        public override void Setup()
        {
            base.Setup();
            inv.ToggleCurrentDisplayingInstance(true);
            inv.Refresh();
            //SoundService.instance.Play("scroll open");
            //MainHudBehaviour.instance.Hide();
        }

        public override void OnClickBtnClose()
        {
            //SoundService.instance.Play("scroll close");
            inv.ToggleCurrentDisplayingInstance(false);
            //MainHudBehaviour.instance.SetMode(MainHudBehaviour.MainHudMode.ExpGoldDiamond);
            //MainHudBehaviour.instance.Show();
            Sound();
            Hide();
        }
    }
}
