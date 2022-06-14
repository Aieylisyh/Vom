using UnityEngine;

namespace game
{
    public class LocalRemoteService : MonoBehaviour
    {
        public static LocalRemoteService instance;
        private void Awake()
        {
            instance = this;
        }
    }


}
