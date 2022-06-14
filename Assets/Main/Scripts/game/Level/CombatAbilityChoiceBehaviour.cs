using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class CombatAbilityChoiceBehaviour : MonoBehaviour
    {
        public Image img;
        public Text titleTxt;
        public GameObject infoButton;
        public GameObject title;
        public Transform main;
        public CombatAbilityPrototype proto { get; private set; }

        public void Show(string s, float delay)
        {
            proto = CombatAbilityService.instance.GetPrototype(s);
            img.sprite = proto.sp;
            titleTxt.text = LocalizationService.instance.GetLocalizedText(proto.title);

            infoButton.SetActive(false);
            title.SetActive(true);
            main.gameObject.SetActive(true);
            main.localScale = Vector3.zero;
            main.DOKill();
            main.DOScale(1, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        }

        public void ShowInfo()
        {
            infoButton.SetActive(true);
        }

        public void HideInfo()
        {
            infoButton.SetActive(false);
        }

        public void SelectHide()
        {
            main.gameObject.SetActive(false);
        }

        public void SelectedFeedback(System.Action action)
        {
            HideInfo();
            //vibrato: shake time
            //elastic: shake amplitude
            main.DOPunchScale(Vector3.one * 1.08f, 0.7f, 4, 0.1f).OnComplete(() => { action?.Invoke(); });
        }

        public void OnClickInfo()
        {
            WindowService.instance.ShowCombatAbilityPopup(proto, false, null);
        }
    }
}

