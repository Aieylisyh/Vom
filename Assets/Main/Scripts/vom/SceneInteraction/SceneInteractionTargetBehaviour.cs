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

        public SceneInteractionUiBehaviour ui { get; private set; }

        public SceneInteractionData data { get; private set; }

        public GameObject targetItem;

        public GameObject vfx;

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
            if (ui != null)
            {
                ui.Remove();
                ui = null;
            }

            if (targetItem == null)
            {
                targetItem = gameObject;
            }

            switch (interaction)
            {
                case ESceneInteraction.None:
                    return;

                case ESceneInteraction.Tree:
                    Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
                    LootSystem.instance.Spawn(transform.position, new ItemData((int)data.baseAmount, "wood"), 0);
                    LootSystem.instance.Spawn(transform.position, new ItemData((int)data.baseAmount, "wood"), 1);
                    LootSystem.instance.Spawn(transform.position, new ItemData((int)data.baseAmount, "wood"), 2);
                    Destroy(targetItem);
                    return;
            }
        }
    }
}