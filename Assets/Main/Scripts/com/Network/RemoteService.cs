using UnityEngine;

namespace com
{
    public class RemoteService : MonoBehaviour
    {
        //upload data/file,request...
        public static RemoteService Instance;
        private void Awake()
        {
            Instance = this;
        }
    }


}
