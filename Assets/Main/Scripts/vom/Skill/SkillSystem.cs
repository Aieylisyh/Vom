using UnityEngine;

namespace vom
{
    public class SkillSystem : MonoBehaviour
    {
        public static SkillSystem instance { get; private set; }

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