using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using System.Collections.Generic;
using com;

namespace game
{
    public class TalentSlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public List<Image> arrowsTo;
        public int uiIndex;
        public float imageSize = 100;
        public Text amountText;
        private TalentPrototype _proto;
        public TalentCategory category;

        private void AssignProto()
        {
            var tts = ConfigService.instance.talentConfig.list;
            foreach (var tt in tts)
            {
                if (tt.category == category)
                {
                    if (tt.uiIndex == this.uiIndex)
                    {
                        _proto = tt;
                        icon.sprite = tt.sp;
                        icon.rectTransform.sizeDelta = new Vector2(imageSize, imageSize);
                        return;
                    }
                }
            }

            TalentItem item = TalentService.instance.GetItem(_proto.id);
        }

        public bool IsLastTalentUnlock()
        {
            if (_proto.baseTalents.Count == 0)
            {
                return true;
            }

            foreach (var baseTalentId in _proto.baseTalents)
            {
                var baseTalent = TalentService.instance.GetItem(baseTalentId);
                if (baseTalent.saveData.level == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void Refresh(bool onlyState)
        {
            if (!onlyState)
            {
                AssignProto();
                return;
            }

            TalentItem item = TalentService.instance.GetItem(_proto.id);
            var crtLevel = item.saveData.level;
            var maxLevel = _proto.GetMaxLevel();
            var crtPoints = TalentService.instance.GetTalentPoint();
            amountText.text = crtLevel + "/" + maxLevel;
            icon.color = crtLevel == 0 ? new Color(0.3f, 0.3f, 0.3f) : Color.white;

            if (crtPoints <= 0)
            {
                amountText.color = Color.white;
            }
            else
            {
                if (crtLevel == maxLevel)
                {
                    amountText.color = Color.yellow;
                }
                else
                {
                    if (IsLastTalentUnlock())
                    {
                        amountText.color = Color.green;
                    }
                    else
                    {
                        amountText.color = Color.grey;
                    }
                }
            }

            if (arrowsTo != null)
            {
                foreach (var arrowTo in arrowsTo)
                {
                    arrowTo.color = crtLevel > 0 ? Color.white : new Color(0.3f, 0.3f, 0.3f);
                }
            }
        }

        public void OnClick()
        {
            SoundService.instance.Play("btn info");
            WindowService.instance.ShowTalentPopup(_proto, this);
        }
    }
}
