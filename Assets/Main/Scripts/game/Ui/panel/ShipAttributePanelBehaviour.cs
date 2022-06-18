using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class ShipAttributePanelBehaviour : MonoBehaviour
    {
        public Text attri_hp;
        public Text attri_speed;
        public Text attri_armor;
        public Text attri_reg;
        //public Text attri_vita;

        public Text attri_tor;
        public Text attri_bomb;

        public Text btnUnlockPrice;
        public Text btnLevelupPrice;

        public GameObject btnUnlock;
        public GameObject btnLevelup;
        public GameObject MaxLevel;
        public Slider slider;
        public Text shipLevelTxt;

        public GameObject particleGo;

        public void Refresh()
        {
            MainHudBehaviour.instance.RefreshToDefault();

            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;
            shipLevelTxt.text = shipLevel + "/" + shipProto.levelMax;
            slider.value = (float)shipLevel / (float)shipProto.levelMax;
            AssignAttributes();
            btnUnlock.SetActive(false);
            btnLevelup.SetActive(false);

            MaxLevel.SetActive(false);
            if (shipItem.saveData.unlocked)
            {
                if (ShipService.instance.IsLevelupPossible())
                {
                    btnLevelup.SetActive(true);
                    btnUnlock.SetActive(false);
                    var price = ShipService.instance.GetLevelupPrice(shipItem);
                    btnLevelupPrice.text = TextFormat.GetItemText(price, true, true);
                }
                else
                {
                    btnLevelup.SetActive(false);
                    btnUnlock.SetActive(false);
                    MaxLevel.SetActive(true);
                }
            }
            else
            {
                btnLevelup.SetActive(false);
                btnUnlock.SetActive(true);
                var price = ShipService.instance.GetUnlockPrice(shipItem);
                btnUnlockPrice.text = TextFormat.GetItemText(price, true);
            }
        }

        private string GetAttriLocalizedLabel(string prefixCode)
        {
            return LocalizationService.instance.GetLocalizedText(prefixCode);
        }

        private string GetAttriText(string prefixCode, float value1, float value2, int multiplier = 1)
        {
            var res = GetAttriLocalizedLabel(prefixCode) + ": <color=#FFFF99>";
            var valueDelta = value2 - value1;
            int v1 = (int)value1;
            int v2 = (int)valueDelta;

            res += (v1 * multiplier) + "</color>";

            if (v2 != 0)
            {
                res += "<color=#AAFFAA> ";
                if (v2 > 0)
                {
                    res += "+";
                }
                res += (int)(v2 * multiplier) + "</color>";
            }
            return res;
        }

        private void AssignAttributes()
        {
            CombatService.instance.RefreshPlayerAttri();
            var attri_base = CombatService.instance.playerAttri.vBase;
            var attri_add = CombatService.instance.playerAttri;

            attri_hp.text = GetAttriText("Hp", attri_base.hp, attri_add.hp);

            attri_speed.text = GetAttriText("Speed", attri_base.speed * 100, attri_add.speed * 100, 1);

            attri_armor.text = GetAttriText("Armor", attri_base.armor, attri_add.armor);

            attri_reg.text = GetAttriText("Reg", attri_base.reg, attri_add.reg);
            attri_reg.text += "/s";

            //attri_vita.text = GetAttriText("Vita", attri_base.vita, attri_add.vita);

            attri_tor.text = GetAttriText("Torpedo Damage", attri_base.torDmg, attri_add.torDmg);

            attri_bomb.text = GetAttriText("Bomb Damage", attri_base.bombDmg, attri_add.bombDmg);
        }

        public void OnClickLevelup()
        {
            WindowService.instance.ShowConfirmUpgradePopup();
        }

        public void OnClickUnlock()
        {
            var shipProto = ShipService.instance.GetPrototype();
            var shipItem = ShipService.instance.GetShipItem();
            var shipLevel = shipItem.saveData.level;

            if (shipItem.saveData.unlocked)
            {
                Debug.Log("unlocked try to unlock!");
            }
            else
            {
                Unlock();
            }
        }

        void UnlockSuc()
        {
            ShipService.instance.UnlockShip();
            //ShipWindowBehaviour.instance.UpdateShipView();
            //ShipWindowBehaviour.instance.PlayEffectUnlock();
            var shipNameLocalized = LocalizationService.instance.GetLocalizedText(ShipService.instance.GetPrototype().title);

            SoundService.instance.Play("ship unlock");
            string s = "<size=110%><color=#FFFF66>" + LocalizationService.instance.GetLocalizedText("UnlockSucTitle");
            s += "</color>\n" + LocalizationService.instance.GetLocalizedTextFormatted("UnlockSucContent", shipNameLocalized) + "</size>";
            FloatingTextPanelBehaviour.instance.Create(s, 0.5f, 0.68f, true);
            var go = Instantiate(particleGo, particleGo.transform.parent);
            go.SetActive(true);
            Destroy(go, 1);
        }

        private void Unlock()
        {
            //Debug.Log("try Unlock!");
            if (ShipService.instance.IsShipUnlockAffordable(true))
            {
                WaitingCircleBehaviour.instance.SetHideAction(() => { UnlockSuc(); });
                WaitingCircleBehaviour.instance.Show(1.6f);
                return;
            }

            var data = new ConfirmBoxPopup.ConfirmBoxData();
            SoundService.instance.Play("btn info");
            data.btnClose = false;
            data.btnBgClose = false;
            data.btnLeft = true;
            data.btnRight = false;
            data.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Ok");
            data.title = LocalizationService.instance.GetLocalizedText("ShipUnlockNonAffordableTitle");
            data.content = LocalizationService.instance.GetLocalizedText("ShipUnlockNonAffordableContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = false;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("ShipAttriInfoTitle");
            data.content = LocalizationService.instance.GetLocalizedText("ShipAttriInfoContent");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }

        public void OnLevelupSuc()
        {
            ShipService.instance.LevelupShip();
            SoundService.instance.Play("btn big");
            //CombatService.instance.RefreshPlayerAttri();
            //ShipWindowBehaviour.instance.UpdateShipView();
            var go = Instantiate(particleGo, particleGo.transform.parent);
            go.SetActive(true);
            Destroy(go, 1);
            //ShipWindowBehaviour.instance.PlayEffectLevelup();
        }
    }
}