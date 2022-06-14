using UnityEngine;
using DG.Tweening;
using com;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ShipWindowBehaviour : WindowBehaviour
    {
        public WindowTabsBehaviour wtb;
        public Text shipLevelLabel;
        public Text shipNameLabel;
        public static ShipWindowBehaviour instance;
        public ShipAbilityPanelBehaviour abilityPanelBehaviour;
        public ShipAttributePanelBehaviour attributePanelBehaviour;

        public GameObject arrowLeft;
        public GameObject arrowRight;

        public GameObject infoBtn;
        public GameObject infoPanel;
        public Text infoDesc1;
        public Text infoDesc2;
        public ParticleSystem psSab;
        public ParticleSystem psLevelup;
        public ParticleSystem psUnlock;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Setup()
        {
            base.Setup();

            infoBtn.SetActive(true);
            infoPanel.SetActive(false);

            OnShowAttribute();
            UpdateShipView(true);
            SetWtbAmount();
            wtb.SetTab(0);
        }

        private void SetWtbAmount()
        {
            var levelPassIndex = LevelService.instance.GetNextCampaignLevelIndex();
            var cfg = ConfigService.instance.tutorialConfig;
            if (levelPassIndex >= cfg.minLevelIndexEnableFunctionsData.sabTab)
            {
                wtb.SetTo2TabsLayout(0, 1);
                return;
            }

            wtb.SetTo1TabLayout(0);
        }

        public void PlayEffectLevelup()
        {
            psLevelup.Play(true);
            PlayShipMeshAnim(false);
        }
        public void PlayEffectSab()
        {
            psSab.Play(true);
            PlayShipMeshAnim(true);
        }
        public void PlayEffectUnlock()
        {
            psUnlock.Play(true);
            PlayShipMeshAnim(true);
        }

        public override void OnClickBtnClose()
        {
            base.OnClickBtnClose();
            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            MainHudBehaviour.instance.RefreshToDefault();

            if (shipItem.saveData.unlocked)
            {
                CombatService.instance.playerShip.shipModelSwitcher.islandBehaviour.SetOutlineThin();
                return;
            }

            ChangeShip(ConfigService.instance.combatConfig.playerParam.defaultShipId);
            CombatService.instance.playerShip.shipModelSwitcher.islandBehaviour.SetOutlineThin();
        }

        public override void Hide()
        {
            base.Hide();
            wtb.Off();
        }

        public void OnClickNextShip()
        {
            Sound();
            var ships = ConfigService.instance.factoryConfig.ships;
            var index = GetCurrentShipIndex();
            index += 1;
            if (index >= ships.Count)
            {
                index -= ships.Count;
            }
            ChangeShip(index);
        }

        public void OnClickLastShip()
        {
            Sound();
            var ships = ConfigService.instance.factoryConfig.ships;
            var index = GetCurrentShipIndex();
            index -= 1;
            if (index < 0)
            {
                index += ships.Count;
            }
            ChangeShip(index);
        }

        public void RefreshShipSwitchArrow()
        {
            var index = GetCurrentShipIndex();
            //arrowLeft.SetActive(index != 0);
            //arrowLeft.SetActive(index != ships.Count - 1);
        }

        private int GetCurrentShipIndex()
        {
            var ships = ConfigService.instance.factoryConfig.ships;
            var index = 0;
            for (var i = 0; i < ships.Count; i++)
            {
                if (ships[i].id == ShipService.instance.currentShipId)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public void ChangeShip(int index)
        {
            var ships = ConfigService.instance.factoryConfig.ships;
            ChangeShip(ships[index].id);
        }

        public void ChangeShip(string id)
        {
            if (ShipService.instance.currentShipId == id)
            {
                return;
            }

            UxService.instance.gameDataCache.cache.currentShipId = id;
            UxService.instance.SaveGameData();
            SoundService.instance.Play("factory");

            CombatService.instance.CreatePlayerAttri();
            UpdateShipView(true);
        }

        public void UpdateShipView(bool changeShip = false)
        {
            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;
            var shipNameLocalized = LocalizationService.instance.GetLocalizedText(shipProto.title);
            if (changeShip)
            {
                CombatService.instance.playerShip.shipModelSwitcher.SetModel(ShipService.instance.currentShipId);
                shipNameLabel.text = shipNameLocalized;
            }
            CombatService.instance.playerShip.shipModelSwitcher.islandBehaviour.SetOutlineNone();
            shipLevelLabel.text = LocalizationService.instance.GetLocalizedTextFormatted("ShipLevel", shipLevel, shipNameLocalized);

            infoBtn.SetActive(true);
            infoPanel.SetActive(false);
            if (wtb.GetCurrentTab() == 1)
            {
                attributePanelBehaviour.Refresh();
            }
            else
            {
                abilityPanelBehaviour.Refresh();
            }

            MainHudBehaviour.instance.Refresh();
        }

        public void OnShowAttribute()
        {
            attributePanelBehaviour.Refresh();
        }
        public void OnShowAbility()
        {
            abilityPanelBehaviour.Refresh();
        }

        public void OnClickCloseInfo()
        {
            Sound();
            infoBtn.SetActive(true);
            infoPanel.SetActive(false);
        }

        public void OnClickInfo()
        {
            var shipProto = ShipService.instance.GetPrototype();
            infoDesc1.text = LocalizationService.instance.GetLocalizedText(shipProto.desc);
            infoDesc2.text = LocalizationService.instance.GetLocalizedText(shipProto.subDesc);

            infoBtn.SetActive(false);
            infoPanel.SetActive(true);
            SoundService.instance.Play("btn info");
        }

        public void PlayShipMeshAnim(bool strong)
        {
            var ship = CombatService.instance.playerShip.transform;
            ship.localScale = Vector3.one;
            ship.DOKill();
            if (strong)
            {
                ship.DOPunchScale(Vector3.one * 0.6f, 0.6f, 4, 1);
            }
            else
            {
                ship.DOPunchScale(Vector3.one * 0.15f, 0.35f, 1, 1);
            }
        }
    }
}