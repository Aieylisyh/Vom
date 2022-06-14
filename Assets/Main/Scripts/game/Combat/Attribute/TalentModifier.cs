
namespace game
{
    public class TalentModifier
    {
        //--{ok}--
        public int hpAdd = 0;//% tt_HullEnh
        //--{ok}--
        public int regAdd = 0;//% 
        //--{ok}--
        public int armorAdd = 0;//not % tt_ArmorThicken
        //--{ok}--
        public int regAdd_below50p = 0;//% tt_Emergency When your Hp is below 50%, Hp Recovery rate +{0}%
        //--{ok}--
        public int speedAdd = 0;//%
       //--{ok}--
        public int spareEngineSec = 0;//tt_Spare Avoid the first death each combat and gain invinsible for 1 second
       //--{ok}--
        public int hpRestoreSpareEngine = 0;//tt_Ultimate Restore {0}% Hp when [Spare Engine] triggers
        //--{ok}--
        public int dmgReduce_laserAdd = 0;//tt_Refractive Reduce {0}% damage from special sources(such as laser and ghost)
        //--{ok}--
        public int dmgReduce_ghostAdd = 0;//tt_Refractive Reduce {0}% damage from special sources(such as laser and ghost)
        //--{ok}--
        public int bombReplenishAdd = 0;//tt_FastReload Bomb replenish rate +{0}%
        //--{ok}--
        public int bombDmgAdd = 0;//% tt_BombEnh Bomb damage +{0}%
        //--{ok}--
        public int bombDmgAdd_critChance = 0;//% tt_MultiExp Bombs have 15% chance to deal {0}% more damage
        //--{ok}--
        public int bombAoeDmg = 0;//% tt_Concentrated
        //--{ok}--
        public int bombDmgAdd_lightTier = 0;//% tt_HE
        //--{ok}--
        public int bombDmgAdd_heavyTier = 0;//% tt_AP Bombs deal {0}% more damage to [Heavy Tier] enemies
        //--{ok}--
        public int bombBackupNum = 0;//  tt_Expand Add {0} backup bomb storage
        //--{ok}--
        public int bombAoeMaxHit = 0;//tt_Nuclear Bombs damage all nearby enemies when hit\nDeal {0}% AOE damage(can damage ghosts)
        //--{ok}--
        public int torDmgAdd = 0;//% tt_TorEnh
        //--{ok}--
        public int torChancePenetrate = 0;//tt_Penetrate Torpedoes have {0}% chance to deal 100% extra damage
        //--{ok}--
        public int torDmgAdd_below50p = 0;//tt_InnerExp Torpedoes deal {0}% more damage to targets with Hp below 40%
        //--{ok}--
        public int torExtraChance = 0;//tt_TorCapa {0}% chance to launch one extra when launch torpedoes manually
        //
        public int torDotPercent = 0;  //tt_Plasma Torpedoes will deal {0}% extra damage over 5 seconds\nDamage over time don't stack
        //--{ok}--
        public int torDmgAdd_multiHit = 0; //tt_Melt Torpedoes deal {0}% more damage when hit the same target multiple times
        //--{ok}--
        public int torAoeMaxHit = 0;//tt_SonicBoom Torpedoes damage all nearby enemies when hit\nDeal {0}% AOE damage(can damage ghosts)

        private void Reset()
        {
            hpAdd = 0;//% tt_HullEnh
            regAdd = 0;//% 
            armorAdd = 0;//not % tt_ArmorThicken
            regAdd_below50p = 0;//% tt_Emergency When your Hp is below 50%, Hp Recovery rate +{0}%
            speedAdd = 0;//%
            spareEngineSec = 0;//tt_Spare Avoid the first death each combat and gain invinsible for 1 second
            hpRestoreSpareEngine = 0;//tt_Ultimate Restore {0}% Hp when [Spare Engine] triggers
            dmgReduce_laserAdd = 0;//tt_Refractive Reduce {0}% damage from special sources(such as laser and ghost)
            dmgReduce_ghostAdd = 0;//tt_Refractive Reduce {0}% damage from special sources(such as laser and ghost)
            bombReplenishAdd = 0;//tt_FastReload Bomb replenish rate +{0}%
            bombDmgAdd = 0;//% tt_BombEnh Bomb damage +{0}%
            bombDmgAdd_critChance = 0;//% tt_MultiExp Bombs have 15% chance to deal {0}% more damage
            bombAoeDmg = 0;//% tt_Concentrated
            bombDmgAdd_lightTier = 0;//% tt_HE
            bombDmgAdd_heavyTier = 0;//% tt_AP Bombs deal {0}% more damage to [Heavy Tier] enemies
            bombBackupNum = 0;//  tt_Expand Add {0} backup bomb storage
            bombAoeMaxHit = 0;//tt_Nuclear Bombs damage all nearby enemies when hit\nDeal {0}% AOE damage(can damage ghosts)
            torDmgAdd = 0;//% tt_TorEnh
            torChancePenetrate = 0;//tt_Penetrate Torpedoes have {0}% chance to deal 100% extra damage
            torDmgAdd_below50p = 0;//tt_InnerExp Torpedoes deal {0}% more damage to targets with Hp below 40%
            torExtraChance = 0;//tt_TorCapa {0}% chance to launch one extra when launch torpedoes manually
            torDotPercent = 0;  //tt_Plasma Torpedoes will deal {0}% extra damage over 5 seconds\nDamage over time don't stack
            torDmgAdd_multiHit = 0; //tt_Melt Torpedoes deal {0}% more damage when hit the same target multiple times
            torAoeMaxHit = 0;//tt_SonicBoom Torpedoes damage all nearby enemies when hit\nDeal {0}% AOE damage(can damage ghosts)
        }

        public void Setup()
        {
            Reset();

            var talents = TalentService.instance.GetItems();
            foreach (var tt in talents)
            {
                var ttLevel = tt.saveData.level;
                //UnityEngine.Debug.Log(tt.id);
                var ttProto = TalentService.instance.GetPrototype(tt.id);
                //UnityEngine.Debug.Log(ttProto);
                var ttIntValue = ttProto.GetIntValue(ttLevel);

                switch (tt.id)
                {
                    case "tt_SonicBoom":
                        torAoeMaxHit += ttIntValue;
                        break;

                    case "tt_Melt":
                        torDmgAdd_multiHit += ttIntValue;
                        break;

                    case "tt_Plasma":
                        torDotPercent += ttIntValue;
                        break;

                    case "tt_TorCapa":
                        torExtraChance += ttIntValue;
                        break;

                    case "tt_InnerExp":
                        torDmgAdd_below50p += ttIntValue;
                        break;

                    case "tt_Penetrate":
                        torChancePenetrate += ttIntValue;
                        break;

                    case "tt_TorEnh":
                        torDmgAdd += ttIntValue;
                        break;

                    case "tt_Nuclear":
                        bombAoeMaxHit += ttIntValue;
                        break;

                    case "tt_Expand":
                        bombBackupNum += ttIntValue;
                        break;

                    case "tt_AP":
                        bombDmgAdd_heavyTier += ttIntValue;
                        break;

                    case "tt_HE":
                        bombDmgAdd_lightTier += ttIntValue;
                        break;

                    case "tt_Concentrated":
                        bombAoeDmg += ttIntValue;
                        break;

                    case "tt_MultiExp":
                        bombDmgAdd_critChance += ttIntValue;
                        break;

                    case "tt_BombEnh":
                        bombDmgAdd += ttIntValue;
                        break;

                    case "tt_FastReload":
                        bombReplenishAdd += ttIntValue;
                        break;

                    case "tt_Refractive":
                        dmgReduce_ghostAdd += ttIntValue;
                        dmgReduce_laserAdd += ttIntValue;
                        break;

                    case "tt_Ultimate":
                        hpRestoreSpareEngine += ttIntValue;
                        break;

                    case "tt_Spare":
                        spareEngineSec += ttIntValue;
                        break;

                    case "tt_Emergency":
                        regAdd_below50p += ttIntValue;
                        break;

                    case "tt_ArmorThicken":
                        armorAdd += ttIntValue;
                        break;

                    case "tt_HullEnh":
                        hpAdd += ttIntValue;
                        break;
                }
            }
        }
    }
}