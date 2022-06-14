using UnityEngine;

namespace com
{
    public class NativeService : MonoBehaviour
    {
        //systeminfo/screenshot/vibrate/brightness/webview
        public static NativeService Instance;
        private void Awake()
        {
            Instance = this;
        }
    }


}
