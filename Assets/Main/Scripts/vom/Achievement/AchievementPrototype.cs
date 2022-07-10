using UnityEngine;

namespace vom
{
    [CreateAssetMenu]
    public class AchievementPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }

        public int quota = 1;

        public Sprite sp;
        public VfxType vfx;
    }
}