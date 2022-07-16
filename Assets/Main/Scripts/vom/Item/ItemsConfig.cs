using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace vom
{
    [CreateAssetMenu]
    public class ItemsConfig : ScriptableObject
    {
        public List<ItemPrototype> items;

        public void Sort()
        {
            items.Sort(CompareItem);
        }

        int CompareItem(ItemPrototype a, ItemPrototype b)
        {
            if (a.sortWeight1 > b.sortWeight1)
            {
                return 1;
            }
            else if (a.sortWeight1 < b.sortWeight1)
            {
                return -1;
            }
            if (a.sortWeight2 > b.sortWeight2)
            {
                return 1;
            }
            else if (a.sortWeight2 < b.sortWeight2)
            {
                return -1;
            }

            return 0;
        }
    }

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