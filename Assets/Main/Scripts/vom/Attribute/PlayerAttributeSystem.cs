using UnityEngine;

namespace vom
{
    public enum PlayerAttributeLayer
    {
        Hero = 1,
        Talent = 10,
        Equipment = 15,
        Inventory = 20,
        DailyPerk = 25,
        Final = 30,
    }

    public class PlayerAttributeSystem : MonoBehaviour
    {
        public static PlayerAttributeSystem instance { get; private set; }

        public AttributeLayerData layerHero { get; private set; }
        public AttributeLayerData layerTalent { get; private set; }
        public AttributeLayerData layerEquipment { get; private set; }
        public AttributeLayerData layerInventory { get; private set; }
        public AttributeLayerData layerDailyPerk { get; private set; }
        public AttributeLayerData layerFinal { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void RefreshHeroLayer()
        {
            layerHero = HeroSystem.instance.GetCurrentHeroAttributes();
        }

        public void RefreshTalentLayer()
        {
            var layer = TalentSystem.instance.GetTalentAttributes();
            layerTalent = AttributeService.Merge(layerHero, layer);
        }

        public void RefreshEquipmentLayer()
        {
            var layer = TalentSystem.instance.GetTalentAttributes();//TODO
            layerEquipment = AttributeService.Merge(layerTalent, layer);
        }

        public void RefreshInventoryLayer()
        {
            var layer = TalentSystem.instance.GetTalentAttributes();//TODO
            layerInventory = AttributeService.Merge(layerEquipment, layer);
        }

        public void RefreshDailyPerkLayer()
        {
            var layer = TalentSystem.instance.GetTalentAttributes();//TODO
            layerDailyPerk = AttributeService.Merge(layerInventory, layer);
        }

        public void RefreshFinalLayer()
        {
            var layer = TalentSystem.instance.GetTalentAttributes();//TODO
            layerFinal = AttributeService.Merge(layerDailyPerk, layer);
        }

        public void SyncLayer(PlayerAttributeLayer layer)
        {
            switch (layer)
            {
                case PlayerAttributeLayer.Hero:
                    RefreshHeroLayer();
                    SyncLayer(PlayerAttributeLayer.Talent);
                    break;

                case PlayerAttributeLayer.Talent:
                    RefreshTalentLayer();
                    SyncLayer(PlayerAttributeLayer.Equipment);
                    break;

                case PlayerAttributeLayer.Equipment:
                    RefreshEquipmentLayer();
                    SyncLayer(PlayerAttributeLayer.Inventory);
                    break;

                case PlayerAttributeLayer.Inventory:
                    RefreshInventoryLayer();
                    SyncLayer(PlayerAttributeLayer.DailyPerk);
                    break;

                case PlayerAttributeLayer.DailyPerk:
                    RefreshDailyPerkLayer();
                    SyncLayer(PlayerAttributeLayer.Final);
                    break;

                case PlayerAttributeLayer.Final:
                    RefreshFinalLayer();
                    break;
            }
        }
    }
}