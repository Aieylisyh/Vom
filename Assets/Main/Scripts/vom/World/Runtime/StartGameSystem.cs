using UnityEngine;
using VacuumShaders.CurvedWorld;

namespace vom
{
    public class StartGameSystem : MonoBehaviour
    {
        public CurvedWorld_Controller c;

        void Start()
        {
            //RenderSettings.fog = true;
            c._V_CW_Bend_X = -12;
            c._V_CW_Bend_Z = -12;
        }
    }
}