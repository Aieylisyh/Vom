using UnityEngine;

namespace vom
{
    public class EnemyAnimationEventResolver : MonoBehaviour
    {
        public EnemyBehaviour ene;

        public void Moved()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Moved");
        }

        public void Attacked()
        {
            //Debug.LogWarning("AnimationEventResolver " + "Attacked");
            ene.Attacked();
        }
    }
}