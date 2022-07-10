using UnityEngine;
using DG.Tweening;

namespace vom
{
    public class SetActiveParticleSystemsBehaviour : MonoBehaviour
    {
        public static SetActiveParticleSystemsBehaviour instance;

        public GameObject cloverEff;
        public GameObject salaryEff;
        public GameObject openEff;
        public GameObject craftEff;

        private void Awake()
        {
            instance = this;
        }

        public void ShowCloverEff()
        {
            ShowEff(cloverEff);
        }

        public void ShowSalaryEff()
        {
            ShowEff(salaryEff);
        }

        public void ShowOpenEff()
        {
            ShowEff(openEff);
        }

        public void ShowCraftEff()
        {
            ShowEff(craftEff);
        }

        void ShowEff(GameObject go)
        {
            if (go.activeSelf)
            {
                return;
            }

            go.SetActive(true);
            var seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() => { go.SetActive(false); });
        }
    }
}
