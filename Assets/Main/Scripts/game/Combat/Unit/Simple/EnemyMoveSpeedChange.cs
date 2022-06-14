using com;
using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class EnemyMoveSpeedChange : LifeTimeEvent
    {
        public float acc;//minus 0 is deccelerate
        public string sound;
    }
}
