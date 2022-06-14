
namespace game
{
    public enum DamageType
    {
        None,
        Torpedo,
        Bomb,
        Laser,
        Ghost,
    }


    [System.Serializable]
    public class Damage
    {
        public Unit origin { get; private set; }
        public int value { get; private set; }
        public DamageType type { get; private set; }
        public bool dealOnceOnly { get; private set; }
        public bool isBossKiller = false;

        private bool _validated = false;

        public Damage(Unit pOrigin, int pValue, DamageType pType, bool pDealOnceOnly)
        {
            Set(pOrigin, pValue, pType, pDealOnceOnly);
        }

        public void Set(Unit pOrigin, int pValue, DamageType pType, bool pDealOnceOnly)
        {
            origin = pOrigin;
            dealOnceOnly = pDealOnceOnly;
            type = pType;
            value = pValue;
            _validated = false;
        }

        public void ModifyPercentage(int p)
        {
            ModifyValue(value * (1f + p * 0.01f));
        }

        public void ModifyValue(float pValue)
        {
            ModifyValue(UnityEngine.Mathf.FloorToInt(pValue));
        }

        public void ModifyValue(int pValue)
        {
            value = UnityEngine.Mathf.Max(0, pValue);
        }

        public Damage GetAndValidateDamage()
        {
            if (_validated)
            {
                return null;
            }
            if (dealOnceOnly)
            {
                _validated = true;
            }
            return this;
        }
    }
}
