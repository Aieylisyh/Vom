using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ActivityPopup : PopupBehaviour
    {
        public Text title;
        public Text content;
        public GameObject btnClaim;
        public GameObject btnClose;
        public Text btnClaimContentTxt;
        public Text btnClaimTxt;

        public void Setup()
        {
        }

        public virtual void OnClickBtnClaim()
        {
            Debug.Log("OnClickBtnClaim");
            Sound();
        }
        public virtual void OnClickBtnDelete()
        {
        }
    }
}
