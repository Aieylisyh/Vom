using UnityEngine;

namespace vom
{
    public class InventoryService
    {
        public static int CompareItem(string x, string y)
        {
            return CompareItem(ItemService.GetPrototype(x), ItemService.GetPrototype(y));
        }

        public static int CompareItem(ItemData x, ItemData y)
        {
            return CompareItem(ItemService.GetPrototype(x.id), ItemService.GetPrototype(y.id));
        }

        public static int CompareItem(ItemPrototype x, ItemPrototype y)
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
    }
}