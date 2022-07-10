using UnityEngine;

namespace vom
{
    [CreateAssetMenu]
    public class DailyPerkPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }

        public bool unlockedByDefault;
        public int minAvailableLevel = 0;
        public bool isBackup;

        public int intValue;
        public bool hasIntValue = true;

        public Sprite sp;
        public VfxType vfx;
    }
}