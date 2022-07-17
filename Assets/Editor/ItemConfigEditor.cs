using UnityEngine;
using UnityEditor;

namespace vom
{
    [CustomEditor(typeof(ItemsConfig))]
    public class ItemsConfigEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Sort items"))
            {
                var cfg = (ItemsConfig)target;
                cfg.Sort();
            }
        }
    }
}