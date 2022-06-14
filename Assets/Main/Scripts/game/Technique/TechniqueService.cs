using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class TechniqueService : MonoBehaviour
    {
        public static TechniqueService instance;
        private void Awake()
        {
            instance = this;
        }

        private List<TechniquePrototype> GetPrototypes()
        {
            //return ConfigService.instance.techniqueConfig.list;
            return null;
        }

        public TechniquePrototype GetPrototype(string id)
        {
            foreach (var i in GetPrototypes())
            {
                if (i.id == id)
                {
                    return i;
                }
            }
            return null;
        }
    }


}
