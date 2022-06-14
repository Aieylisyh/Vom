using UnityEngine;
using System.Collections.Generic;

namespace game
{
    [CreateAssetMenu]
    public class ItemPrototype : ScriptableObject
    {
        public string id;
        public virtual string title { get { return id; } }
        public virtual string desc { get { return id + "_desc"; } }
        public virtual string subDesc { get { return id + "_subDesc"; } }

        public bool invHide;

        public Sprite sp;
        public List<Item> itemValue;

        public Item itemOutPut;
        public int intValue;
        public float floatValue;

        public int sortWeight1;
        public int sortWeight2;

        public bool sellConfirm;

        public Usage usage;
        public enum Usage
        {
            None,//no button
            CheckOut,//different function each
            Sell,//itemValue reward, show amount bar first
            Open,// lottery reward
            Consume,//use different function each
            //GotoWindow,
            Craft,//check craft and get reward
            Transaction_limited,//amount is the limit
            Transaction_unlimited,//show buy amount 
            Cab,//combatAbility
            Restock,
            Iap,
        }
    }
}