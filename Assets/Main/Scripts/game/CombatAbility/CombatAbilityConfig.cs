using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class CombatAbilityConfig : ScriptableObject
    {
        public List<CombatAbilityPrototype> cabs = new List<CombatAbilityPrototype>();

        public List<ItemPrototype> cabItems = new List<ItemPrototype>();

        public CombatAbilityPrototype backupCab;
    }
}