using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class   TownIsland: IslandBehaviour
    {
        public override void ClickFunction()
        {
            WindowService.instance.ShowTown();
        }
    }
}
