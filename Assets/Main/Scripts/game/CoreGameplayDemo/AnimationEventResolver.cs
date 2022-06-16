using UnityEngine;

namespace vom
{
    public class AnimationEventResolver : MonoBehaviour
    {
        public void Moved()
        {
            Debug.LogWarning("AnimationEventResolver " + "Moved");
        }
    }
}