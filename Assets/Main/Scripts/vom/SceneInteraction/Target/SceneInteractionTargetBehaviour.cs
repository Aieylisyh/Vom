using UnityEngine;

namespace vom
{
    public enum ESceneInteraction
    {
        None,
        Tree,
        Fruit,
        Chest,
        Fish,
        Herb,
        Mine,
        Dig,
    }

    public class SceneInteractionTargetBehaviour : MonoBehaviour
    {
        public ESceneInteraction interaction;

        public SceneInteractionUiBehaviour ui { get; private set; }

        public SceneInteractionData data { get; private set; }

        public GameObject targetItem;

        public GameObject vfx;

        public string startSound;

        public bool triggered { get; protected set; }

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

        public virtual bool TestCanInteract()
        {
            com.SoundService.instance.Play(startSound);
            return true;
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

                case ESceneInteraction.Dig:
                    (this as DigHoleBehaviour).FinishDig();
                    break;

                case ESceneInteraction.Mine:
                    (this as MineBehaviour).FinishMining();
                    break;

                case ESceneInteraction.Herb:
                    (this as HerbBehaviour).FinishHerbing();
                    break;

                case ESceneInteraction.Fish:
                    (this as FishingSpotBehaviour).FinishFishing();
                    break;
            }
        }
    }
}