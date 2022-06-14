using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class SharkIsland : IslandBehaviour
    {
        public GameObject shark;
        public static SharkIsland instance;
        public string correspondShipAbilityId = "Puck_ab_2";
        private void Start()
        {
            instance = this;
        }

        public void TryShowShark()
        {
            //Debug.Log("TryShowShark");
            var has = ShipService.instance.HasAnyShipUnlockedAbility(correspondShipAbilityId);
            //Debug.Log("has " + has);
            if (!has)
            {
                shark.SetActive(false);
                return;
            }

            var last = UxService.instance.gameDataCache.cache.rawLastPlayedDays_shark;
            var today = UxService.instance.GetRawPlayedDays();
            //Debug.Log("last " + last);
            //Debug.Log("today " + today);
            if (last == today&& shark.activeSelf)
            {
                shark.SetActive(false);
                return;
                //这个可以避免通过从一个有鲨鱼的账号切换到一个已经领取过今日鲨鱼的账号，刷鲨鱼
                //但是可能造成反复进出港口把自己的鲨鱼刷掉的情况，但是可能性不高且符合文字描述的偶尔出现鲨鱼
                //因此没有必要在存档功能中额外增加鲨鱼是否领取的变量
            }
            if (last != today)
            {
                shark.SetActive(has);
            }
        }

        public override void ClickFunction()
        {
            //Debug.Log("shark ClickFunction");
            shark.SetActive(false);
            List<Item> items = new List<Item>();

            if (Random.value < 0.5f)
                items.Add(new Item(Random.Range(4, 6), "Purse"));
            else
                items.Add(new Item(1, "ChestDiamond"));

            items.Add(new Item(Random.Range(1, 3), "Diamond"));
            items.Add(new Item(Random.Range(1, 2), "ChestGold"));

            ItemService.instance.GiveReward(items, false);

            shark.SetActive(false);

            var today = UxService.instance.GetRawPlayedDays();
            UxService.instance.gameDataCache.cache.rawLastPlayedDays_shark = today;
            UxService.instance.SaveGameData();
        }
    }
}
