using UnityEngine;

namespace com
{
    [CreateAssetMenu]
    public class AdConfig : ScriptableObject
    {
        public bool AdFreeSuc = false;
        public string Android_default_Id;
        public string Ios_default_Id;
    }


}
