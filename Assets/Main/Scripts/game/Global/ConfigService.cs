using UnityEngine;
using com;

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
        public ShipConfig factoryConfig;
        public TalentConfig talentConfig;
        //public TechniqueConfig techniqueConfig;
        public FishingConfig fishingConfig;
        public MailConfig mailConfig;
        public CombatAbilityConfig combatAbilityConfig;
        public CombatConfig combatConfig;
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