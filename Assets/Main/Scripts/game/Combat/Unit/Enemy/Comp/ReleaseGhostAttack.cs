using UnityEngine;
using com;

namespace game
{
    public class ReleaseGhostAttack : EnemyAttack
    {
        public bool randomizeGhostPosition;
        public float offset = 1.8f;

        protected override void LaunchAttack()
        {
            CreateGhost();
        }

        private void CreateGhost()
        {
        }
    }
}