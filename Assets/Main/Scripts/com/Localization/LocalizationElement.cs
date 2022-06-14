using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com
{

    [System.Serializable]
    public class LocalizationElement
    {
        public string code;
        public string textEN_US;
        public string textZHT;
        public string textPL;
        public string textDE;
        public string textFR;
        public string textES;
        public string textPTBR;
        public string textKO;
        public string textRU;
        public string textJA;
        public string textIT;
        public string textZHS;

        public string Localize(LocalizationService.Language language)
        {
            switch (language)
            {
                case LocalizationService.Language.en_US: return textEN_US;
                case LocalizationService.Language.ZHT: return textZHT;
                case LocalizationService.Language.DE: return textDE;
                case LocalizationService.Language.ES: return textES;
                case LocalizationService.Language.FR: return textFR;
                case LocalizationService.Language.JA: return textJA;
                case LocalizationService.Language.KO: return textKO;
                case LocalizationService.Language.PL: return textPL;
                case LocalizationService.Language.PTBR: return textPTBR;
                case LocalizationService.Language.RU: return textRU;
                case LocalizationService.Language.IT: return textIT;
                case LocalizationService.Language.ZHS: return textZHS;

            }

            Debug.LogError("No localization for language: " + language.ToString());
            return string.Empty;
        }

        public void SetText(LocalizationService.Language language, string value)
        {
            switch (language)
            {
                case LocalizationService.Language.en_US: textEN_US = value; break;
                case LocalizationService.Language.ZHT: textZHT = value; break;
                case LocalizationService.Language.DE: textDE = value; break;
                case LocalizationService.Language.ES: textES = value; break;
                case LocalizationService.Language.FR: textFR = value; break;
                //case LocalizationService.Language.JA: textJA = value; break;
                //case LocalizationService.Language.KO: textKO = value; break;
                //case LocalizationService.Language.PL: textPL = value; break;
                //case LocalizationService.Language.PTBR: textPTBR = value; break;
                //case LocalizationService.Language.RU: textRU = value; break;
                //case LocalizationService.Language.IT: textIT = value; break;
                case LocalizationService.Language.ZHS: textZHS = value; break;

                default:
                    Debug.LogError("Unsupporeted language: " + language.ToString());
                    break;
            }
        }
    }
}