using UnityEngine;

namespace vom
{
    [CreateAssetMenu]
    public class ToastConfig : ScriptableObject
    {
        public float expandDuration = 0.5f;//bigger as it grows
        public float duration = 5f;

        public float heightPureText = 70;
        public float heightWithSp = 125;
    }
}