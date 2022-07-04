using UnityEngine;
using UnityEngine.UI;

namespace vom
{
    public class EquipedSkillSlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public TMPro.TextMeshProUGUI text;
        public Image cdCover;
        public CanvasGroup cg;

        public void Init(SkillPrototype skl)
        {
            if (skl != null)
            {
                icon.sprite = skl.sp;
                text.text = skl.title;
                Sync(0);
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Sync(float cd)
        {
            cdCover.fillAmount = Mathf.Min(1, Mathf.Max(0, cd));
        }

        public void Shine()
        {
            Debug.Log("Shine");
        }

        void Show()
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        void Hide()
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
}
