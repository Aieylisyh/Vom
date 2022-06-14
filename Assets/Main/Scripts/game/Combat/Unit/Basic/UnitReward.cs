using UnityEngine;
using System.Collections.Generic;
namespace game
{
    public class UnitReward : UnitComponent
    {
        List<Item> DropLootTable;
        List<Item> RoundEndLootTable;

        public int Score;
    }
}
