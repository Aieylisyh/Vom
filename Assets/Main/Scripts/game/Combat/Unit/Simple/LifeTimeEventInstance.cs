using UnityEngine;
using com;

namespace game
{
    public class LifeTimeEventInstance
    {
        public LifeTimeEvent lifeTimeEvent;

        private float _timeToTrigger;
        private bool _triggered;
        private bool _ended;
        private float _lastEventMoment;
        private float _durationTimer;
        public Unit host;

        public void Reset(float moment)
        {
            _lastEventMoment = moment;
            Reset();
        }

        public void Reset()
        {
            _triggered = false;

            switch (lifeTimeEvent.type)
            {
                case LifeTimeEvent.LifeTimeEventType.None:
                    _ended = true;
                    _timeToTrigger = -1;
                    break;

                case LifeTimeEvent.LifeTimeEventType.Delay:
                    _ended = false;
                    _timeToTrigger = _lastEventMoment + lifeTimeEvent.GetTime();
                    break;
            }
        }

        public void TryTrigger()
        {
            if (_triggered)
            {
                if (_durationTimer > 0)
                {
                    Tick(false);
                }
                else
                {
                    if (_ended)
                    {
                        lifeTimeEvent.nextEvent?.instance.TryTrigger();
                    }
                }
                return;
            }

            if (_timeToTrigger > 0 && GameTime.time > _timeToTrigger)
            {
                Trigger();
            }
        }

        public void ForceTrigger()
        {
            if (_triggered)
                return;

            Reset();
            Trigger();
        }

        private void Trigger()
        {
            _triggered = true;

            if (lifeTimeEvent.duration > 0)
            {
                _durationTimer = lifeTimeEvent.duration;
            }
            else
            {
                _durationTimer = 0;
            }

            Tick(true);
        }

        void PlaySound()
        {
            if (!string.IsNullOrEmpty(lifeTimeEvent.paramString1))
            {
                SoundService.instance.Play(lifeTimeEvent.paramString1);
            }
        }

        protected virtual void Tick(bool isFirst)
        {
            if (!host.IsAlive())
                return;

            switch (lifeTimeEvent.lifeTimeEventFunction)
            {
                case LifeTimeEvent.LifeTimeEventFunction.Acc:
                    if (isFirst)
                        PlaySound();
                    host.move.Speed += lifeTimeEvent.paramFloat1 * com.GameTime.deltaTime;//acc, minus 0 is deccelerate
                    break;

                case LifeTimeEvent.LifeTimeEventFunction.Attack:
                    if (isFirst)
                        PlaySound();
                    host.attack.Attack();
                    break;

                case LifeTimeEvent.LifeTimeEventFunction.SpecialModuleToggle:
                    if (isFirst)
                        PlaySound();
                    host.ToggleSpecialModule(lifeTimeEvent.paramBool1);
                    break;

                case LifeTimeEvent.LifeTimeEventFunction.DieUnsilent:
                    if (isFirst)
                        PlaySound();
                    host.death.Die(false);
                    break;

                case LifeTimeEvent.LifeTimeEventFunction.TurnBack:
                    if (isFirst)
                    {
                        PlaySound();
                        var em = host.move as EnemyMove;
                        em.TurnBack(lifeTimeEvent.paramFloat1);
                    }
                    break;
            }

            _durationTimer -= com.GameTime.deltaTime;
            if (_durationTimer < 0)
            {
                OnEnd();
            }
        }

        private void OnEnd()
        {
            _ended = true;
            _durationTimer = 0;
            if (lifeTimeEvent.nextEvent != null)
            {
                lifeTimeEvent = lifeTimeEvent.nextEvent;
                Reset(GameTime.time);
            }
        }
    }
}
