using com;
using System;
using UnityEngine;

namespace vom
{
    public class WindowSystem : MonoBehaviour
    {
        public static WindowSystem instance { get; private set; }

        //public MailBoxPopup mailBoxPopup;
        //public SettingsPopup settingsPopup;
        //public ConfirmBoxPopup confirmBoxPopup;
        //public ActivityPopup activityPopup;
        //public PausePopup pausedPopup;
        //public AmountSelectPopup amountSelectPopup;
        //public CommodityPopup commodityPopup;
        //public ShipAbilityPopup shipAbilityPopup;
        //public FishingAbilityPopup fishingAbilityPopup;
        //public CombatAbilityPopup combatAbilityPopup;
        //public CombatAbilitiesPopup combatAbilitiesPopup;
        //public TalentPopup talentPopup;
        //public ConfirmUpgradePopup confirmUpgradePopup;
        //public ConfirmUpgradeBoatPopup confirmUpgradeBoatPopup;
        //public PediaPanelPopup pediaPanelPopup;
        //public ItemPopup itemPopup;
        //public ItemsPopup itemsPopup;
        //
        //public WindowBehaviour map;
        //public WindowBehaviour town;
        //public WindowBehaviour merchant;
        //public WindowBehaviour ship;
        //public WindowBehaviour fishing;
        //public WindowBehaviour workshop;
        //public WindowBehaviour inventory;
        //public WindowBehaviour login;
        //public RoundEndWindowBehaviour roundEnd;

        private void Awake()
        {
            instance = this;
        }

        public void HideAllWindows()
        {
            //  login?.Hide();
            //  map?.Hide();
            //  town?.Hide();
            //  merchant?.Hide();
            //  ship?.Hide();
            //  fishing?.Hide();
            //  workshop?.Hide();
            //  inventory?.Hide();
            //  roundEnd?.Hide();
        }

        /*
        public void ShowConfirmBoxPopup(ConfirmBoxPopup.ConfirmBoxData data)
        {
            confirmBoxPopup.Setup(data);
            confirmBoxPopup.Show();
        }
        public void ShowMailBoxPopup()
        {
            mailBoxPopup.Setup();
            mailBoxPopup.Show();
        }
        public void ShowSettingsPopup()
        {
            settingsPopup.Setup(true);
            settingsPopup.Show();
        }
        public void ShowSettingsWithoutExitPopup()
        {
            settingsPopup.Setup(false);
            settingsPopup.Show();
        }
        public void ShowActivityPopup()
        {
            activityPopup.Setup();
            activityPopup.Show();
        }

        public void ShowPausedPopup()
        {
            if (GameFlowService.instance.windowState == GameFlowService.WindowState.Main
           || GameFlowService.instance.gameFlowEvent == GameFlowService.GameFlowEvent.GoToPort)
            {
                return;
            }
            pausedPopup.Setup();
            pausedPopup.Show();
        }

        public void ShowAmountSelectPopup(CommoditySlotData commodity, CommodityPopup p, int max)
        {
            amountSelectPopup.Setup(commodity, p, max);
            amountSelectPopup.Show();
        }

        public void ShowAmountSelectPopup(Item item, ItemPopup p)
        {
            amountSelectPopup.Setup(item, p);
            amountSelectPopup.Show();
        }

        public void ShowAmountSelectPopup(Item item, ItemPopup p, int max)
        {
            amountSelectPopup.Setup(item, p, max);
            amountSelectPopup.Show();
        }

        public void ShowCommodityPopup(CommodityPopup.CommodityPopupData data)
        {
            commodityPopup.Setup(data);
            commodityPopup.Show();
        }

        public void ShowItemPopup(ItemPopup.ItemPopupData data)
        {
            itemPopup.Setup(data);
            itemPopup.Show();
        }

        public void ShowItemsPopup(ItemsPopup.ItemsPopupData data)
        {
            itemsPopup.Setup(data);
            itemsPopup.Show();
        }

        public void ShowShipAbilityPopup(ShipPrototype.ShipAbilityUnlockPrototype data, ShipAbilitySlotBehaviour slot)
        {
            shipAbilityPopup.Setup(data, slot);
            shipAbilityPopup.Show();
        }

        public void ShowFishingAbilityPopup(FishingAbilityUnlockPrototype data, FishingAbilitySlotBehaviour slot)
        {
            fishingAbilityPopup.Setup(data, slot);
            fishingAbilityPopup.Show();
        }

        public void ShowTalentPopup(TalentPrototype data, TalentSlotBehaviour slot)
        {
            talentPopup.Setup(data, slot);
            talentPopup.Show();
        }

        public void ShowConfirmUpgradePopup()
        {
            confirmUpgradePopup.Setup();
            confirmUpgradePopup.Show();
        }

        public void ShowConfirmUpgradeBoatPopup()
        {
            confirmUpgradeBoatPopup.Setup();
            confirmUpgradeBoatPopup.Show();
        }

        public void ShowPediaPanelPopup()
        {
            pediaPanelPopup.Setup();
            pediaPanelPopup.Show();
        }

        public void ShowCombatAbilitiesPopup(bool isProceededOrPool)
        {
            combatAbilitiesPopup.Setup(isProceededOrPool);
            combatAbilitiesPopup.Show();
        }

        public void ShowCombatAbilityPopup(CombatAbilityPrototype combatAbilityPrototype, bool isProceededOrPool, Action action)
        {
            combatAbilityPopup.Setup(combatAbilityPrototype, isProceededOrPool, action);
            combatAbilityPopup.Show();
        }

        public void ShowMap()
        {
            map.Show();
        }

        public void ShowTown()
        {
            town.Show();
        }

        public void ShowMerchant()
        {
            merchant.Show();
        }

        public void ShowFishing()
        {
            fishing.Show();
        }

        public void ShowWorkshop()
        {
            workshop.Show();
        }

        public void ShowShip()
        {
            ship.Show();
        }

        public void ShowInventory()
        {
            inventory.Show();
        }

        public void ShowLogin()
        {
            login.Show();
        }

        public void ShowRoundEnd(bool winOrLoose)
        {
            roundEnd.ShowRoundEnd(winOrLoose);
        }
        */
    }
}