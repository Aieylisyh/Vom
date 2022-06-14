using UnityEngine;
using game;

namespace com
{
    public class WorldService : MonoBehaviour
    {
        private GameObject _currentWorld;
        public static WorldService Instance;
        public Transform worldParent;
        void Awake()
        {
            Instance = this;
        }

        public void ClearWorld()
        {
            Destroy(_currentWorld);
            _currentWorld = null;
        }

        public void CreateWorld(GameObject prefab)
        {
            _currentWorld = Instantiate(prefab, worldParent);
        }
    }
}