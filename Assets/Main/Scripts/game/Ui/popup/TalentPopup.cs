using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using com;

namespace game
{
    public class TalentPopup : PopupBehaviour
    {
        public Text titleTxt;
        public Text crtLevelTxt;
        public Text descTxt;
        public Text needTxt;
        public GameObject btn;
        public Image icon;
        public ResizeRectTransform resizer;

        public Text checkCabTxt;
        public GameObject btn_leftSub;//check cab
        public GameObject btn_rightSub;//check cab pool
        private CombatAbilityPrototype _combatAbilityPrototype;
        private TalentPrototype _proto;
        private TalentSlotBehaviour _slot;

        public void Setup(TalentPrototype proto, TalentSlotBehaviour slot)
        {
            _slot = slot;
            _proto = proto;
            var item = TalentService.instance.GetItem(proto.id);
            var crtLevel = item.saveData.level;

            icon.sprite = proto.sp;
            var protoStringValue = proto.GetStringValue();
            bool hasStringValue = !string.IsNullOrEmpty(protoStringValue);
            var localizedStringValue = "";

            if (hasStringValue)
            {
                var cab = CombatAbilityService.instance.GetPrototype(protoStringValue);
                localizedStringValue = LocalizationService.instance.GetLocalizedText(cab.title);
                _combatAbilityPrototype = cab;
                checkCabTxt.text = localizedStringValue;
                btn_leftSub.SetActive(true);
                btn_rightSub.SetActive(true);
            }
            else
            {
                btn_leftSub.SetActive(false);
                btn_rightSub.SetActive(false);
            }

            titleTxt.text = proto.GetLocalizedTitle();
            // crtLevelTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("CrtTalentLevel", crtLevel);
            crtLevelTxt.text = crtLevel + "/" + proto.GetMaxLevel();

            int demoLevel = crtLevel;
            if (crtLevel == 0)
                demoLevel = 1;
            bool hasNextLevel = demoLevel < proto.GetMaxLevel();

            var descCrtTxt = "";
            var descNextTxt = "";
            if (proto.hasIntValue)
            {
                descCrtTxt = GetDesc(demoLevel, true, proto.desc, proto.GetIntValue(demoLevel) + "");
                descNextTxt = hasNextLevel ? GetDesc(demoLevel + 1, true, proto.desc, proto.GetIntValue(demoLevel + 1) + "") : "";
            }
            else if (hasStringValue)
            {
                descCrtTxt = GetDesc(demoLevel, true, proto.desc, localizedStringValue);
                descNextTxt = hasNextLevel ? GetDesc(demoLevel + 1, true, proto.desc, localizedStringValue) : "";
            }
            else
            {
                descCrtTxt = GetDesc(demoLevel, false, proto.desc, "");
                descNextTxt = "";
            }

            if (!string.IsNullOrEmpty(descNextTxt))
            {
                descNextTxt = "\n\n" + descNextTxt;
            }
            descTxt.text = descCrtTxt + descNextTxt;

            var res = TalentService.instance.LearnTalent(proto.id, false);
            btn.SetActive(res.suc);
            if (!res.suc)
            {
                if (res.baseTalentNoOk)
                {
                    var needText = "";
                    bool firstNeed = true;
                    foreach (var baseTalentId in proto.baseTalents)
                    {
                        var tpProto = TalentService.instance.GetPrototype(baseTalentId);
                        if (!firstNeed)
                        {
                            needText += " & ";
                        }
                        needText += tpProto.GetLocalizedTitle();
                        firstNeed = false;
                    }

                    needTxt.text = LocalizationService.instance.GetLocalizedTextFormatted("NeedBaseTalent", needText);
                }
                else if (res.maxLevelReached)
                {
                    needTxt.text = LocalizationService.instance.GetLocalizedText("TalentLevelMax");
                }
                else if (res.noTalentPoint)
                {
                    needTxt.text = LocalizationService.instance.GetLocalizedText("TalentPointNotEnough");
                }
            }
            else
            {
                needTxt.text = "";
            }

            resizer.ResizeLater();
        }

        public void OnClickLearnBtn()
        {
            TalentService.instance.LearnTalent(_proto.id, true);
            TalentWindowBehaviour.instance.Refresh();
            TalentWindowBehaviour.instance.PlayEffectLearn();
            TalentWindowBehaviour.instance.PlayEffectLearnSlot(_slot);
            //SoundService.instance.Play("btn big");
            SoundService.instance.Play("ship sp");
            Hide();
        }

        private string GetLocalizedLevelString(int level)
        {
            var tp = LocalizationService.instance.GetLocalizedTextFormatted("TalentLevel", level);
            return "<color=#FF9FFF>" + tp + " </color>";
            // < color =#FF9FFF>Level 1: </color>
        }

        private string GetDesc(int level, bool hasParam, string descKey, string paramString)
        {
            return GetLocalizedLevelString(level) + (hasParam ?
                LocalizationService.instance.GetLocalizedTextFormatted(descKey, paramString)
                : LocalizationService.instance.GetLocalizedText(descKey));
        }

        public void OnClickBtnCab()
        {
            WindowService.instance.ShowCombatAbilityPopup(_combatAbilityPrototype, false, null);
        }

        public void OnClickBtnCabPool()
        {
            WindowService.instance.ShowCombatAbilitiesPopup(false);
        }
    }
}
