using UnityEngine;

namespace com
{
    public class FileService : MonoBehaviour
    {
        //download/preload/release¡¢storageService
        public static FileService Instance;
        private void Awake()
        {
            Instance = this;
        }
    }


}
