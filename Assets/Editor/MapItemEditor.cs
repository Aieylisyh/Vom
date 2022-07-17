using UnityEngine;
using UnityEditor;

namespace vom
{
    [CustomEditor(typeof(MapItem))]
    public class MapItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Init"))
            {
                var mapItem = (MapItem)target;
                foreach (var c in mapItem.connectors)
                {
                    c.fromId = mapItem.mapId;
                }

                if (mapItem.tiles.Count > 0)
                {
                    mapItem.sizeX = mapItem.tiles[mapItem.tiles.Count - 1].x + 1;
                    mapItem.sizeZ = mapItem.tiles[mapItem.tiles.Count - 1].z + 1;

                    for (var i = 0; i < mapItem.tiles.Count; i++)
                    {
                        var data = mapItem.tiles[i];
                        //data.h = (short)(mapItem.tiles[i].height * 100f);
                        if (data.h == -15)
                            data.h = -25;
                        mapItem.tiles[i] = data;
                    }
                }
            }
        }
    }
}