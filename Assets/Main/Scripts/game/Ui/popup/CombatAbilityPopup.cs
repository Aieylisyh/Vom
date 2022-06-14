using System;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class CombatAbilityPopup : PopupBehaviour
    {
        public Text titleTxt;
        public Text stateTxt;
        public Text descTxt;
        public Image icon;
        private CombatAbilityPrototype _proto;
        private Action _actionClosed;

        public void Setup(CombatAbilityPrototype proto, bool isNewUnlocked, Action action)
        {
            _proto = proto;
            _actionClosed = action;
            icon.sprite = proto.sp;
            titleTxt.text = LocalizationService.instance.GetLocalizedText(proto.title);

            var stateKey = isNewUnlocked ? "CombatAbilityUnlocked" : "CombatAbility";
            stateTxt.text = LocalizationService.instance.GetLocalizedText(stateKey);

            if (proto.hasIntValue)
            {
                descTxt.text = LocalizationService.instance.GetLocalizedTextFormatted(proto.desc, proto.intValue);
            }
            else
            {
                descTxt.text = LocalizationService.instance.GetLocalizedText(proto.desc);
            }
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            _actionClosed?.Invoke();
        }
    }
}
