using UnityEngine;

namespace vom
{
    public enum ESceneInteraction
    {
        None,
        Tree,
        Loot,
    }

    public class SceneInteractionTargetBehaviour : MonoBehaviour
    {
        public ESceneInteraction interaction;

        [HideInInspector]
        public SceneInteractionUiBehaviour ui;

        public void HideUi()
        {
            if (ui != null)
            {
                ui.Remove();
            }
        }
    }
}