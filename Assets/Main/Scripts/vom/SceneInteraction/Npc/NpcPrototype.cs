using UnityEngine;

namespace vom
{
    public enum NpcService
    {
        None,
        Sell,
        Quest,
        AdOffer,
    }

    [System.Serializable]
    public class NpcPrototype
    {
        public string name;
        public NpcDialogSet dialogSet;
        public NpcService service;

        public class NpcDialogSet
        {
            public string[] salute;
            public string[] bye;
        }
    }
}