using UnityEngine;
using System.Collections.Generic;
using com;

namespace vom
{
    public class TutorialSystem : MonoBehaviour
    {
        public static TutorialSystem instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }
    }
}
