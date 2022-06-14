namespace game
{
    public class ShipAbilityModifier
    {
        public int hpAdd = 0;//%
        public int regAdd = 0;//%
        public int armorAdd = 0;//n
        public int speedAdd = 0;//%
        public int torDmgAdd = 0;//%
        public int bombDmgAdd = 0;//%

        public bool isPuck;
        public bool isSchierke;
        public bool isGriffith;

        public int dmgReduceGhostAdd;
        public int dmgReduceLaserAdd;
        public int dmgReduceHotAdd;

        private void Reset()
        {
            hpAdd = 0;//%
            regAdd = 0;//%
            armorAdd = 0;//n
            speedAdd = 0;//%
            torDmgAdd = 0;//%
            bombDmgAdd = 0;//%

            dmgReduceGhostAdd = 0;//%
            dmgReduceLaserAdd = 0;//%
            dmgReduceHotAdd = 0;//%
        }

        public void Setup(ShipItem shipItem)
        {
            isPuck = shipItem.id == "Puck";
            isSchierke = shipItem.id == "Schierke";
            isGriffith = shipItem.id == "Griffith";

            Reset();

            var ships = ShipService.instance.GetShipItems();
            foreach (var ship in ships)
            {
                foreach (var sab in ship.saveData.unlockedAbilities)
                {
                    var sabProto = ShipService.instance.GetAbilityPrototype(sab);
                    var intV = sabProto.intParam;
                    var intVOther = sabProto.intOtherParam;
                    switch (sab)
                    {
                        case "Puck_ab_1":
                            //精灵的经验：使巴克的炸弹外形变为锚链，伤害提升5%
                            if (isPuck)
                            {
                                bombDmgAdd += intV;
                            }
                            break;

                        case "Puck_ab_2":
                            //精灵的狗屎运：港口有时会来一条鲨鱼，赶走它！
                            break;

                        case "Puck_ab_3":
                            //巴克回复+100% 
                            if (isPuck)
                            {
                                regAdd += intV;
                            }
                            else
                            {
                                regAdd += intVOther;
                            }
                            break;

                        case "Puck_ab_4":
                            //所有人护甲+4
                            if (isPuck)
                            {
                                armorAdd += intV;
                            }
                            else
                            {
                                armorAdd += intVOther;
                            }
                            break;

                        case "Puck_ab_5":
                            //巴克对激光伤害减免25%
                            if (isPuck)
                            {
                                dmgReduceLaserAdd += intV;
                            }
                            else
                            {
                                dmgReduceLaserAdd += intVOther;
                            }
                            break;

                        case "Puck_ab_6":
                            //巴克生命+50%
                            if (isPuck)
                            {
                                hpAdd += intV;
                            }
                            else
                            {
                                hpAdd += intVOther;
                            }
                            break;

                        case "Puck_ab_7":
                            //所有人 速度+30%，加一个石像
                            speedAdd += intV;
                            break;

                        case "Schierke_ab_1":
                            //异邦的科技：使史尔基的鱼雷外形变为魔法飞弹，伤害提高5%
                            if (isSchierke)
                            {
                                torDmgAdd += intV;
                            }
                            break;

                        case "Schierke_ab_2":
                            //神秘国度的财富：在港口创建魔法传送门，点击可以获得金币
                            break;

                        case "Schierke_ab_3":
                            //史尔基伤害+35%
                            if (isSchierke)
                            {
                                torDmgAdd += intV;
                                bombDmgAdd += intV;
                            }
                            else
                            {
                                torDmgAdd += intVOther;
                                bombDmgAdd += intVOther;
                            }
                            break;

                        case "Schierke_ab_4":
                            //所有人回复+30%
                            if (isSchierke)
                            {
                                regAdd += intV;
                            }
                            else
                            {
                                regAdd += intVOther;
                            }
                            break;

                        case "Schierke_ab_5":
                            //史尔基对幽灵伤害减免25%
                            if (isSchierke)
                            {
                                dmgReduceGhostAdd += intV;
                            }
                            else
                            {
                                dmgReduceGhostAdd += intVOther;
                            }
                            break;

                        case "Schierke_ab_6":
                            //史尔基hp
                            if (isSchierke)
                            {
                                hpAdd += intV;
                            }
                            else
                            {
                                hpAdd += intVOther;
                            }
                            break;

                        case "Schierke_ab_7":
                            //所有人伤害+30%，加一个石像
                            torDmgAdd += intV;
                            bombDmgAdd += intV;
                            break;

                        case "Griffith_ab_1":
                            //高远雄心：使格里菲斯的武器外形变为生化武器，伤害提高3%
                            if (isGriffith)
                            {
                                torDmgAdd += intV;
                                bombDmgAdd += intV;
                            }
                            break;

                        case "Griffith_ab_2":
                            //鹰的梦境：港口出现更多白色的大鸟，钓鱼效率提高10%
                            break;

                        case "Griffith_ab_3":
                            //格里菲斯护甲+5
                            if (isGriffith)
                            {
                                armorAdd += intV;
                            }
                            else
                            {
                                armorAdd += intVOther;
                            }
                            break;

                        case "Griffith_ab_4":
                            //所有人生命+30%
                            if (isGriffith)
                            {
                                hpAdd += intV;
                            }
                            else
                            {
                                hpAdd += intVOther;
                            }
                            break;

                        case "Griffith_ab_5":
                            //格里菲斯常规热武器伤害减免15%
                            if (isGriffith)
                            {
                                dmgReduceHotAdd += intV;
                            }
                            else
                            {
                                dmgReduceHotAdd += intVOther;
                            }
                            break;

                        case "Griffith_ab_6":
                            //格里菲斯伤害+30%
                            if (isGriffith)
                            {
                                torDmgAdd += intV;
                                bombDmgAdd += intV;
                            }
                            else
                            {
                                torDmgAdd += intVOther;
                                bombDmgAdd += intVOther;
                            }
                            break;

                        case "Griffith_ab_7":
                            //所有人护甲+5，加一个石像
                            armorAdd += intV;
                            break;
                    }
                }
            }
        }
    }
}