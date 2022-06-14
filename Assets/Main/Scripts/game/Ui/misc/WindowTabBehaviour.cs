using UnityEngine;
using DG.Tweening;
using com;
using UnityEngine.UI;

namespace game
{
    public class WindowTabBehaviour : MonoBehaviour
    {
        public GameObject tabOn;
        public GameObject tabOff;

        public GameObject content;
        public Sprite spOn;
        public Sprite spOff;
        public Image img;
        public bool useSpriteSwap;

        public void On()
        {
            if (useSpriteSwap)
            {
                img.sprite = spOn;
            }
            else
            {
                tabOn.SetActive(true);
                tabOff.SetActive(false);
            }
            if (content != null)
            {
                content.SetActive(true);
            }

        }

        public void Off()
        {
            if (useSpriteSwap)
            {
                img.sprite = spOff;
            }
            else
            {
                tabOn.SetActive(false);
                tabOff.SetActive(true);
            }
            if (content != null)
            {
                content.SetActive(false);
            }
        }

        public void OnEnable()
        {

        }
    }
}