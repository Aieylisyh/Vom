using System.Collections.Generic;
using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        public  List<EnemyPrototype> list = new List<EnemyPrototype>();
    }
}