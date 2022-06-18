using System.Collections.Generic;
using com;

namespace game
{
    public class PlayerAttributes
    {
        public PlayerAttributes(string id)
        {
            shipId = id;
            ttModifier = new TalentModifier();
            //sabModifier = new ShipAbilityModifier();
            cabModifier = new CombatAbilityModifier();
        }

        public enum PlayerAttriLayer
        {
            Ship,
            ShipAbility,
            Talent,
            Combat,
        }

        public struct BaseValue
        {
            public int hp;
            public float speed;
            public int torDmg;
            public int bombDmg;
            public int reg;//reg per tick
            public int vita;

            public int armor;
            public int armor_hotWeapon;
            public int armor_laser;
            public int armor_ghost;

            public float dmgReduce;
            public float dmgReduce_hot;
            public float dmgReduce_laser;
            public float dmgReduce_ghost;

            public int torTraceNum;
            public int torDirNum;
            public int bombStorage;
            public float bombInterval;
        }

        public string shipId { get; private set; }

        public TalentModifier ttModifier;
        public CombatAbilityModifier cabModifier;        //RuntimeLevel List<string> cabCards to check cab

        public BaseValue vBase;

        public int hp
        {
            get
            {
                return 0;
            }
        }
        public float speed
        {
            get
            {
                return 0;
            }
        }
        public int torDmg
        {
            get
            {
                return 0;
                // return MathGame.GetPercentageAdded(       vBase.torDmg,     sabModifier.torDmgAdd + cabModifier.torDmgAdd + ttModifier.torDmgAdd);
            }
        }
        public int bombDmg
        {
            get
            {
                return 0;
            }
        }
        //reg per tick
        public int reg
        {
            get
            {
                return 0;
            }
        }
        public int vita
        {
            get { return vBase.vita; }
        }
        public int armor
        {
            get
            {
                return 0;
            }
        }
        public int armor_hotWeapon
        {
            get { return vBase.armor_hotWeapon; }
        }
        public int armor_laser
        {
            get { return vBase.armor_laser; }
        }
        public int armor_ghost
        {
            get { return 0; }
        }
        public float dmgReduce
        {
            get { return vBase.dmgReduce; }
        }
        public float dmgReduce_hotWeapon
        {
            get { return 0; }
        }
        public float dmgReduce_laser
        {
            get { return 0; }
        }
        public float dmgReduce_ghost
        {
            get { return 0; }
        }
        public int torTraceNum
        {
            get { return vBase.torTraceNum; }
        }
        public int torDirNum
        {
            get { return vBase.torDirNum; }
        }
        public int bombStorage
        {
            get { return vBase.bombStorage; }
        }
        public float bombInterval
        {
            get { return vBase.bombInterval; }
        }

        public void HardReset()
        {
            cabModifier.HardReset();
        }

        public void Refresh(PlayerAttriLayer layer)
        {
            var shipItem = ShipService.instance.GetShipItem(shipId);
            var proto = ShipService.instance.GetPrototype(shipId);

            if (layer == PlayerAttriLayer.Ship)
            {
                //UnityEngine.Debug.Log("value_base");
                var shipLevel = 1;
                if (shipItem.saveData.unlocked)
                    shipLevel = shipItem.saveData.level;

                vBase.hp = proto.hp.GetIntValue(shipLevel);
                vBase.speed = proto.speed.GetValue(shipLevel);
                vBase.torDmg = proto.torDmg.GetIntValue(shipLevel);
                vBase.bombDmg = proto.bombDmg.GetIntValue(shipLevel);
                vBase.reg = proto.reg.GetIntValue(shipLevel);
                vBase.vita = proto.vita.GetIntValue(shipLevel);

                vBase.armor = proto.armor.GetIntValue(shipLevel);
                vBase.armor_hotWeapon = proto.armor_hotWeapon.GetIntValue(shipLevel);
                vBase.armor_laser = proto.armor_laser.GetIntValue(shipLevel);
                vBase.armor_ghost = proto.armor_ghost.GetIntValue(shipLevel);
                vBase.dmgReduce = proto.dmgReduce.GetValue(shipLevel);
                vBase.dmgReduce_hot = proto.dmgReduce_hotWeapon.GetValue(shipLevel);
                vBase.dmgReduce_laser = proto.dmgReduce_laser.GetValue(shipLevel);
                vBase.dmgReduce_ghost = proto.dmgReduce_ghost.GetValue(shipLevel);

                layer = PlayerAttriLayer.ShipAbility;
            }

            if (layer == PlayerAttriLayer.ShipAbility)
            {
                //UnityEngine.Debug.Log("shipAbilityModifier");
                //sabModifier.Setup(shipItem);

                layer = PlayerAttriLayer.Talent;
            }

            if (layer == PlayerAttriLayer.Talent)
            {
                //UnityEngine.Debug.Log("talentModifier");
                ttModifier.Setup();
                layer = PlayerAttriLayer.Combat;
            }

            if (layer == PlayerAttriLayer.Combat)
            {
                cabModifier.Setup();
                //UnityEngine.Debug.Log("combatAbilityModifier");
            }

            //Log();
        }

        private void Log()
        {
            UnityEngine.Debug.Log("---PlayerAttributes Refresh---");
            UnityEngine.Debug.Log("vBase");
            TextFormat.DumpToConsole(vBase);
            UnityEngine.Debug.Log("ttModifier");
            TextFormat.DumpToConsole(ttModifier);
            UnityEngine.Debug.Log("sabModifier");
            // TextFormat.DumpToConsole(sabModifier);
            UnityEngine.Debug.Log("cabModifier");
            TextFormat.DumpToConsole(cabModifier);
        }
    }
}