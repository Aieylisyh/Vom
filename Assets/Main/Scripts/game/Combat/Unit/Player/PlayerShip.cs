using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class PlayerShip : Unit
    {
        public List<SpotLightBehaviour> spotLights;
        public Transform combatPos;
        public ShipModelSwitcher shipModelSwitcher;

        public PlayerAi playerAi
        {
            get { return ai as PlayerAi; }
        }
        public PlayerAttack playerAttack
        {
            get { return attack as PlayerAttack; }
        }

        public PlayerMove playerMove
        {
            get { return move as PlayerMove; }
        }

        protected override void Start()
        {
            base.Start();
            ResetComponentState();
        }

        public void RefreshAttributes()
        {
            //Debug.Log("refresh Attri");
            var attri = CombatService.instance.playerAttri;

            move.Speed = attri.speed;
            health.hpMax = attri.hp;
            health.reg = attri.reg;

            health.armor = attri.armor;
            health.armor_hotWeapon = attri.armor_hotWeapon;
            health.armor_laser = attri.armor_laser;
            health.armor_ghost = attri.armor_ghost;
            health.dmgReduce = attri.dmgReduce;
            health.dmgReduce_hotWeapon = attri.dmgReduce_hotWeapon;
            health.dmgReduce_laser = attri.dmgReduce_laser;
            health.dmgReduce_ghost = attri.dmgReduce_ghost;

            playerAttack.InitBomb(attri.bombDmg, attri.bombStorage, attri.bombInterval);
            playerAttack.InitTor(attri.torDmg, attri.torTraceNum, attri.torDirNum);
        }

        public void InitData(bool keepHp)
        {
            //Debug.Log("Init Data keepHp " + keepHp);
            int hp = health.hp;
            int hpMax = health.hpMax;

            RefreshAttributes();
            ResetComponentState();

            var attri = CombatService.instance.playerAttri;
            if (keepHp)
            {
                if (attri.cabModifier.overrideHp > 0)
                {
                    hp = attri.cabModifier.overrideHp;
                }
                if (attri.cabModifier.healToFull)
                {
                    hp = health.hpMax;
                }

                int hpMaxNew = health.hpMax;
                var delta = hpMaxNew - hpMax;
                if (delta > 0)
                {
                    health.SetHealth(hp + delta);
                }
                else
                {
                    health.SetHealth(hp);
                }
            }
            else
            {
                health.SetHealth(health.hpMax);
            }
        }

        public void ResetPlayerView(bool repos = false)
        {
            if (repos)
            {
                Reposition();
                //Init();//???
            }

            CloseLights();
        }

        public void Reposition()
        {
            move.transform.SetPositionAndRotation(combatPos.position, combatPos.rotation);
        }

        public override void Recycle()
        {
            Debug.LogError("Recycle player?");
        }

        public void OpenLights()
        {
            foreach (var s in spotLights)
            {
                s.flagAwake = true;
            }
        }

        public void CloseLights()
        {
            foreach (var s in spotLights)
            {
                s.StopLights();
            }
        }
    }
}
