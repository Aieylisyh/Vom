using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class CoreConfig : ScriptableObject
    {
        public int version_1;
        public int version_2;
        public int version_3;
        public int version_4;//platform specified, ios 1 android 2 pc 3 web 4 default0

        public string GetVersionString()
        {
            return "V " + version_1 + "." + version_2 + "." + version_3 + "." + version_4;
        }
    }
}