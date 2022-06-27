using UnityEngine;

namespace vom
{
    public class PlayerAnimationEventResolver : VomPlayerComponent
    {
        public void Moved()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Moved");
            host.move.Moved();
        }

        public void Attacked()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Attacked");
            host.attack.Attacked();
        }

        public void Spawned()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Spawned");
        }

        public void DiedFromEnemy()
        {

        }
    }
}