using UnityEngine;

namespace vom
{
    public class SkillPanelBehaviour : MonoBehaviour
    {
        public static SkillPanelBehaviour instance { get; private set; }

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