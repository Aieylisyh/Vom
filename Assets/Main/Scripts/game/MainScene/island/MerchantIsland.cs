using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class MerchantIsland : IslandBehaviour
    {
        public override void ClickFunction()
        {
            WindowService.instance.ShowMerchant();
        }
    }
}
