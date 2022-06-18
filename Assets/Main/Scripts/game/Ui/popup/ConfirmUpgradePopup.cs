using UnityEngine;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class ConfirmUpgradePopup : PopupBehaviour
    {
        public Text priceText;
        public Text attriText;
        public Text attriValueText;
        public Text shipNameText;
        private float _floatingTextY;

        public void Setup()
        {
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;

            var price = ShipService.instance.GetLevelupPrice(shipItem);
            priceText.text = TextFormat.GetItemText(price, true, true);
            var shipNameLocalized = LocalizationService.instance.GetLocalizedText(ShipService.instance.GetPrototype().title);
            //shipNameText.text = LocalizationService.instance.GetLocalizedTextFormatted("ShipLevel", shipLevel, shipNameLocalized);
            shipNameText.text = shipNameLocalized;
            DisplayAttributes();
        }

        public void OnClickBtnOk()
        {
            Sound();
            Hide();

            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;

            if (shipItem.saveData.unlocked)
            {
                if (ShipService.instance.IsLevelupPossible())
                {
                    var attri_shipLayer = CombatService.instance.playerAttri.vBase;

                    var hp1 = attri_shipLayer.hp;
                    var speed1 = attri_shipLayer.speed;
                    var armor1 = attri_shipLayer.armor;
                    var reg1 = attri_shipLayer.reg;
                    //var vita1 = attri_shipLayer.vita;
                    var torDmg1 = attri_shipLayer.torDmg;
                    var bombDmg1 = attri_shipLayer.bombDmg;

                    var suc = Levelup();
                    if (suc)
                    {
                        //ShipWindowBehaviour.instance.attributePanelBehaviour.OnLevelupSuc();
                        //ShipService.instance.CheckShipUpgradeMissions();

                        attri_shipLayer = CombatService.instance.playerAttri.vBase;
                        var hp2 = attri_shipLayer.hp;
                        var speed2 = attri_shipLayer.speed;
                        var armor2 = attri_shipLayer.armor;
                        var reg2 = attri_shipLayer.reg;
                        //var vita2 = attri_shipLayer.vita;
                        var torDmg2 = attri_shipLayer.torDmg;
                        var bombDmg2 = attri_shipLayer.bombDmg;
                        ShowAttriFloatText(hp2 - hp1, (int)(100 * (speed2 - speed1)), armor2 - armor1,
                         (int)(reg2 - reg1), //vita2 - vita1,
                            torDmg2 - torDmg1, bombDmg2 - bombDmg1);
                    }
                }
                else
                {
                    Debug.Log("ShipLevelMaximumReachedTitle ShipLevelMaximumReachedContent");
                }
            }
            else
            {
                Debug.Log("locked!");
            }
        }

        private void ShowAttriFloatText(params int[] arguments)
        {
            _floatingTextY = 0.66f;
            string s = "<size=90%>";
            ConcatAttriFloatText(ref s, "Hp", arguments[0]);
            ConcatAttriFloatText(ref s, "Speed", arguments[1]);
            ConcatAttriFloatText(ref s, "Armor", arguments[2]);
            ConcatAttriFloatText(ref s, "Reg", arguments[3]);
            //ConcatAttriFloatText(ref s, "Vita", arguments[4]);
            ConcatAttriFloatText(ref s, "Torpedo Damage", arguments[4]);
            ConcatAttriFloatText(ref s, "Bomb Damage", arguments[5]);
            s = s + "</size>";

            var x = Random.Range(0.42f, 0.58f);

            FloatingTextPanelBehaviour.instance.Create(s, x, _floatingTextY);
        }

        private void ConcatAttriFloatText(ref string s, string key, int delta)
        {
            if (delta <= 0)
                return;

            s = s + (string.IsNullOrEmpty(s) ? "" : "\n") + GetAttriLocalizedLabel(key) + "+" + delta;
        }

        private string GetAttriLocalizedLabel(string prefixCode)
        {
            return LocalizationService.instance.GetLocalizedText(prefixCode);
        }

        private bool Levelup()
        {
            //Debug.Log("try Levelup");
            var res = ShipService.instance.IsLevelupAffordable(true);
            if (res)
                return true;

            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");
            data.title = LocalizationService.instance.GetLocalizedText("LevelupNonAffordableTitle");
            data.content = LocalizationService.instance.GetLocalizedText("LevelupNonAffordableContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
            return false;
        }

        private string GetAttriText(string prefixCode, float value1, float value2, bool addReturn = true, string color1 = "BBBBBB", string color2 = "FFFF99")
        {
            int v1 = (int)value1;
            int v2 = (int)value2;
            if (v1 == v2)
                return "";

            var res = "<size=85%> " + GetAttriLocalizedLabel(prefixCode) + "</size>\t\t<color=#" + color1 + ">";
            res += v1 + "</color>";
            res += "-> <color=#" + color2 + "><size=110%> ";
            res += (int)v2 + "</size></color>";

            if (addReturn)
                res += "\n";

            return res;
        }

        private void SetAttriText(out string titleText, out string valueText, string prefixCode, float value1, float value2, bool addReturn = true, string color1 = "BBBBBB", string color2 = "FFFF99")
        {
            titleText = "";
            valueText = "";
            int v1 = (int)value1;
            int v2 = (int)value2;
            if (v1 == v2)
                return;

            titleText = GetAttriLocalizedLabel(prefixCode) + ":";
            valueText = " <color=#" + color1 + ">" + v1 + "</color> -> <color=#" + color2 + ">" + v2 + "</color>";

            if (addReturn)
            {
                titleText += "\n";
                valueText += "\n";
            }
        }

        private void DisplayAttributes()
        {
            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;

            CombatService.instance.RefreshPlayerAttri();
            var attri_1 = CombatService.instance.playerAttri.vBase;

            shipItem.saveData.level = shipLevel + 1;

            CombatService.instance.RefreshPlayerAttri();
            var attri_2 = CombatService.instance.playerAttri.vBase;

            shipItem.saveData.level = shipLevel;
            CombatService.instance.RefreshPlayerAttri();

            //var attri_lv_text = GetAttriText("ShipLv", shipLevel, shipLevel + 1, true, "BBBBBB", "66FFAA");
            //var attri_hp_text = GetAttriText("Hp", attri_1.hp, attri_2.hp, true, "BBBBBB", "CCFFAA");
            //var attri_speed_text = GetAttriText("Speed", attri_1.speed * 100, attri_2.speed * 100, true, "BBBBBB", "AACCFF");
            //var attri_reg_text = GetAttriText("Reg", attri_1.reg, attri_2.reg, true, "BBBBBB", "FFFFAA");
            //var attri_bomb_text = GetAttriText("Bomb Damage", attri_1.bombDmg, attri_2.bombDmg, true, "BBBBBB", "FFCCAA");
            //var attri_tor_text = GetAttriText("Torpedo Damage", attri_1.torDmg, attri_2.torDmg, false, "BBBBBB", "FFCCAA");
            //attriValueText.text = attri_lv_text + "\n" + attri_hp_text + attri_reg_text + attri_speed_text + attri_bomb_text + attri_tor_text;
            //attriText.text = attri_lv_text + "\n" + attri_hp_text + attri_reg_text + attri_speed_text + attri_bomb_text + attri_tor_text;

            string attri_lv_title, attri_hp_title, attri_speed_title, attri_reg_title, attri_bomb_title, attri_tor_title;
            string attri_lv_value, attri_hp_value, attri_speed_value, attri_reg_value, attri_bomb_value, attri_tor_value;
            SetAttriText(out attri_lv_title, out attri_lv_value, "ShipLv", shipLevel, shipLevel + 1, true, "C0FFD0", "77FFAA");
            SetAttriText(out attri_hp_title, out attri_hp_value, "Hp", attri_1.hp, attri_2.hp, true, "C0C0C0", "CCFFAA");
            SetAttriText(out attri_speed_title, out attri_speed_value, "Speed", attri_1.speed * 100, attri_2.speed * 100, true, "C0C0C0", "AACCFF");
            SetAttriText(out attri_reg_title, out attri_reg_value, "Reg", attri_1.reg, attri_2.reg, true, "C0C0C0", "FFFFAA");
            SetAttriText(out attri_bomb_title, out attri_bomb_value, "Bomb Damage", attri_1.bombDmg, attri_2.bombDmg, true, "C0C0C0", "FFCCAA");
            SetAttriText(out attri_tor_title, out attri_tor_value, "Torpedo Damage", attri_1.torDmg, attri_2.torDmg, false, "C0C0C0", "FFCCAA");

            attriText.text = attri_lv_title + "\n" + attri_hp_title + attri_reg_title + attri_speed_title + attri_bomb_title + attri_tor_title;
            attriValueText.text = attri_lv_value + "\n" + attri_hp_value + attri_reg_value + attri_speed_value + attri_bomb_value + attri_tor_value;
        }
    }
}