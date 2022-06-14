using UnityEngine;
using System.Collections.Generic;
using com;

namespace game
{
    public class ShipModelSwitcher : MonoBehaviour
    {
        public IslandBehaviour islandBehaviour;

        public List<ShipModel> shipModels;
        private ShipModel _crtModel;

        public void SetModel(ShipModel sm)
        {
            if (_crtModel == sm)
            {
                return;
            }

            foreach (var m in shipModels)
            {
                if (m != sm)
                {
                    m.model.SetActive(false);
                    m.outline.OutlineWidth = 0;
                }
                else
                {
                    m.model.SetActive(true);
                    if (islandBehaviour.outline != null)
                    {
                        islandBehaviour.SetOutlineNone();
                    }

                    islandBehaviour.outline = m.outline;
                    islandBehaviour.SetOutlineNone();
                }
            }
        }

        public void SetModel(int index)
        {
            SetModel(shipModels[index]);
        }

        public void SetModel(string id)
        {
            foreach (var m in shipModels)
            {
                if (m.id == id)
                {
                    SetModel(m);
                    break;
                }
            }
        }
    }

    [System.Serializable]
    public class ShipModel
    {
        public GameObject model;
        public Outline outline;
        public string id;
    }
}
