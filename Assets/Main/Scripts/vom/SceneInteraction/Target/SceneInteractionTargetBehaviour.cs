using UnityEngine;

namespace vom
{
    public enum ESceneInteraction
    {
        None,
        Tree,
        Fruit,
        Chest,
    }

    public class SceneInteractionTargetBehaviour : MonoBehaviour
    {
        public ESceneInteraction interaction;

        public SceneInteractionUiBehaviour ui { get; private set; }

        public SceneInteractionData data { get; private set; }

        public GameObject targetItem;

        public GameObject vfx;

        public bool triggered { get; private set; }

        private void Start()
        {
            triggered = true;
            if (targetItem == null)
                targetItem = gameObject;
        }

        public void HideUi()
        {
            if (ui != null)
            {
                ui.Remove();
                ui = null;
            }
        }

        public void ShowUi()
        {
            if (ui == null)
            {
                data = SceneInteractionSystem.GetData(this);
                ui = SceneInteractionSystem.instance.CreateUi(this, data);
            }
        }

        public void OnFinish()
        {
            triggered = true;

            if (ui != null)
            {
                ui.Remove();
                ui = null;
            }

            switch (interaction)
            {
                case ESceneInteraction.None:
                    return;

                case ESceneInteraction.Tree:
                    (this as FruitTreeBehaviour).FinishChop();
                    return;

                case ESceneInteraction.Fruit:
                    (this as FruitTreeBehaviour).FinishFruit();
                    return;

                case ESceneInteraction.Chest:
                    (this as ChestBehaviour).Open();
                    return;
            }
        }
    }
}