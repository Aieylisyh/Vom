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
            var res = CompareItem(ItemService.GetPrototype(x.id), ItemService.GetPrototype(y.id));
            if (res == 0)
            {
                if (x.n < y.n)
                    return 1;
                if (x.n > y.n)
                    return -1;
            }

            return res;
        }

        public static int CompareItem(ItemPrototype x, ItemPrototype y)
        {
            if (x.sortWeight1 > y.sortWeight1)
                return 1;
            if (x.sortWeight1 < y.sortWeight1)
                return -1;
            if (x.sortWeight2 > y.sortWeight2)
                return 1;
            if (x.sortWeight2 < y.sortWeight2)
                return -1;

            var hash1 = x.id.GetHashCode();
            var hash2 = y.id.GetHashCode();
            if (hash1 > hash2)
                return 1;
            if (hash1 < hash2)
                return -1;

            return 0;
        }
    }
}