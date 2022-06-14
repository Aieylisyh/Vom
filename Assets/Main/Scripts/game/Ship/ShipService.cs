using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public class ShipService : MonoBehaviour
    {
        public static ShipService instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private List<ShipPrototype> GetPrototypes()
        {
            return ConfigService.instance.factoryConfig.ships;
        }

        public string currentShipId
        {
            get { return UxService.instance.gameDataCache.cache.currentShipId; }
        }

        public ShipItem GetShipItem(string id)
        {
            var ships = UxService.instance.gameDataCache.cache.shipItems;
            foreach (var i in ships)
            {
                if (i.id == id)
                {
                    return i;
                }
            }
            return null;
        }

        public List<ShipItem> GetShipItems()
        {
            return UxService.instance.gameDataCache.cache.shipItems;
        }

        public ShipItem GetShipItem()
        {
            return GetShipItem(currentShipId);
        }

        public ShipPrototype GetPrototype(string id)
        {
            foreach (var i in ConfigService.instance.factoryConfig.ships)
            {
                if (i.id == id)
                {
                    return i;
                }
            }
            return null;
        }

        public ShipPrototype GetPrototype()
        {
            return GetPrototype(currentShipId);
        }

        public bool IsLevelupPossible(string id)
        {
            return IsLevelupPossible(GetShipItem(id));
        }

        public bool IsLevelupPossible()
        {
            return IsLevelupPossible(currentShipId);
        }

        public bool IsLevelupPossible(ShipItem item)
        {
            var res = true;
            var proto = GetPrototype(item.id);
            if (item.saveData.level >= proto.levelMax)
            {
                res = false;
            }

            return res;
        }

        public bool IsLevelupAffordable(string id, bool afford)
        {
            return IsLevelupAffordable(GetShipItem(id), afford);
        }

        public bool IsLevelupAffordable(bool afford)
        {
            return IsLevelupAffordable(currentShipId, afford);
        }

        public bool IsLevelupAffordable(ShipItem item, bool afford)
        {
            var res = true;
            var affordableRes = ItemService.instance.IsPriceAffordable(GetLevelupPrice(item), afford);
            if (!affordableRes.success)
            {
                res = false;
            }

            return res;
        }

        public Item GetUnlockPrice(ShipItem item)
        {
            var proto = GetPrototype(item.id);
            return new Item(proto.price.n, proto.price.id);
        }

        public Item GetLevelupPrice(ShipItem item)
        {
            var proto = GetPrototype(item.id);
            var priceNum = proto.levelupGoldCost.GetIntValue(item.saveData.level);
            return new Item(priceNum, "Gold");
        }

        public bool IsShipAbilityLearnPossible(string id, ShipPrototype.ShipAbilityUnlockPrototype abu)
        {
            return IsShipAbilityLearnPossible(GetShipItem(id), abu);
        }

        public bool IsShipAbilityLearnPossible(ShipPrototype.ShipAbilityUnlockPrototype abu)
        {
            return IsShipAbilityLearnPossible(currentShipId, abu);
        }

        public bool IsShipAbilityLearnPossible(ShipItem item, ShipPrototype.ShipAbilityUnlockPrototype abu)
        {
            var res = false;
            var proto = GetPrototype(item.id);
            //if (abu == null) abu = GetAbilityUnlock(proto, GetLastUnlockedAbilityUnlockIndex(item));//must not prevent this case

            int levelReq = abu.shipLevelRequire;

            if (item.saveData.level >= levelReq)
                res = true;

            return res;
        }

        public int GetLastUnlockedAbilityUnlockIndex(ShipItem item)
        {
            Debug.LogError("should never use this");
            var proto = GetPrototype(item.id);
            if (item.saveData.unlockedAbilities.Count < proto.abilityUnlocks.Count)
            {
                //not all unlocked
                return item.saveData.unlockedAbilities.Count;
            }
            // all unlocked
            return -1;//not exist!
        }

        public ShipPrototype.ShipAbilityUnlockPrototype GetAbilityUnlockPrototype(ShipPrototype proto, int index)
        {
            if (index < 0)
            {
                return null;
            }

            return proto.abilityUnlocks[index];
        }

        public ShipPrototype.ShipAbilityUnlockPrototype GetAbilityUnlockPrototype(string id)
        {
            var protos = GetPrototypes();
            foreach (var p in protos)
            {
                foreach (var abu in p.abilityUnlocks)
                {
                    if (abu.ability.id == id)
                    {
                        return abu;
                    }
                }
            }
            return null;
        }

        public ShipAbilityPrototype GetAbilityPrototype(string id)
        {
            var protos = GetPrototypes();
            foreach (var p in protos)
            {
                foreach (var abu in p.abilityUnlocks)
                {
                    if (abu.ability.id == id)
                    {
                        return abu.ability;
                    }
                }
            }
            return null;
        }

        public bool IsShipAbilityLearnAffordable(ShipPrototype.ShipAbilityUnlockPrototype proto)
        {
            var res = true;
            var affordableRes = ItemService.instance.IsPriceAffordable(proto.price, false);
            if (!affordableRes.success)
            {
                res = false;
            }

            return res;
        }

        public bool IsShipUnlockAffordable(bool consume = false)
        {
            var shipProto = GetPrototype();
            var res = ItemService.instance.IsPriceAffordable(new Item(shipProto.price.n, shipProto.price.id), consume);

            return res.success;
        }

        public void LevelupShip()
        {
            var shipItem = GetShipItem();
            shipItem.saveData.level = shipItem.saveData.level + 1;
            UxService.instance.SaveGameData();
        }

        public void UnlockShip()
        {
            var shipItem = GetShipItem();
            shipItem.saveData.unlocked = true;
            UxService.instance.SaveGameData();
        }

        public void UnlockShipAbility(string s)
        {
            var shipItem = GetShipItem();
            shipItem.saveData.unlockedAbilities.Add(s);
            UxService.instance.SaveGameData();
        }

        public bool HasAnyShipUnlockedAbility(string abilityId)
        {
            var ships = UxService.instance.gameDataCache.cache.shipItems;
            foreach (var i in ships)
            {
                if (i.HasUnlockedAbility(abilityId))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasShipUnlockedAbility(string shipId, string abilityId)
        {
            var shipItem = GetShipItem(shipId);
            return shipItem.HasUnlockedAbility(abilityId);
        }

        public void CombatDone(int rawPlayedDays)
        {
            var shipItem = GetShipItem();
            //shipItem.saveData.rawPlayedDays = rawPlayedDays;
            UxService.instance.SaveGameData();
        }

        public void CheckShipUpgradeMissions()
        {
            var ml = MissionService.instance.GetCrtMainlineProto();
            if (ml != null && ml.content.type == MissionPrototype.MissionType.UpgradeShip)
            {
                CheckShipUpgradeMission("Puck", "p");
                CheckShipUpgradeMission("Schierke", "s");
                CheckShipUpgradeMission("Griffith", "g");
            }
        }

        void CheckShipUpgradeMission(string shipKey, string shipChar)
        {
            var item = GetShipItem(shipKey);
            if (item == null)
                return;

            var level = item.saveData.level;
            CheckShipUpgradeMission(shipChar, 5, level);
            CheckShipUpgradeMission(shipChar, 10, level);
            CheckShipUpgradeMission(shipChar, 20, level);
            CheckShipUpgradeMission(shipChar, 30, level);
            CheckShipUpgradeMission(shipChar, 40, level);
            CheckShipUpgradeMission(shipChar, 50, level);
            CheckShipUpgradeMission(shipChar, 60, level);
            CheckShipUpgradeMission(shipChar, 70, level);
            CheckShipUpgradeMission(shipChar, 80, level);
            //CheckShipUpgradeMission(shipChar, 100, level);//not in use
            //CheckShipUpgradeMission(shipChar, 120, level);//not in use
        }

        void CheckShipUpgradeMission(string shipChar, int levelCap, int level)
        {
            //Debug.Log("shipChar " + shipChar + " levelCap " + levelCap + " level " + level);
            MissionService.instance.PushMl("slv" + levelCap + "-" + shipChar, level, false);
        }
    }
}