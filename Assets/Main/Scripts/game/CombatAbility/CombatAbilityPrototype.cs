using UnityEngine;
using System.Collections;
namespace game
{
    [CreateAssetMenu]
    public class CombatAbilityPrototype : ScriptableObject
    {
        public string id;
        public string title { get { return id; } }
        public string desc { get { return id + "_desc"; } }

        public bool unlockedByDefault;
        public int minAvailableLevel = 0;
        public bool isBackup;
        public int intValue;
        public bool hasIntValue = true;

        public Sprite sp;

        public enum FxType
        {
            None,
            Buff,
            Heal,
            Block,
        }
        public FxType fxType;
    }
}
