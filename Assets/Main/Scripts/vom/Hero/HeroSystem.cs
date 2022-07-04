using UnityEngine;

namespace vom
{
    public class HeroSystem : MonoBehaviour
    {
        public static HeroSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}