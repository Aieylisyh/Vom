using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class PediaConfig : ScriptableObject
    {
        public List<PediaPrototype> pedias;

        public Vector3 camPos = new Vector3(20f, 6.5f, -18f);
        public Vector3 camEular = new Vector3(-5, 0, 0);
        public float camSize = 6.2f;
        public float camSizeOffset = 0.7f;
        public float camSizeDuration = 5;

        public Vector3 enemyPos = new Vector3(20f, 6.7f, 0);
    }
}
