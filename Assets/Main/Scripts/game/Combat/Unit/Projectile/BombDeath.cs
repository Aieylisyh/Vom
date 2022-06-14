using UnityEngine;

namespace game
{
    public class BombDeath : ProjectileDeath
    {
        protected override void DieUnsilent()
        {
            (self as Bomb).TryCreateShards();
            base.DieUnsilent();
        }
    }
}