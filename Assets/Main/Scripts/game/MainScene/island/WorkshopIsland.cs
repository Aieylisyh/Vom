using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class WorkshopIsland : IslandBehaviour
    {
        public override void ClickFunction()
        {
            WindowService.instance.ShowWorkshop();
        }
    }
}
