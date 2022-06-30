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
                    var go = Instantiate(vfx, transform.position, Quaternion.identity, MapSystem.instance.mapParent);
                    go.SetActive(true);
                    for (int i = 0; i < 5; i++)
                    {
                        LootSystem.instance.SpawnGold(transform.position, new ItemData((int)data.baseAmount, "wood"), i);
                    }
                    Destroy(targetItem);
                    return;
            }
        }
    }
}