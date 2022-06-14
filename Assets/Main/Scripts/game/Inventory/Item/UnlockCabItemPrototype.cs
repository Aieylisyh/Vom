using UnityEngine;

namespace game
{
    [CreateAssetMenu]
    public class UnlockCabItemPrototype : ItemPrototype
    {
        public override string title { get { return itemOutPut.id; } }

        public override string desc { get { return "Unlock_cab_desc"; } }

        public override string subDesc { get { return itemOutPut.id + "_desc"; } }
    }
}
