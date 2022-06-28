using UnityEngine;

namespace vom
{
    public class PlayerInteractionCheckBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var si = other.GetComponent<SceneInteractionTargetBehaviour>();
            if (si != null)
            {
                Debug.Log("enter " + si.interaction);
                var data = SceneInteractionSystem.GetData(si);
                SceneInteractionSystem.instance.CreateUi(si, data);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var si = other.GetComponent<SceneInteractionTargetBehaviour>();
            if (si != null)
            {
                Debug.Log("exit " + si.interaction);
                si.HideUi();
            }
        }
    }
}