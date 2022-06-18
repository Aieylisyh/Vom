using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class PortalBuilderBehaviour : MonoBehaviour
    {
        public GameObject core_normal;
        public GameObject core_actived;

        public List<SelfBuildPartBehaviour> parts;

        private bool built;

        private void Start()
        {
            built = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                ToggleBuild();
            }
        }

        void ToggleBuild()
        {
            if (built)
                UnBuild();
            else
                Build();
        }

        void Build()
        {
            built = true;
            foreach (var part in parts)
            {
                part.Build();
            }

            core_actived.SetActive(true);
            core_normal.SetActive(false);
            SoundService.instance.Play("module on");
        }

        void UnBuild()
        {
            built = false;
            foreach (var part in parts)
            {
                part.UnBuild();
            }

            core_actived.SetActive(false);
            core_normal.SetActive(true);
            SoundService.instance.Play("module on");
        }
    }
}