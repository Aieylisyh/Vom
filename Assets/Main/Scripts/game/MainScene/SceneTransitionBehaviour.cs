using com;
using UnityEngine;
//using DG.Tweening;

namespace game
{
    public class SceneTransitionBehaviour : MonoBehaviour
    {
        public GameObject[] transitionObjects;
        public Animator animator;
        public static SceneTransitionBehaviour instance;

        private void Awake()
        {
            instance = this;
        }

        public void StartTransition()
        {
            //Debug.Log("StartTransition");
            animator.SetTrigger("Transit");
            foreach (var g in transitionObjects)
            {
                g.SetActive(true);
            }
            //TODO tansition场景播放不同的声音（去upgrade是打铁 打仗是水泡等等）
            SoundService.instance.Play("transit1");
           
        }

        public void OnTransition()
        {
            //Debug.Log("OnTransition");
            CameraControllerBehaviour.instance.SwitchView();
            SoundService.instance.Play("transit2");
        }

        public void EndTransition()
        {
            //Debug.Log("EndTransition");
            animator.ResetTrigger("Transit");
            foreach (var g in transitionObjects)
            {
                g.SetActive(false);
            }
        }
    }
}
