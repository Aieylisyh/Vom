using UnityEngine;

namespace vom
{
    public class HeroSystem : MonoBehaviour
    {
        public static HeroSystem instance { get; private set; }

        public HeroData debugHeroData;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {

        }

       public HeroData GetCurrentHeroData()
        {
            return debugHeroData;
        }
    }
}