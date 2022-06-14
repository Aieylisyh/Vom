using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class EventBoardBehaviour : MonoBehaviour
    {
        public Text EventTitle;
        public Text EventContent;
        public List<DisplaySlotBehaviour> slots;

        public void Refresh()
        {
            int ec = UxService.instance.GetEventCount();
            int ei = UxService.instance.GetEventIndex();
            string eTokenId = UxService.instance.GetEventTokenId();
            var proto = ItemService.instance.GetPrototype(eTokenId);
            string tokenName = proto.title;
            string tokenNameLocalized = LocalizationService.instance.GetLocalizedText(tokenName);
            EventTitle.text = GetEventTitle(ec, ei, tokenNameLocalized);
            EventContent.text = GetEventContent(ec, ei, tokenNameLocalized);

            for (int i = 0; i < 4; i++)
            {
                var pId = "Token" + (i + 1);
                slots[i].Setup(new Item(1, pId), pId == eTokenId);
            }
        }

        private string GetEventTitle(int eventCount, int eventIndex, string tokenName)
        {
            return LocalizationService.instance.GetLocalizedTextFormatted("Event_Festival", tokenName);
        }

        private string GetEventContent(int eventCount, int eventIndex, string tokenName)
        {
            var param1 = GetEventTitle(eventCount, eventIndex, tokenName);
            string eTokenId = UxService.instance.GetEventTokenId();
            var param2 = tokenName + "("+com.TextFormat.GetRichTextTag(eTokenId)+")";
            string content = LocalizationService.instance.GetLocalizedTextFormatted("event_content", param1, param2);
            //Debug.Log(param1);
            //Debug.Log(param2);
            //Debug.Log(content);
            return content;
        }

        public void OnClickInfo()
        {
            SoundService.instance.Play("btn info");
            var data = new ConfirmBoxPopup.ConfirmBoxData();
            data.btnClose = true;
            data.btnBgClose = true;
            data.btnLeft = false;
            data.btnRight = false;
            data.title = LocalizationService.instance.GetLocalizedText("Event_Tokens_Title");
            data.content = LocalizationService.instance.GetLocalizedText("Event_Tokens_Desc");
            WindowService.instance.ShowConfirmBoxPopup(data);
        }
    }
}
