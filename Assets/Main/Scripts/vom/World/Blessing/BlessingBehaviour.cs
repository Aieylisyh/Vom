using UnityEngine;
using com;

namespace vom
{
    public class BlessingBehaviour : MonoBehaviour
    {
        public GameObject lit;
        public GameObject nolit;

        public void SetLit()
        {
            lit.SetActive(true);
            nolit.SetActive(false);
        }

        public void SetNoLit()
        {
            lit.SetActive(false);
            nolit.SetActive(true);
        }

        public void DoLit()
        {
            HeartDistortSystem.instance.Create(this.transform, 32, 5.5f);
            SetLit();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (CanLit())
                {
                    MmoCameraCinematicSystem.instance.LitBlessing(other.transform, transform, this);
                }
            }
        }

        bool CanLit()
        {
            if (lit.activeSelf)
                return false;

            return true;
        }
    }
}