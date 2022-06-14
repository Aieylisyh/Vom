using UnityEngine;

namespace game
{
    public class MissionPanelBehaviour : MonoBehaviour
    {
        public static MissionPanelBehaviour instance { get; private set; }

        public MissionPanelItemBehaviour ml;
        public MissionPanelItemBehaviour dl;

        private void Awake()
        {
            instance = this;
        }

        public void SetDl(string ctt, int crtQuota, int quota, bool done)
        {
            //Debug.Log("SetDl" + ctt + " " + crtQuota + "/" + quota);
            Set(dl, ctt, crtQuota, quota, done);
        }

        public void SetMl(string ctt, int crtQuota, int quota, bool done)
        {
            //Debug.Log("SetMl" + ctt + " " + crtQuota + "/" + quota);
            Set(ml, ctt, crtQuota, quota, done);
        }

        public void HideDl()
        {
            //Debug.Log("HideDl");
            Hide(dl);
        }

        public void HideMl()
        {
            //Debug.Log("HideMl");
            Hide(ml);
        }

        void Hide(MissionPanelItemBehaviour mptb)
        {
            mptb.cg.alpha = 0;
            mptb.cg.interactable = false;
            mptb.cg.blocksRaycasts = false;
        }

        void Set(MissionPanelItemBehaviour mptb, string ctt, int crtQuota, int quota, bool done)
        {
            mptb.cg.alpha = 1;
            mptb.cg.interactable = true;
            mptb.cg.blocksRaycasts = true;
            mptb.Set(ctt, crtQuota, quota, done);
        }

        public void OnClickMainline()
        {
            //Debug.Log("OnClickMainline");
            MissionService.instance.TryCommitMainline();
        }

        public void OnClickDaily()
        {
            //Debug.Log("OnClickDaily");
            MissionService.instance.TryCommitDaily();
        }
    }
}