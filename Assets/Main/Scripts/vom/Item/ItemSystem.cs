using UnityEngine;
using System.Collections.Generic;

namespace vom
{
    public class ItemSystem : MonoBehaviour
    {
        public static ItemSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }
    }
}