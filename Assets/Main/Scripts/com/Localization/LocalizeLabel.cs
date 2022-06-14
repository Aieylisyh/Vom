using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace com
{
    public class LocalizeLabel : MonoBehaviour
    {
        public bool uppercaseMode = false;
        public bool textMeshPro = true;

        private Text _text;
        private TextMeshProUGUI _tmText;
        private string _localizeCode = "";
        private static List<LocalizeLabel> instances = new List<LocalizeLabel>();

        public static void OnLanguageChange()
        {
            foreach (var i in instances)
                i.ChangeText();
        }

        private void Start()
        {
            instances.Add(this);
            ChangeText();
        }

        public void SetLocalizedText(string code)
        {
            _localizeCode = code;
            ChangeText();
        }

        public void ChangeText()
        {
            if (!gameObject)
                return;

            if (textMeshPro)
            {
                ChangeTextMesh();
            }
            else
            {
                ChangeUGUIText();
            }
        }

        private void ChangeUGUIText()
        {
            if (_text == null)
                _text = GetComponent<Text>();

            if (string.IsNullOrEmpty(_localizeCode))
            {
                _localizeCode = _text.text;
            }

            string localizedText = GetLocalizedText();
            if (!string.IsNullOrEmpty(localizedText))
            {
                _text.text = uppercaseMode ? localizedText.ToUpper() : localizedText;
            }
        }

        private void ChangeTextMesh()
        {
            if (_tmText == null)
                _tmText = GetComponent<TextMeshProUGUI>();

            if (string.IsNullOrEmpty(_localizeCode))
            {
                _localizeCode = _tmText.text;
            }

            string localizedText = GetLocalizedText();

            if (!string.IsNullOrEmpty(localizedText))
            {
                _tmText.text = uppercaseMode ? localizedText.ToUpper() : localizedText;
            }
        }

        private string GetLocalizedText()
        {
            string[] aLocalizeCodes = _localizeCode.Split('+');
            string ansLocalizedText = "";
            string aLocalizeCodeTrimmed;
            foreach (string aLocalizeCode in aLocalizeCodes)
            {
                aLocalizeCodeTrimmed = aLocalizeCode.Trim();
                if (aLocalizeCodeTrimmed.Contains("\""))
                {
                    ansLocalizedText += aLocalizeCodeTrimmed.Trim('"');
                }
                else
                {
                    ansLocalizedText += LocalizationService.instance.GetLocalizedText(aLocalizeCodeTrimmed);
                }
            }
            return ansLocalizedText;
        }
    }
}
