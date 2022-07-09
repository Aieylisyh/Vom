using UnityEngine;

namespace vom
{
    public class ItemService
    {
        public static ItemsConfig itemsConfig
        {
            get
            {
                return game.ConfigService.instance.itemsConfig;
            }
        }

        public static ItemPrototype GetPrototype(string id)
        {
            foreach (var p in itemsConfig.items)
            {
                if (p.id == id)
                    return p;
            }

            return null;
        }

        public static Color GetColorByRarity(ItemPrototype.Rarity r)
        {
            switch (r)
            {
                case ItemPrototype.Rarity.Common:
                    return Color.white;

                case ItemPrototype.Rarity.Uncommon:
                    return new Color(0.25f, 1f, 0);

                case ItemPrototype.Rarity.Rare:
                    return new Color(0.25f, 0.25f, 1);

                case ItemPrototype.Rarity.Epic:
                    return new Color(1, 0.0f, 1);

                case ItemPrototype.Rarity.Legendary:
                    return new Color(1, 0.5f, 0);
            }

            return Color.white;
        }
    }
}