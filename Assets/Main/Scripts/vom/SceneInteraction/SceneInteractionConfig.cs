using System.Collections.Generic;
using UnityEngine;

namespace vom
{
    [System.Serializable]
    public class SceneInteractionData
    {
        public ESceneInteraction type;
        public float duration;
        public float baseAmount;
        public UnityEngine.Sprite sp;
    }

    [CreateAssetMenu]
    public class SceneInteractionConfig : ScriptableObject
    {
        public List<SceneInteractionData> interactionDatas;
    }
}