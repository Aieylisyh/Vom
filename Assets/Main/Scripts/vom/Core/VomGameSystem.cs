using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class VomGameSystem : MonoBehaviour
    {
        public static VomGameSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            TransitionBehaviour.instance.Opening(() =>
            {
                // ConfirmBoxPopup.ConfirmBoxData data = new ConfirmBoxPopup.ConfirmBoxData();
                // data.title = "What to Test";
                // data.content = "new bie\nVillage";
                // data.btnClose = false;
                // data.btnBgClose = true;
                // WindowService.instance.ShowConfirmBoxPopup(data);
            });
        }
    }
}