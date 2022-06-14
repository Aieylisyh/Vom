using UnityEngine;

namespace com
{
    public class NetworkService : MonoBehaviour
    {
        public static NetworkService Instance;
        private void Awake()
        {
            Instance = this;
        }
    }


}
