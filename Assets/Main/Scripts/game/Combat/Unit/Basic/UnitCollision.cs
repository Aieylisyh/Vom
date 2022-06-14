using UnityEngine;

namespace game
{
    public class UnitCollision : UnitComponent
    {
        public CollisionFlag targetFlag;
        public CollisionFlag selfFlag;
        public bool activeCollisionTest;
        public bool attackOnOtherUcIsCollided;

        [System.Flags]
        public enum CollisionFlag
        {
            Player = 1 << 0,
            PlayerProjectile = 1 << 1,
            // Allies = Player | PlayerProjectile,//这样就可以实现枚举多选
            Enemy = 1 << 2,
            EnemyProjectile = 1 << 3,
            //Enemies = Enemy | EnemyProjectile,//这样就可以实现枚举多选
            Killzone = 1 << 4,
        }

        public void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered(other);
        }

        protected virtual void OnTriggerEntered(Collider other)
        {
            UnitCollision otherUc = other.gameObject.GetComponent<UnitCollision>();
            if (otherUc == null)
            {
                return;
            }

            if (selfFlag == CollisionFlag.Killzone && otherUc.selfFlag != CollisionFlag.Killzone)
            {
                otherUc.OnCollidedKillzone();
                return;
            }

            if (activeCollisionTest && otherUc.selfFlag != 0 && targetFlag.HasFlag(otherUc.selfFlag))
            {
                //Debug.Log(selfFlag + " is Collided by " + uc.selfFlag);
                OnCollided(otherUc.self);
                otherUc.OnCollided(self);
                if (attackOnOtherUcIsCollided)
                {
                    self.attack.Attack();
                }
            }
        }
        protected virtual void OnCollided(Unit target)
        {

        }

        protected virtual void OnCollidedKillzone()
        {
            self.Recycle();
        }
    }
}
