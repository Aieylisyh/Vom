using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class CombatAbilitiesPopup : PopupBehaviour
    {
        public Text titleText;

        public CombatAbilityInventoryBehaviour cabInv;
        public ResizeRectTransform resizer;
        public GameObject infoBtn;
        public GameObject poolBtn;
        public GameObject proceededBtn;

        public void Setup(bool isProceededOrPool)
        {
            cabInv.isProceedOrPool = isProceededOrPool;
            cabInv.Refresh();
            var key = (isProceededOrPool ? "CombatAbilitySelected" : "CombatAbilityPool");
            titleText.text = LocalizationService.instance.GetLocalizedText(key);
            resizer.ResizeLater();

            poolBtn.SetActive(isProceededOrPool);
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Gameplay)
            {
                proceededBtn.SetActive(!isProceededOrPool);
            }
            else
            {
                proceededBtn.SetActive(false);
            }
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            if (cabInv.isProceedOrPool)
            {
                data.title = LocalizationService.instance.GetLocalizedText("CombatAbilitySelected");
                data.content = LocalizationService.instance.GetLocalizedText("CombatAbilitySelectedDesc");
            }
            else
            {
                data.title = LocalizationService.instance.GetLocalizedText("CombatAbilityPool");
                data.content = LocalizationService.instance.GetLocalizedText("CombatAbilityPoolDesc");
            }

            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickPool()
        {
            SoundService.instance.Play("btn info");
            Setup(false);
        }

        public void OnClickProceeded()
        {
            SoundService.instance.Play("btn info");
            Setup(true);
        }
    }
}
