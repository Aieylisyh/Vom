using UnityEngine;
using System.Collections;
using com;

namespace game
{
    public class BossSkillInstance
    {
        public bool ended { get; private set; }
        public BossSkillPrototype proto { get; private set; }

        private float _endGameTime;

        public void Update()
        {
            if (ended)
                return;
            //Debug.Log(GameTime.time + " --Update");
            if (proto.isTimedSkill)
            {
                //Debug.Log(_endGameTime - GameTime.time);
                if (GameTime.time >= _endGameTime)
                    ended = true;
            }
            else
            {
                //check other end conditions
            }
        }

        public void Init(BossSkillPrototype p)
        {
            proto = p;
            ended = false;

            if (p.isTimedSkill)
            {
                //Debug.Log(GameTime.time + " now");
                _endGameTime = GameTime.time + p.duration;
                //Debug.Log(_endGameTime + " _endGameTime");
            }
        }
    }
}