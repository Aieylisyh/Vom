using UnityEngine;
using com;

namespace vom
{
    [CreateAssetMenu]
    public class SettingsConfig : ScriptableObject
    {
        public LocalizationService.Language languageDefault;

        public bool sfxOn;
        public bool musicOn;
        public bool vibrateOn;

        public int ver_1;
        public int ver_2;
        public int ver_3;
        public int ver_4;//platform specified, ios 1 android 2 pc 3 web 4 default0

        public string GetVersionString()
        {
            return "V " + ver_1 + "." + ver_2 + "." + ver_3 + "." + ver_4;
        }
    }
}