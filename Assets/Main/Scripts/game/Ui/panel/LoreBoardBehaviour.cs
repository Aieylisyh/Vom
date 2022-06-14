using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class LoreBoardBehaviour : MonoBehaviour
    {
        public Text content;

        public void Refresh()
        {
            int ec = UxService.instance.GetEventCount();
            int ei = UxService.instance.GetEventIndex();
            string eTokenId = UxService.instance.GetEventTokenId();
            var proto = ItemService.instance.GetPrototype(eTokenId);
            string tokenName = proto.title;
            string tokenNameLocalized = LocalizationService.instance.GetLocalizedText(tokenName);

            content.text = GetEventContent(ec, ei, tokenNameLocalized);
        }

        private string GetEventTitle(int eventCount, int eventIndex, string tokenName)
        {
            return LocalizationService.instance.GetLocalizedTextFormatted("Event_Festival", tokenName);
        }

        private string GetEventContent(int eventCount, int eventIndex, string tokenName)
        {
            //"EventTitle GetEventCount " + ec+ " GetEventIndex " + ei+ " GetEventTokenId " + es+" is good";
            string content = LocalizationService.instance.GetLocalizedTextFormatted("event_content", GetEventTitle(eventCount, eventIndex, tokenName), tokenName);

            return content;
        }
    }
}
