using UnityEngine;
using UnityEngine.UI;

namespace game
{
    public class LanguageOption : MonoBehaviour
    {
        public Image img;
        public void SetSp(Sprite sp)
        {
            img.sprite = sp;
        }
    }
}