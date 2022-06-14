using com;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ShipAbilitySlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public GameObject newAbilityPlus;
        public GameObject checkMark;
        public GameObject view;
        public float imageSize = 72;
        public ShipPrototype.ShipAbilityUnlockPrototype proto;

        public enum State
        {
            None,
            Next,
            Unlockable,
            Unlocked,
        }

        private State state;

        public void SetState(State pState)
        {
            state = pState;
            //Debug.Log("SetState " + pState);
            if (state == State.None)
            {
                if (view.activeSelf)
                {
                    view.SetActive(false);
                }
            }
            else
            {
                if (!view.activeSelf)
                {
                    view.SetActive(true);
                }

                icon.enabled = true;
                icon.sprite = proto.ability.sp;
                icon.rectTransform.sizeDelta = new Vector2(imageSize, imageSize);
                if (state == State.Unlocked)
                {
                    icon.color = Color.white;
                    newAbilityPlus.SetActive(false);
                    checkMark.SetActive(true);
                }
                else
                {
                    checkMark.SetActive(false);
                    if (state == State.Next)
                    {
                        icon.color = Color.grey;
                        newAbilityPlus.SetActive(false);
                    }
                    else if (state == State.Unlockable)
                    {
                        icon.color = Color.grey;
                        newAbilityPlus.SetActive(true);
                    }
                }
            }
        }

        public void OnClick()
        {
            //Debug.Log("OnClick " + state);
            if (state == State.None)
                return;

            SoundService.instance.Play("btn info");
            WindowService.instance.ShowShipAbilityPopup(proto, this);
        }
    }
}
