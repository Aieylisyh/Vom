using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using vom;

namespace com
{
    [System.Serializable]
    public class LocalizationService : MonoBehaviour
    {
        public static LocalizationService instance;
        public enum Language { en_US, PL, DE, FR, ES, PTBR, ZHT, KO, RU, JA, IT, ZHS, None };
        public List<Language> supportedLanguages;

        const string saveKey = "vom";

        public LocalizationStorage localizationStorage;
        public Language currentLanguage;
        private Dictionary<string, LocalizationElement> _localizations;

        readonly Language defaultLanguage = Language.en_US;

        readonly List<TMPro.TMP_FontAsset> fonts;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SetCurrentLangue();
            RecalculateLocalizationDictionary();

            LocalizeLabel.OnLanguageChange();
        }

        void SetCurrentLangue()
        {
            var cache = UxService.instance?.settingsDataCache?.cache;

            if (cache != null && cache.langueHasSet)
            {
                //Debug.Log("有存档语言 " + cache.language);
                //Debug.Log(" cache.langueHasSet " + cache.langueHasSet);
                currentLanguage = cache.language;
                return;
            }
            //Debug.Log("无存档语言");
            SystemLanguage sysLangue = Application.systemLanguage;
            var langue = LanguageFromSystemLanguage(sysLangue);
            // Debug.Log("系统语言 " + langue);
            if (supportedLanguages.Contains(langue))
            {
                currentLanguage = langue;
            }
            else
            {
                currentLanguage = defaultLanguage;
            }
            //Debug.Log("currentLanguage " + currentLanguage);
            if (cache != null)
            {
                //Debug.Log("保存语言 " + currentLanguage);
                cache.langueHasSet = true;
                cache.language = currentLanguage;
                UxService.instance.SaveSettingsData();
                return;
            }
        }

        public Language LanguageFromSystemLanguage(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.English:
                    return Language.en_US;
                case SystemLanguage.Polish:
                    return Language.PL;
                case SystemLanguage.German:
                    return Language.DE;
                case SystemLanguage.French:
                    return Language.FR;
                case SystemLanguage.Spanish:
                    return Language.ES;
                case SystemLanguage.Portuguese:
                    return Language.PTBR;
                case SystemLanguage.ChineseTraditional:
                    return Language.ZHT;
                case SystemLanguage.ChineseSimplified:
                    return Language.ZHS;
                case SystemLanguage.Chinese:
                    return Language.ZHS;
                case SystemLanguage.Korean:
                    return Language.KO;
                case SystemLanguage.Russian:
                    return Language.RU;
                case SystemLanguage.Japanese:
                    return Language.JA;
                case SystemLanguage.Italian:
                    return Language.IT;
            }
            return defaultLanguage;
        }

        public static string GetLanguageTitle(Language language)
        {
            switch (language)
            {
                case LocalizationService.Language.en_US:
                    return "ENGLISH";
                case LocalizationService.Language.ZHT:
                    return "繁中";
                case LocalizationService.Language.ZHS:
                    return "简中";
                case LocalizationService.Language.DE:
                    return "DEUTSCH";
                case LocalizationService.Language.ES:
                    return "ESPAŃOL";
                case LocalizationService.Language.FR:
                    return "FRANÇAIS";
                case LocalizationService.Language.JA:
                    return "日本語";
                case LocalizationService.Language.KO:
                    return "한국어";
                case LocalizationService.Language.PL:
                    return "POLSKI";
                case LocalizationService.Language.PTBR:
                    return "PORTUGUÊS";
                case LocalizationService.Language.RU:
                    return "Русский";
                case LocalizationService.Language.IT:
                    return "ITALIANO";
            }

            return "NONE";
        }

        public static string GetLanguageString(Language language)
        {
            switch (language)
            {
                case LocalizationService.Language.en_US:
                    return "en";
                case LocalizationService.Language.ZHT:
                    return "zht";
                case LocalizationService.Language.ZHS:
                    return "zhs";
                case LocalizationService.Language.DE:
                    return "de";
                case LocalizationService.Language.ES:
                    return "es";
                case LocalizationService.Language.FR:
                    return "fr";
                case LocalizationService.Language.JA:
                    return "jp";
                case LocalizationService.Language.KO:
                    return "ko";
                case LocalizationService.Language.PL:
                    return "pl";
                case LocalizationService.Language.PTBR:
                    return "pt";
                case LocalizationService.Language.RU:
                    return "ru";
                case LocalizationService.Language.IT:
                    return "it";
            }

            return "NONE";
        }

        public static Language GetLanguageByString(string language)
        {
            switch (language)
            {
                case "en":
                    return LocalizationService.Language.en_US;

                case "zht":
                    return LocalizationService.Language.ZHT;

                case "zhs":
                    return LocalizationService.Language.ZHS;

                case "de":
                    return LocalizationService.Language.DE;

                case "es":
                    return LocalizationService.Language.ES;

                case "fr":
                    return LocalizationService.Language.FR;

                case "jp":
                    return LocalizationService.Language.JA;

                case "ko":
                    return LocalizationService.Language.KO;

                case "pl":
                    return LocalizationService.Language.PL;

                case "pt":
                    return LocalizationService.Language.PTBR;

                case "ru":
                    return LocalizationService.Language.RU;

                case "it":
                    return LocalizationService.Language.IT;
            }

            return LocalizationService.Language.None;
        }

        public void RecalculateLocalizationDictionary()
        {
            _localizations = new Dictionary<string, LocalizationElement>();
            var i = 0;
            foreach (LocalizationElement localization in localizationStorage.localizations)
            {
                if (!string.IsNullOrEmpty(localization.code))
                {
                    if (_localizations.ContainsKey(localization.code))
                    {
                        Debug.LogError("Duplicate code: " + localization.code);
                    }
                    else
                    {
                        _localizations.Add(localization.code, localization);
                    }
                }
                else
                {
                    //Debug.LogWarning("Empty key for an entry in localizations! Text:" + localization.textEN_US + " " + i);
                }
                i++;
            }
        }

        public string GetLocalizedText(string code)
        {
            if (string.IsNullOrEmpty(code))
                return string.Empty;
            return GetLocalizedText(code, currentLanguage);
        }

        public string GetLocalizedTextFormatted(string code, params object[] arguments)
        {
            if (string.IsNullOrEmpty(code))
                return string.Empty;
            var s = GetLocalizedText(code, currentLanguage);
            return string.Format(s, arguments);
        }

        public string GetLocalizedText(string code, string defaultText)
        {
            if (string.IsNullOrEmpty(code))
                return string.Empty;
            return GetLocalizedText(code, currentLanguage, defaultText);
        }

        public string GetLocalizedText(string code, LocalizationService.Language language)
        {
            return GetLocalizedText(code, "[no code]" + code);
        }

        public string GetLocalizedText(string code, LocalizationService.Language language, string defaultText)
        {
            if (_localizations == null)
            {
                RecalculateLocalizationDictionary();
            }

            LocalizationElement localization = null;
            if (!_localizations.TryGetValue(code, out localization))
            {
                return defaultText;
            }

            var loc = localization.Localize(language);

            return loc.Replace("\\n", "\n");
        }

        public List<string> Tags
        {
            get
            {
                return _localizations.Keys.ToList();
            }
        }
    }
}
