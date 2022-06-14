using UnityEngine;
using com;

namespace game
{
    public class ActivableSpecialModule : MonoBehaviour
    {
        public enum SpecialModule
        {
            GameObjectActive,
            RotatePlay,
        }

        public SpecialModule specialModule;
        public GameObject go;
        public RotateBehaviour rb;

        public void ToggleSpecialModule(bool active)
        {
            switch (specialModule)
            {
                case SpecialModule.GameObjectActive:
                    go.SetActive(active);
                    break;
                case SpecialModule.RotatePlay:
                    if (active)
                    {
                        rb.Play();
                    }
                    else
                    {
                        rb.Stop();
                    }
                    break;
            }
        }

    }
}
