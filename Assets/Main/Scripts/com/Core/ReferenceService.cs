using UnityEngine;

namespace com
{
    public class ReferenceService : MonoBehaviour
    {
        public static ReferenceService Instance;

        void Awake()
        {
            Instance = this;
        }
    }
}