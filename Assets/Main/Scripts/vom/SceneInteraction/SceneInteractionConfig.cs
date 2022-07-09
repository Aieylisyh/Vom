using System.Collections.Generic;
using UnityEngine;

namespace vom
{
    [System.Serializable]
    public class SceneInteractionData
    {
        public ESceneInteraction type;
        public float duration;
        public Sprite sp;
    }

    [CreateAssetMenu]
    public class SceneInteractionConfig : ScriptableObject
    {
        public List<SceneInteractionData> interactionDatas;

        public float treeHasFruitChance = 0.1f;
    }
}