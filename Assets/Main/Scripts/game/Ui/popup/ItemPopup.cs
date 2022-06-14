
using UnityEngine;
using UnityEngine.UI;
using com;
using System.Collections.Generic;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ItemPopup : PopupBehaviour
    {
        public Text title;
        public Text desc;
        public Text subDesc;
        public SlotBehaviour slot;
        public Text btnTxt;
        private ItemPopupData _data;
        private ItemPrototype _proto;
        public struct ItemPopupData
        {
            public Item item;
            public bool enableUse;
        }

        public void Setup(ItemPopupData data)
        {
            _data = data;
            slot.Setup(data.item);

            _proto = ItemService.instance.GetPrototype(data.item.id, false);
            var localizedTitle = LocalizationService.instance.GetLocalizedText(_proto.title);
            title.text = localizedTitle;
            var localizedDesc = LocalizationService.instance.GetLocalizedText(_proto.desc);
            desc.text = localizedDesc;

            if (_proto.usage == ItemPrototype.Usage.Sell)
            {
                var prices = _proto.itemValue;
                subDesc.text = LocalizationService.instance.GetLocalizedText("SellWorth") + ": " + TextFormat.GetItemText(prices, true);
            }
            else if (_proto.usage == ItemPrototype.Usage.Craft)
            {
                var needs = _proto.itemValue;
                var key = "CraftDesc";
                var outputItemProto = ItemService.instance.GetPrototype(_proto.itemOutPut.id);
                var descCraftContent = TextFormat.GetItemText(_proto.itemOutPut, true) + "(" + LocalizationService.instance.GetLocalizedText(outputItemProto.title) + ")";
                desc.text = LocalizationService.instance.GetLocalizedTextFormatted(key, descCraftContent);
                subDesc.text = LocalizationService.instance.GetLocalizedText("CraftNeed") + ": " + TextFormat.GetItemText(needs, true);
            }
            else if (_proto.usage == ItemPrototype.Usage.Cab)
            {
                subDesc.text = LocalizationService.instance.GetLocalizedText(_proto.subDesc);
                var cabProto = CombatAbilityService.instance.GetPrototype(_proto.itemOutPut.id);
                if (cabProto.hasIntValue)
                    subDesc.text = LocalizationService.instance.GetLocalizedTextFormatted(_proto.subDesc, cabProto.intValue);
            }
            else
            {
                subDesc.text = LocalizationService.instance.GetLocalizedText(_proto.subDesc);
            }

            SetButton();
        }

        private void SetButton()
        {
            if (!_data.enableUse)
            {
                btnTxt.text = LocalizationService.instance.GetLocalizedText("OK");
                return;
            }

            switch (_proto.usage)
            {
                case ItemPrototype.Usage.None:
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("OK");
                    break;
                case ItemPrototype.Usage.CheckOut:
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("See");
                    break;
                case ItemPrototype.Usage.Sell:
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Sell");
                    break;
                case ItemPrototype.Usage.Open:
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Open");
                    break;
                case ItemPrototype.Usage.Consume:
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Use");
                    break;
                case ItemPrototype.Usage.Craft:
                    btnTxt.text = LocalizationService.instance.GetLocalizedText("Craft");
                    break;
            }
        }

        public void OnClickOkBtn()
        {
            Sound();
            //Debug.Log("item OnClickOkBtn " + _proto.usage);
            if (!_data.enableUse)
            {
                Hide();
                return;
            }

            //UxService.instance.LogItems();
            int amount = UxService.instance.GetItemAmount(_data.item.id);
            if (amount < 1)
            {
                Hide();
                return;
            }
            switch (_proto.usage)
            {
                case ItemPrototype.Usage.None:
                    Hide();
                    break;

                case ItemPrototype.Usage.CheckOut:
                    UseItem(1);
                    Hide();
                    break;

                case ItemPrototype.Usage.Craft:
                    int craftMaxAmount = ItemService.instance.GetCraftMaxAmount(_data.item.id);
                    int maxAmount = Mathf.Min(craftMaxAmount, amount);
                    //Debug.Log("Usage.Craft");
                    //Debug.Log("craftMaxAmount " + craftMaxAmount);
                    //Debug.Log("amount " + amount);
                    if (maxAmount == 0)
                    {
                        SoundService.instance.Play("btn info");
                        var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
                        confirmBoxData.btnClose = false;
                        confirmBoxData.btnBgClose = false;
                        confirmBoxData.btnLeft = true;
                        confirmBoxData.btnRight = false;
                        confirmBoxData.title = LocalizationService.instance.GetLocalizedText("Not success");
                        var localizedCraftMat = LocalizationService.instance.GetLocalizedText("CraftMat");
                        confirmBoxData.content = LocalizationService.instance.GetLocalizedTextFormatted("SomethingNotEnough", localizedCraftMat);
                        confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
                        WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
                        Hide();
                    }
                    else if (maxAmount == 1)
                    {
                        UseItem(1);
                        Hide();
                    }
                    else
                    {
                        SoundService.instance.Play("btn info");
                        WindowService.instance.ShowAmountSelectPopup(_data.item, this, maxAmount);
                    }
                    break;

                case ItemPrototype.Usage.Consume:
                case ItemPrototype.Usage.Open:
                case ItemPrototype.Usage.Sell:
                    if (amount == 1)
                    {
                        UseItem(1);
                        Hide();
                    }
                    else
                    {
                        WindowService.instance.ShowAmountSelectPopup(_data.item, this);
                    }
                    break;
            }
        }

        public void UseItem(int amount)
        {
            if (amount < 1)
                return;

            switch (_proto.usage)
            {
                case ItemPrototype.Usage.None:
                    break;

                case ItemPrototype.Usage.CheckOut:
                    if (_data.item.id == "Note")
                    {
                        SpecialItemService.instance.ShowNote();
                        MissionService.instance.PushMl("note", 1, true);
                    }
                    else if (_data.item.id == "Compass")
                    {
                        SpecialItemService.instance.ShowCompass();
                        MissionService.instance.PushMl("compass", 1, true);
                    }
                    else if (_data.item.id == "Clover")
                    {
                        SpecialItemService.instance.UseClover();
                        MissionService.instance.PushMl("clover", 1, true);
                    }
                    else if (_data.item.id == "Salary")
                    {
                        SpecialItemService.instance.UseSalary();
                        MissionService.instance.PushMl("salary", 1, true);
                    }
                    break;

                case ItemPrototype.Usage.Craft:
                    WaitingCircleBehaviour.instance.SetHideAction(() =>
                    {
                        var rewards = new List<Item>();
                        foreach (var price in _proto.itemValue)
                        {
                            UxService.instance.AddItem(price.id, -price.n * amount);
                        }
                        rewards.Add(new Item(_proto.itemOutPut.n * amount, _proto.itemOutPut.id));
                        UxService.instance.AddItem(_data.item.id, -amount);
                        ItemService.instance.GiveReward(rewards, false);
                        SetActiveParticleSystemsBehaviour.instance.ShowCraftEff();
                        WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
                    });
                    WaitingCircleBehaviour.instance.Show(0.6f);
                    break;

                case ItemPrototype.Usage.Consume:
                    if (_data.item.id == "Vip1" || _data.item.id == "Vip3" || _data.item.id == "Vip10")
                    {
                        int day = 1;
                        switch (_data.item.id)
                        {
                            case "Vip1":
                                day = 1;
                                break;
                            case "Vip3":
                                day = 3;
                                break;
                            case "Vip10":
                                day = 10;
                                break;
                        }
                        var days = day * amount;

                        WaitingCircleBehaviour.instance.SetHideAction(() =>
                        {
                            UxService.instance.AddRestTimeVip(days, 0, 0, 0);
                            SoundService.instance.Play("btn big");
                            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
                            confirmBoxData.btnClose = false;
                            confirmBoxData.btnBgClose = false;
                            confirmBoxData.btnLeft = true;
                            confirmBoxData.btnRight = false;
                            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("AddVipTitle");
                            confirmBoxData.content = LocalizationService.instance.GetLocalizedTextFormatted("AddVipDesc", days);
                            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
                            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
                        });
                        WaitingCircleBehaviour.instance.Show(0.6f);

                    }
                    else if (_data.item.id == "Course1" || _data.item.id == "Course2"
                        || _data.item.id == "Course3" || _data.item.id == "Course4" || _data.item.id == "Course5")
                    {
                        var tpCount = amount * _proto.itemOutPut.n;
                        var addTalentPoints = new List<Item>();
                        addTalentPoints.Add(new Item(tpCount, "TalentPoint"));
                        UxService.instance.AddItem("TalentPoint", tpCount);

                        SoundService.instance.Play("btn big");
                        var data = new ItemsPopup.ItemsPopupData();
                        data.clickBgClose = false;
                        data.hasBtnOk = true;
                        data.title = LocalizationService.instance.GetLocalizedText("UseCourseTitle");
                        data.content = LocalizationService.instance.GetLocalizedTextFormatted("UseCourseContent", tpCount);
                        data.items = addTalentPoints;
                        WindowService.instance.ShowItemsPopup(data);
                    }

                    UxService.instance.AddItem(_data.item.id, -amount);
                    break;

                case ItemPrototype.Usage.Open:
                    string complexItemId = "";

                    switch (_data.item.id)
                    {
                        case "Purse":
                            complexItemId = "openPurse";
                            break;
                        case "ChestGold":
                            complexItemId = "openChestGold";
                            break;
                        case "ChestDiamond":
                            complexItemId = "openChestDiamond";
                            break;
                    }

                    var parsedRes = ConfigService.instance.itemConfig.ParseComplexItem(complexItemId, amount);
                    var openRewards = new List<Item>();
                    foreach (var parsed in parsedRes)
                    {
                        if (parsed != null && parsed.id != "")
                        {
                            openRewards.Add(new Item(parsed.n, parsed.id));
                        }
                    }
                    UxService.instance.AddItem(_data.item.id, -amount);
                    ItemService.instance.GiveReward(openRewards, false);
                    SetActiveParticleSystemsBehaviour.instance.ShowOpenEff();
                    break;

                case ItemPrototype.Usage.Sell:
                    if (_proto.sellConfirm)
                    {
                        SellDoubleConfirm(() =>
                        {
                            ItemService.instance.SellItem(new Item(amount, _data.item.id), false);
                            UxService.instance.SaveGameItemData();
                            WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
                        }, TextFormat.GetItemText(new Item(amount, _data.item.id), true));
                    }
                    else
                    {
                        ItemService.instance.SellItem(new Item(amount, _data.item.id), false);
                    }
                    break;
            }

            UxService.instance.SaveGameItemData();
            WindowInventoryBehaviour.RefreshCurrentDisplayingInstances();
        }

        public void OnAmountSelected(int amount)
        {
            UseItem(amount);
            Hide();
        }

        void SellDoubleConfirm(System.Action cb, string itemString)
        {
            SoundService.instance.Play("btn info");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = true;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;

            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("SellDoubleConfirmTitle");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedTextFormatted("SellDoubleConfirmContent", itemString);

            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            confirmBoxData.btnLeftAction = cb;
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }
    }
}
