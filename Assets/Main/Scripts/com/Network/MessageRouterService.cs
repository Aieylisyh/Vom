using UnityEngine;

namespace com
{
    public class MessageRouterService : MonoBehaviour
    {
        public static MessageRouterService Instance;
        private void Awake()
        {
            Instance = this;
        }
    }


}
