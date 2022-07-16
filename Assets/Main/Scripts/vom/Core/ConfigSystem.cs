using UnityEngine;
using com;

namespace vom
{
    public class ConfigSystem : MonoBehaviour
    {
        public static ConfigSystem instance { get; private set; }

        public AdConfig adConfig;
        public NetworkConfig networkConfig;
        public PayConfig payConfig;
        public EnemyConfig enemyConfig;

        public CombatConfig combatConfig;
        public MapConfig mapConfig;
        public SceneInteractionConfig sceneInteractionConfig;
        public CraftConfig craftConfig;
        public InventoryConfig inventoryConfig;
        public LootConfig lootConfig;
        public ItemsConfig itemsConfig;
        public HeroConfig heroConfig;
        public DailyLevelConfig dailyLevelConfig;
        public SkillConfig skillConfig;
        public ToastConfig toastConfig;

        public SettingsConfig settingsConfig;
        public ShopsConfig shopsConfig;
        public TalentConfig talentConfig;
        public AttributesConfig attributesConfig;

        void Awake()
        {
            instance = this;
        }
    }
}