using UnityEngine;
using System.Collections.Generic;
using com;
using game;

namespace vom
{
    public class MapService
    {
        public static MapItem GetMapItemById(string id)
        {
            var maps = ConfigSystem.instance.mapConfig.maps;
            foreach (var map in maps)
            {
                if (map.mapId == id)
                    return map;
            }

            return null;
        }
    }
}