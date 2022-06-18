using System;
using com;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class BossAi : EnemyAi
    {
        public List<BossDecisionData> skillsData;
        public BossSkillPrototype showupSkill;
        public BossSkillPrototype restSkill;

        private BossSkillPrototype _queuedNextSkill;
        private int _exclusiveGroupId;
        private BossSkillInstance _crtSkl;
        private List<BossSkillPrototype> _skills;
        private int _sklIndex;

        public override void ResetState()
        {
            base.ResetState();

            _skills = new List<BossSkillPrototype>();
            foreach (var data in skillsData)
            {
                _skills.Add(data.skl);
            }

            _sklIndex = 0;
            // _exclusiveGroupId = -1;
            _queuedNextSkill = null;
            _crtSkl = new BossSkillInstance();
            _crtSkl.Init(showupSkill);
            StartNewSkill();
        }

        protected override void Tick()
        {
            Think();
        }

        void Think()
        {
            CheckTurnDirection();
            //Debug.LogWarning(GameTime.time + " Think!!!");
            if (!IsIdle())
            {
                _crtSkl.Update();
                return;
            }

            var boss = self as Boss;
            boss.bossAttack.Cease();
            SetNewSkill();
            StartNewSkill();
        }

        bool CheckTurnDirection()
        {
            bool res = false;
            var cfg = ConfigService.instance.combatConfig;
            var distanceSafe = 4.1f;

            var boss = self as Boss;
            var left =111;
            var right = 1111;

            if (self.move.transform.position.x > right && boss.bossMove.goingRight && boss.bossMove.Speed != 0)
            {
                //Debug.Log("TurnBack");
                boss.bossMove.TurnBack(3f);
                return true;
            }
            else if (self.move.transform.position.x < left && !boss.bossMove.goingRight && boss.bossMove.Speed != 0)
            {
                //Debug.Log("TurnBack");
                boss.bossMove.TurnBack(3f);
                return true;
            }

            return res;
        }

        void StartNewSkill()
        {
            if (_crtSkl == null)
                return;

            //Debug.Log("StartNewSkill " + _crtSkl.proto.id);
            var boss = self as Boss;
            var sklProto = _crtSkl.proto;
            var bossAtk = boss.bossAttack;
            bossAtk.InitBossAttack(sklProto);
        }

        bool IsIdle()
        {
            if (_crtSkl == null)
                return true;

            return _crtSkl.ended;
        }

        void SetNewSkill()
        {
            var bsp = PickSkill();
            if (bsp != null)
            {
                //Debug.Log("SetNewSkill " + bsp.id);
                _crtSkl = new BossSkillInstance();
                _crtSkl.Init(bsp);
            }
        }

        public void SetQueuedSkill(BossSkillPrototype skl)
        {
            _queuedNextSkill = skl;
        }

        BossSkillPrototype GetNormalSkill()
        {
            _sklIndex++;
            if (_sklIndex > _skills.Count)
            {
                _sklIndex = 1;
            }

            return _skills[_sklIndex - 1];
        }

        bool LastIsRest()
        {
            //Debug.Log(_crtSkill);
            if (_crtSkl != null)
            {
                //Debug.Log(_crtSkill.prototype.id);
                return _crtSkl.proto.id == restSkill.id;
            }

            return false;
        }

        BossSkillPrototype PickSkill()
        {
            if (_queuedNextSkill != null)
            {
                var skl = _queuedNextSkill;
                _queuedNextSkill = null;
                return skl;
            }

            var lastIsRest = LastIsRest();
            //Debug.Log("lastIsRest? " + lastIsRest);
            if (lastIsRest)
            {
                BossSkillPrototype res;
                var count = 0;
                do
                {
                    res = GetNormalSkill();
                    count++;
                    //Debug.Log(_exclusiveGroupId);
                    if (count > 12)
                        return restSkill;
                } while (false && res.exclusiveGroupId == _exclusiveGroupId);
                //Debug.Log("GetRandomSkill!");
                //_exclusiveGroupId = res.exclusiveGroupId;
                return res;
            }
            //Debug.Log("rest!");
            return restSkill;
        }

        [Serializable]
        public struct BossDecisionData
        {
            public BossSkillPrototype skl;
            //public int pick;
        }
    }
}