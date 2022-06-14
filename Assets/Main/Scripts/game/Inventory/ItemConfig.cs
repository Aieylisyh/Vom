using System.Collections.Generic;
using UnityEngine;
using com;

namespace game
{
    [CreateAssetMenu]
    public class ItemConfig : ScriptableObject
    {
        //global config  not a perticular item
        public List<ItemPrototype> list = new List<ItemPrototype>();

        public List<ItemPrototype> commodityList = new List<ItemPrototype>();

        public List<ComplexItemPrototype> complexItemList = new List<ComplexItemPrototype>();

        public SpeicalItemConfig speicalItemConfig;

        public void Sort()
        {
            list.Sort(CompareItem);
            //commodityList.Sort(CompareItem);//dont do this!
        }

        private static int CompareItem(ItemPrototype x, ItemPrototype y)
        {
            if (x.sortWeight1 > y.sortWeight1)
            {
                return 1;
            }
            if (x.sortWeight1 < y.sortWeight1)
            {
                return -1;
            }
            if (x.sortWeight2 > y.sortWeight2)
            {
                return 1;
            }
            if (x.sortWeight2 < y.sortWeight2)
            {
                return -1;
            }
            return 0;
        }

        public List<Item> ParseComplexItem(string id, int amount)
        {
            if (id == "tokens")
            {
                List<Item> tokens = new List<Item>();
                tokens.Add(new Item(amount, UxService.instance.GetEventTokenId()));
                return tokens;
            }

            var res = new List<Item>();
            var ci = getComplexItem(id);
            if (ci == null)
            {
                return res;
            }

            List<int> picks = new List<int>();
            foreach (var i in ci.list)
            {
                picks.Add(i.n);
            }

            var resAllo = ListUtil.FastRandomAllocate(amount, picks);
            for (int i = 0; i < resAllo.Count; i++)
            {
                if (resAllo[i]>0)
                {
                    res.Add(new Item(resAllo[i], ci.list[i].id));
                }
            }

            return res;
        }

        public ComplexItemPrototype getComplexItem(string id)
        {
            foreach (var ci in complexItemList)
            {
                if (ci.complexId == id)
                {
                    return ci;
                }
            }
            return null;
        }

        [System.Serializable]
        public class ComplexItemPrototype
        {
            public string complexId;
            public List<Item> list;
        }
    }
}