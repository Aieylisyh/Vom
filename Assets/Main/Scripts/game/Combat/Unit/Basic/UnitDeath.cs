using com;

namespace game
{
    public class UnitDeath : UnitComponent
    {
        public bool isDead { get; protected set; }

        public override void ResetState()
        {
            base.ResetState();
            isDead = false;
        }

        public void Die(bool silent)
        {
            if (isDead)
                return;

            isDead = true;
            if (self.health)
                self.health.ClearHealth();

            if (silent)
                DieSilent();
            else
                DieUnsilent();
        }

        protected virtual void DieSilent()
        {
            //Debug.Log("DieSilent " + self.gameObject.name);
            self.Recycle();
        }

        protected virtual void DieUnsilent()
        {
            //Debug.Log("DieUnsilent " + self.gameObject.name);
            self.Recycle();
        }

        protected void SpawnEffect(string effectId)
        {
            if (string.IsNullOrEmpty(effectId))
                return;

            var go = PoolingService.instance.GetInstance(effectId);
            go.transform.position = transform.position;
        }
    }
}
