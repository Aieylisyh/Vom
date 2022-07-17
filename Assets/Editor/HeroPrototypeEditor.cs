using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System;

namespace vom
{
    [CustomEditor(typeof(HeroPrototype))]
    public class HeroPrototypeEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("attributesList to dic"))
            {
                HeroPrototype t = target as HeroPrototype;
                if (t.attributes.atbs != null)
                    t.attributes.atbs.Clear();
                t.attributes.atbs = new Dictionary<string, int>();

                foreach (var i in t.attributesList)
                {
                    Debug.Log(i.id);
                    if (t.attributes.atbs.ContainsKey(i.id))
                    {
                        t.attributes.atbs[i.id] = t.attributes.atbs[i.id] + i.v;
                    }
                    else
                    {
                        t.attributes.atbs[i.id] = i.v;
                    }
                }
            }

            if (GUILayout.Button("test + operator"))
            {
                HeroPrototype t = target as HeroPrototype;
                var attriLayer = t.attributes;

                var a = attriLayer + attriLayer + attriLayer;
                var b = attriLayer + attriLayer;
                var c = +attriLayer;

                Debug.Log("--a--");
                foreach (var kv in a.atbs)
                {
                    Debug.Log(kv.Key + " " + kv.Value);
                }
                Debug.Log("--b--");
                foreach (var kv in b.atbs)
                {
                    Debug.Log(kv.Key + " " + kv.Value);
                }
                Debug.Log("--c--");
                foreach (var kv in c.atbs)
                {
                    Debug.Log(kv.Key + " " + kv.Value);
                }
            }
        }
    }
}