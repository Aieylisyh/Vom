using UnityEngine;
using com;
using game;

namespace vom
{
    public class SceneInteractionSystem : MonoBehaviour
    {
        public static SceneInteractionSystem instance { get; private set; }

        public SceneInteractionUiBehaviour prefabUi;
        public Transform uiParent;

        private void Awake()
        {
            instance = this;
        }

        public static SceneInteractionData GetData(ESceneInteraction type)
        {
            var cfg = ConfigService.instance.sceneInteractionConfig;
            foreach (var si in cfg.interactionDatas)
            {
                if (si.type == type)
                    return si;
            }

            return null;
        }

        public static SceneInteractionData GetData(SceneInteractionTargetBehaviour host)
        {
            return GetData(host.interaction);
        }

        public SceneInteractionUiBehaviour CreateUi(SceneInteractionTargetBehaviour host, SceneInteractionData data)
        {
            var ui = Instantiate<SceneInteractionUiBehaviour>(prefabUi, uiParent);
            ui.Init(host, data);

            return ui;
        }
    }
}