using UnityEngine;
using com;
using System.Collections.Generic;

namespace game
{
    public class EnemyAi : UnitAi
    {
        public List<LifeTimeEvent> events;
        private List<LifeTimeEventInstance> _events;

        protected override void Tick()
        {
            UpdateEvents();
        }

        private void UpdateEvents()
        {
            if (_events == null || _events.Count < 1)
            {
                return;
            }

            foreach (var i in _events)
            {
                i.TryTrigger();
            }
        }

        public void ForceTriggerEvent(int i)
        {
            _events[i].ForceTrigger();
        }

        public override void ResetState()
        {
            _events = new List<LifeTimeEventInstance>();
            foreach (var e in events)
            {
                if (e == null)
                    continue;

                var i = new LifeTimeEventInstance();
                i.host = self;
                i.lifeTimeEvent = e;
                i.Reset(GameTime.time);

                _events.Add(i);
            }
        }
    }
}