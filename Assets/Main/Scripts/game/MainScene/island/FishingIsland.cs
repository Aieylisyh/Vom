using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class FishingIsland : IslandBehaviour
    {
        public override void ClickFunction()
        {
            WindowService.instance.ShowFishing();
        }
    }
}
