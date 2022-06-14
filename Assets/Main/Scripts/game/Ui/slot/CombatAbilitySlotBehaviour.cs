using UnityEngine;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class CombatAbilitySlotBehaviour : MonoBehaviour
    {
        public Image icon;
        private CombatAbilityPrototype _proto;
        private bool _isProceedOrPool;
        public float imageSize = 100;

        public void Setup(string id, bool isProceedOrPool)
        {
            _proto = CombatAbilityService.instance.GetPrototype(id);
            _isProceedOrPool = isProceedOrPool;
            if (_proto == null)
            {
                Debug.LogWarning(id);
                SetEmpty();
                return;
            }

            SetView(_proto.sp, isProceedOrPool);
        }

        private void SetView(Sprite sp, bool isProceedOrPool)
        {
            icon.enabled = true;
            icon.sprite = sp;
            icon.color = isProceedOrPool ? Color.white : Color.grey;
        }

        public void SetEmpty()
        {
            icon.enabled = false;
        }

        public void Awake()
        {
            icon.rectTransform.sizeDelta = new Vector2(imageSize, imageSize);
        }

        public void OnClick()
        {
            //Debug.Log("CombatAbilitySlotBehaviour OnClick");
            if (_proto == null)
                return;

            SoundService.instance.Play("btn info");
            WindowService.instance.ShowCombatAbilityPopup(_proto, false, null);
        }
    }
}
