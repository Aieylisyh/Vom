using UnityEngine;
using VacuumShaders.CurvedWorld;
using System.Collections.Generic;
using com;

namespace vom
{
    public class StartGameSystem : MonoBehaviour
    {
        public List<GameObject> gos;
        public CurvedWorld_Controller c;

        public static StartGameSystem instance { get; private set; }
        public float curvedX = -10;
        public float curvedZ = -10;

        private void Awake()
        {
            instance = this;

            foreach (var go in gos)
                go.SetActive(true);
        }

        void Start()
        {
            //RenderSettings.fog = true;
            c._V_CW_Bend_X = curvedX;
            c._V_CW_Bend_Z = curvedZ;

            TransitionBehaviour.instance.Opening(() =>
            {
                // ConfirmBoxPopup.ConfirmBoxData data = new ConfirmBoxPopup.ConfirmBoxData();
                // data.title = "What to Test";
                // data.content = "new bie\nVillage";
                // data.btnClose = false;
                // data.btnBgClose = true;
                // WindowService.instance.ShowConfirmBoxPopup(data);
                UxService.instance.StartLogin();
            });
        }
    }
}