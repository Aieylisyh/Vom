using UnityEngine;
using com;
namespace game
{
    public class TorpedoAi : UnitAi
    {
        public override void ResetState()
        {
        }

        protected override void Tick()
        {
            Torpedo tor = self as Torpedo;
            if (tor.torType == Torpedo.TorType.Trace)
            {
                tor.torMove.SearchTarget();
            }
        }
    }
}
