using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    public class SettingsConfig : ScriptableObject
    {
        public LocalizationService.Language languageDefault;
        public bool sfxOn;
        public bool musicOn;
        public bool vibrateOn;
    }
}