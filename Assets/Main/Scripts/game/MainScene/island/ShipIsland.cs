using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class ShipIsland : IslandBehaviour
    {
        public override void ClickFunction()
        {
            WindowService.instance.ShowShip();
        }
    }
}
