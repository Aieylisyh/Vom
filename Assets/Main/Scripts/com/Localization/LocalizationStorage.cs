using UnityEngine;
using System.Collections.Generic;

namespace com
{
    [System.Serializable, CreateAssetMenu]
    public class LocalizationStorage:ScriptableObject
    {
        public List<LocalizationElement> localizations;
    }
}