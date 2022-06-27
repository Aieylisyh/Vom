using UnityEngine;

namespace vom
{
    public class EnemyAnimationEventResolver : VomEnemyComponent
    {
        public void Moved()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Moved");
        }

        public void Attacked()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Attacked");
            host.attack.Attacked();
        }
    }
}