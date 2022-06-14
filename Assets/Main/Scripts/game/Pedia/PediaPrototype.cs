using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class PediaPrototype : ScriptableObject
    {
        public string id
        {
            get
            {
                return eneProto.id;
            }
        }
        public float attackRate;
        public float hpRate;

        public EnemyPrototype eneProto;
    }
}