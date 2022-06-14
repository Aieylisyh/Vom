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
            sabModifier = new ShipAbilityModifier();
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
        public ShipAbilityModifier sabModifier;
        public CombatAbilityModifier cabModifier;        //RuntimeLevel List<string> cabCards to check cab

        public BaseValue vBase;

        public int hp
        {
            get
            {
                return MathGame.GetPercentageAdded(
                    vBase.hp,
                    sabModifier.hpAdd + cabModifier.hpAdd + ttModifier.hpAdd);
            }
        }
        public float speed
        {
            get
            {
                var speedMultiplied = MathGame.GetPercentageAdded(vBase.speed, sabModifier.speedAdd + ttModifier.speedAdd);
                return speedMultiplied + 0.01f * cabModifier.speedAdd;
            }
        }
        public int torDmg
        {
            get
            {
                return MathGame.GetPercentageAdded(
           vBase.torDmg,
           sabModifier.torDmgAdd + cabModifier.torDmgAdd + ttModifier.torDmgAdd);
            }
        }
        public int bombDmg
        {
            get
            {
                return MathGame.GetPercentageAdded(
           vBase.bombDmg,
           sabModifier.bombDmgAdd + cabModifier.bombDmgAdd + ttModifier.bombDmgAdd);
            }
        }
        //reg per tick
        public int reg
        {
            get
            {
                return MathGame.GetPercentageAdded(
            vBase.reg,
            sabModifier.regAdd + cabModifier.regAdd + ttModifier.regAdd);
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
                return vBase.armor
                  + sabModifier.armorAdd + cabModifier.armorAdd + ttModifier.armorAdd;
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
            get { return vBase.armor_ghost + sabModifier.dmgReduceGhostAdd; }
        }
        public float dmgReduce
        {
            get { return vBase.dmgReduce; }
        }
        public float dmgReduce_hotWeapon
        {
            get { return vBase.dmgReduce_hot + sabModifier.dmgReduceHotAdd + cabModifier.dmgReduceHotAdd; }
        }
        public float dmgReduce_laser
        {
            get
            {
                return vBase.dmgReduce_laser + ttModifier.dmgReduce_laserAdd + sabModifier.dmgReduceLaserAdd + cabModifier.dmgReduceLaserAdd;
            }
        }
        public float dmgReduce_ghost
        {
            get
            {
                return vBase.dmgReduce_ghost
                   + ttModifier.dmgReduce_ghostAdd;
            }
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

                vBase.torTraceNum = ConfigService.instance.combatConfig.playerParam.torTraceNum;
                vBase.torDirNum = ConfigService.instance.combatConfig.playerParam.torDirNum;
                vBase.bombStorage = ConfigService.instance.combatConfig.playerParam.bombStorage;
                vBase.bombInterval = ConfigService.instance.combatConfig.playerParam.bombInterval;

                layer = PlayerAttriLayer.ShipAbility;
            }

            if (layer == PlayerAttriLayer.ShipAbility)
            {
                //UnityEngine.Debug.Log("shipAbilityModifier");
                sabModifier.Setup(shipItem);

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
            TextFormat.DumpToConsole(sabModifier);
            UnityEngine.Debug.Log("cabModifier");
            TextFormat.DumpToConsole(cabModifier);
        }
    }
}