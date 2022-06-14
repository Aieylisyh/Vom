using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class LifeTimeEvent : ScriptableObject
    {
        public enum LifeTimeEventType
        {
            None,//will not trigger unless called
            Delay,//will trigger after [time] counting from the last EnemyLifeTimeEvent, or the the spawn moment
        }

        public LifeTimeEventType type;
        public float time;
        public float timeAddRandomRange;
        public LifeTimeEvent nextEvent;

        public bool instantEnd;
        public float duration = 0;

        public LifeTimeEventInstance instance;
        public enum LifeTimeEventFunction
        {
            None,
            Acc,
            Attack,
            SpecialModuleToggle,
            DieUnsilent,
            TurnBack,
        }
        public LifeTimeEventFunction lifeTimeEventFunction;

        public float paramFloat1;
        public string paramString1;
        public bool paramBool1;

        public float GetTime()
        {
            return Random.Range(time, time + timeAddRandomRange);
        }
    }
}
