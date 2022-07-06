using UnityEngine;

namespace vom
{
    public class HeroSystem : MonoBehaviour
    {
        public static HeroSystem instance { get; private set; }

        public HeroData debugHeroData;
        public HeroData debugHeroData_1;
        public HeroData debugHeroData_2;

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