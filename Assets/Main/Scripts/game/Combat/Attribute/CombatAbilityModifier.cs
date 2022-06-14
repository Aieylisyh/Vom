namespace game
{
    public class CombatAbilityModifier
    {
        public int hpAdd = 0;//%
        public int regAdd = 0;//%
        public int armorAdd = 0;//not %
        public int speedAdd = 0;//not %
        public int torDmgAdd = 0;//%
        public int bombDmgAdd = 0;//%
        public int bombShardsDmgPercent;//%

        public bool autoMissile = false;
        public bool doubleBomb = false;
        public bool bombDropFaster = false;
        public int bombReplenishAdd = 0;//&
        public int autoMissileDmgPercent;
        public int torExtraAdd;
        public int torDirDmgAdd;
        public bool torDirSpeedUp;
        public bool torDirHitProj;
        public bool torTraceHitProj;
        public bool bombHitProj;
        public int detonateDmgAdd;
        public bool autoTor;
        public int torChargeTimeReduceAdd;
        public bool bombSizeHuge;
        public bool healToFull;
        public int overrideHp;
        public int dmgReduceLaserAdd;
        public int dmgReduceHotAdd;
        public bool enable_ca_Reg;
        public bool enable_ca_Refurbish;

        public void HardReset()
        {
            enable_ca_Refurbish = true;
            enable_ca_Reg = true;
        }

        private void Reset()
        {
            hpAdd = 0;//%
            regAdd = 0;//%
            armorAdd = 0;//not %
            speedAdd = 0;//not %
            torDmgAdd = 0;//%
            bombDmgAdd = 0;//%
            bombShardsDmgPercent = 0;//%

            autoMissile = false;
            autoMissileDmgPercent = 0;
            doubleBomb = false;
            bombReplenishAdd = 0;

            torExtraAdd = 0;
            torDirDmgAdd = 0;
            torDirSpeedUp = false;
            torDirHitProj = false;
            torTraceHitProj = false;
            bombHitProj = false;
            detonateDmgAdd = 0;
            autoTor = false;
            bombDropFaster = false;
            torChargeTimeReduceAdd = 0;
            bombSizeHuge = false;
            healToFull = false;
            overrideHp = 0;
            dmgReduceLaserAdd = 0;
            dmgReduceHotAdd = 0;
        }

        public void Setup()
        {
            Reset();
            var cabIds = CombatAbilityService.instance.selectedPool;
            if (cabIds == null)
                return;

            foreach (var v in cabIds)
            {
                ValidateSelected(v);
            }

            //InputPanel.instance.longPressController.SetChargeTime();
            CombatService.instance.playerShip.InitData(true);
        }

        public void ValidateSelected(string v)
        {
            var proto = CombatAbilityService.instance.GetPrototype(v);
            int value = proto.intValue;
            //proto.hasIntValue
            switch (v)
            {
                case "ca_Armor":
                    //hot weapon reduce
                    dmgReduceHotAdd += value;
                    break;

                case "ca_AutoMissile":
                    //Every 3 seconds, launch a missile which deals {0}% torpedo damage
                    autoMissile = true;
                    autoMissileDmgPercent += value;
                    break;

                case "ca_Bombs":
                    //Double your bombs, but decrease bomb replenish rate by {0}%
                    doubleBomb = true;
                    bombReplenishAdd -= value;
                    break;

                case "ca_Shards":
                    bombShardsDmgPercent += value;
                    break;

                case "ca_IntensiveBomb":
                    //Bombs drop faster. Increase bomb replenish rate by {0}%
                    bombReplenishAdd += value;
                    bombDropFaster = true;
                    break;

                case "ca_Maintenance":
                    //Increase Hp recovery per second by {0}%
                    regAdd += value;
                    break;

                case "ca_Reboot":
                    //Replace all combat abilities to choose from
                    break;

                case "ca_Speed":
                    //Directional torpedo's damage +{0}%, increase directional torpedo's speed
                    torDirDmgAdd += value;
                    torDirSpeedUp = true;
                    //test
                    break;

                case "ca_MinArmor":
                    //Increase ship armor by {0}
                    dmgReduceLaserAdd += value;
                    break;

                case "ca_MinDmg":
                    torDirDmgAdd += value;
                    bombDmgAdd += value;
                    break;

                case "ca_MinBombDmg":
                    //Increase bomb damage by {0}%
                    bombDmgAdd += value;
                    break;

                case "ca_MinBombRep":
                    //Increase bomb replenish rate by {0}%
                    bombReplenishAdd += value;
                    break;

                case "ca_MinHp":
                    //Increase maximum Hp by {0}%
                    hpAdd += value;
                    break;

                case "ca_MinTorDmg":
                    //Increase torpedo damage by {0}%
                    torDmgAdd += value;
                    break;

                case "ca_Detect":
                    //Enable your bombs and torpedoes to hit ghosts and enemy projectiles
                    torDirHitProj = true;
                    torTraceHitProj = true;
                    bombHitProj = true;
                    break;

                case "ca_Detonate":
                    //Bomb deals {0}% of a target's Maximum Hp as extra damage, if the target's Hp is full
                    detonateDmgAdd += value;
                    break;

                case "ca_LaunchTube":
                    //75% chance to add {0} extra torpedoes when launching manually
                    torExtraAdd += value;
                    break;

                case "ca_AutoTor":
                    //Every 4 seconds, launch 2 torpedoes which deals {0}% torpedo damage
                    autoTor = true;
                    break;

                case "ca_Fluid":
                    //Decrease the charge time when launch torpedoes manually
                    torChargeTimeReduceAdd += value;
                    break;

                case "ca_HugeBomb":
                    //Increase bomb damage by {0}%. Increase the collision size of bomb
                    bombSizeHuge = true;
                    bombDmgAdd += value;
                    break;

                case "ca_Refurbish":
                    //Increase maximum Hp by {0}%. Instantly recover to full Hp.
                    hpAdd += value;
                    if (enable_ca_Refurbish)
                    {
                        healToFull = true;
                        enable_ca_Refurbish = false;
                    }
                    break;

                case "ca_Reg":
                    //Keep only 1 Hp. Increase Hp recovery per second by {0}%
                    regAdd += value;
                    if (enable_ca_Reg)
                    {
                        enable_ca_Reg = false;
                        overrideHp = 1;
                    }
                    break;

                case "ca_Turbo":
                    //Ship speed +{0}
                    speedAdd += value;
                    break;

                case "ca_Heal":
                    healToFull = true;
                    break;
            }
        }

    }
}