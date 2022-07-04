﻿using UnityEngine;
using com;

namespace vom
{
    public class SkillSystem : MonoBehaviour
    {
        public static SkillSystem instance { get; private set; }

        public SkillPrototype equipedSkl1 { get; private set; }
        public SkillPrototype equipedSkl2 { get; private set; }
        public SkillPrototype equipedSkl3 { get; private set; }

        public RuntimeSkillData _rtskl1;
        public RuntimeSkillData _rtskl2;
        public RuntimeSkillData _rtskl3;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            FetchEquipedSkills();
            ResetRuntimeSkillData();
        }

        void FetchEquipedSkills()
        {
            //var hero = game.ConfigService.instance.heroConfig.defaultHero;
            var hero = HeroSystem.instance.GetCurrentHeroData();
            equipedSkl1 = SkillService.GetPrototype(hero.equipedSkill1);
            equipedSkl2 = SkillService.GetPrototype(hero.equipedSkill2);
            equipedSkl3 = SkillService.GetPrototype(hero.equipedSkill3);

            EquipedSkillPanelBehaviour.instance.slots[0].Init(equipedSkl1);
            EquipedSkillPanelBehaviour.instance.slots[1].Init(equipedSkl2);
            EquipedSkillPanelBehaviour.instance.slots[2].Init(equipedSkl3);
        }

        void ResetRuntimeSkillData()
        {
            _rtskl1.hasCd = false;
            _rtskl2.hasCd = false;
            _rtskl3.hasCd = false;

            _rtskl1.useTimestamp = 0;
            _rtskl2.useTimestamp = 0;
            _rtskl3.useTimestamp = 0;
        }

        public void UseSkill1()
        {
            _rtskl1.hasCd = true;
            _rtskl1.useTimestamp = GameTime.time;
            UseSkill(equipedSkl1);
        }

        public void UseSkill2()
        {
            _rtskl2.hasCd = true;
            _rtskl2.useTimestamp = GameTime.time;
            UseSkill(equipedSkl2);
        }

        public void UseSkill3()
        {
            _rtskl3.hasCd = true;
            _rtskl3.useTimestamp = GameTime.time;
            UseSkill(equipedSkl3);
        }

        public bool CanUseSkill1()
        {
            if (_rtskl1.hasCd)
                return false;

            return true;
        }

        public bool CanUseSkill2()
        {
            if (_rtskl2.hasCd)
                return false;

            return true;
        }

        public bool CanUseSkill3()
        {
            if (_rtskl3.hasCd)
                return false;

            return true;
        }

        void SyncEquipedSkills()
        {
            SyncEquipedSkill(_rtskl1, equipedSkl1, EquipedSkillPanelBehaviour.instance.slots[0]);
            SyncEquipedSkill(_rtskl2, equipedSkl2, EquipedSkillPanelBehaviour.instance.slots[1]);
            SyncEquipedSkill(_rtskl3, equipedSkl3, EquipedSkillPanelBehaviour.instance.slots[2]);
        }

        void SyncEquipedSkill(RuntimeSkillData data, SkillPrototype skl, EquipedSkillSlotBehaviour slot)
        {
            if (data.hasCd)
            {
                var cd = skl.cd;
                var dt = GameTime.time - data.useTimestamp;
                if (dt >= cd)
                {
                    //finished cd
                    data.hasCd = false;
                    slot.Sync(0);
                    slot.Shine();
                    return;
                }

                slot.Sync(1 - dt / cd);
            }
        }

        private void Update()
        {
            SyncEquipedSkills();
        }

        void UseSkill(SkillPrototype skl)
        {
            Debug.Log("UseSkill " + skl.id);
            switch (skl.id)
            {
                case "FireOrb":
                    PlayerBehaviour.instance.attack.AddFireBalls();
                    break;

                case "IceOrb":
                    PlayerBehaviour.instance.attack.AddIceBalls();
                    break;

                case "PoisonOrb":
                    PlayerBehaviour.instance.attack.AddPoisonBalls();
                    break;

                case "Spint":
                    break;

                case "Blink":
                    break;

                case "FrostNova":
                    break;

                case "ArcaneExposion":
                    break;

                case "ArcaneBlasts":
                    break;
            }
        }
    }
}