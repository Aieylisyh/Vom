using UnityEngine;
using com;
using vom;

namespace game
{
    public class ConfigService : MonoBehaviour
    {
        public static ConfigService instance { get; private set; }

        public AdConfig adConfig;
        public EcsConfig ecsConfig;
        public InputConfig inputConfig;
        public NetworkConfig networkConfig;
        public PayConfig payConfig;
        public AchievementConfig achievementConfig;
        public TalentConfig talentConfig;
        public MailConfig mailConfig;
        public CombatAbilityConfig combatAbilityConfig;
     
        public EnemyConfig enemyConfig;
        public CoreConfig coreConfig;
        public SettingsConfig settingsConfig;
        public ItemConfig itemConfig;
        public MissionConfig missionConfig;
        public TutorialConfig tutorialConfig;
        public LevelConfig levelConfig;
        public LocalRemoteConfig localRemoteConfig;
        public ShopsConfig shopsConfig;
        public PediaConfig pediaConfig;

        public CombatConfig combatConfig;
        public MapConfig mapConfig;
        public SceneInteractionConfig sceneInteractionConfig;
        public CraftConfig craftConfig;
        public InventoryConfig inventoryConfig;
        public LootConfig lootConfig;
        public ItemsConfig itemsConfig;

        void Awake()
        {
            instance = this;
            InitConfig();
        }

        public void InitConfig()
        {
            itemConfig.Sort();
        }
    }
}