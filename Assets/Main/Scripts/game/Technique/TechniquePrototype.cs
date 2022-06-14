using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class TechniquePrototype : ScriptableObject
    {
        public string id;
        public string desc;
        public TechniqueModuleModel defaultModule;
        public TechniqueModuleModel leftModule;
        public TechniqueModuleModel midModule;
        public TechniqueModuleModel rightModule;

        [System.Serializable]
        public class TechniqueModuleModel
        {
            public Sprite Sp;
            public string name;
            public string desc;
            public string desc_l;
            public GameObject prefab;
        }

        [System.Serializable]
        public struct TechniqueModuleModelSaveData
        {
            public int index;//0 none, 123 choosen
        }
    }
}