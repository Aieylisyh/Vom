using UnityEngine;

namespace vom
{
    public class PlayerAnimationEventResolver : VomUnitComponent
    {
        public void Moved()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Moved");
            player.move.Moved();
        }

        public void Attacked()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Attacked");
            player.attack.Attacked();
        }

        public void Spawned()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Spawned");
        }
    }
}