using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace com
{
    [CustomEditor(typeof(LocalizationStorage))]
    public class LocalizationConfigEditor : Editor
    {
        private const int LANG_ZHS = 1;//01
        private const int LANG_ZHT = 2;//10
        private const int LANG_ZH = 3;//11
        private const int LANG_KO = 4;//100
        private const int LANG_JP = 8;//1000
        private const int LANG_ASIAN = 15;//1111
        private const int LANG_RU = 16;//10000
        private const int LANG_EFIGS = 32;//100000
        private const int LANG_ALL = 63;//111111

        protected SerializedProperty _localProp;
        protected int _selectedToolbar;
        protected Dictionary<int, bool> _categoryFolds;

        protected void OnEnable()
        {
            _localProp = serializedObject.FindProperty("localizations");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Import from xls"))
            {
                Import();
            }
            if (GUILayout.Button("Get all non-dup characters"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target)));
            }
            if (GUILayout.Button("Get EN characters"))
            {
                Debug.Log(new string(GetAllEnglishCharacters((LocalizationStorage)target)));
            }

            if (GUILayout.Button("Get all special characters"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, c => (c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9'))));
            }
            //   if (GUILayout.Button("Get all KOREAN characters"))
            //   {
            //       Debug.Log(new string(GetAllKoreanCharacters((LocalizationStorage)target)));
            //   }
            //   if (GUILayout.Button("Get all JAPANESE characters"))
            //   {
            //       Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_JP)));
            //   }
            if (GUILayout.Button("Get all CHINESE characters"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_ZH)));
            }
            if (GUILayout.Button("Get all simplified CHINESE characters"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_ZHS)));
            }
            if (GUILayout.Button("Get qualified ZHS characters"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, FilterCharsForChinese, LANG_ZHS)));
            }
            //  if (GUILayout.Button("Get all ASIA characters"))
            //  {
            //      Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_ASIAN)));
            //  }
            //  if (GUILayout.Button("Get all RU characters"))
            //  {
            //      Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_RU)));
            //  }
            //if (GUILayout.Button("Get all EFIGS characters"))
            //{
            //    Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_EFIGS)));
            //}

            if (GUILayout.Button("Get 简体和繁体中文"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, FilterCharsForChinese, LANG_ZH)));
            }

            if (GUILayout.Button("Get 欧美拉丁语系"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, null, LANG_EFIGS)));
            }
            if (GUILayout.Button("Get 欧美拉丁语系特殊"))
            {
                Debug.Log(new string(GetAllChars((LocalizationStorage)target, FilterCharsForEFIGS, LANG_EFIGS)));
            }
        }

        private bool FilterCharsForEFIGS(char input)
        {
            var value = input.ToString();
            if (Regex.IsMatch(value, ".*[1234567890]+.*"))
            {
                return false;
            }
            if (Regex.IsMatch(value, ".*[A-Za-z]+.*"))
            {
                return false;
            }
            return true;
        }

        private bool FilterCharsForChinese(char input)
        {
            var value = input.ToString();
            //if (Regex.IsMatch(value,"^(?:(?=.*[0-9].*)(?=.*[A-Za-z].*)(?=.*[\\W].*))[\\W0-9A-Za-z]{8,16}$"))
            //{
            //    return false;
            //}
            if (Regex.IsMatch(value, ".*[~!@#$%^&*()_+|<>,.?/:;'\\[\\]{}\"]+.*"))
            {
                return false;
            }
            if (Regex.IsMatch(value, ".*[（）。，；：【】《》……？=-、！]+.*"))
            {
                return false;
            }
            if (Regex.IsMatch(value, ".*[1234567890]+.*"))
            {
                return false;
            }
            if (Regex.IsMatch(value, ".*[A-Za-z]+.*"))
            {
                return false;
            }
            return true;
        }

        protected static char[] GetAllEnglishCharacters(LocalizationStorage storage)
        {
            if (storage == null)
                return null;
            Func<char, bool> filter = null;
            HashSet<char> map = new HashSet<char>();

            for (int i = 0; i < storage.localizations.Count; i++)
            {
                LocalizationElement elem = storage.localizations[i];
                AddCharsToMap(map, elem.textEN_US, filter);
            }
            char[] array = new char[map.Count];
            map.CopyTo(array);
            return array;
        }

        protected static char[] GetAllKoreanCharacters(LocalizationStorage storage)
        {
            if (storage == null)
                return null;
            Func<char, bool> filter = null;
            HashSet<char> map = new HashSet<char>();

            for (int i = 0; i < storage.localizations.Count; i++)
            {
                LocalizationElement elem = storage.localizations[i];
                AddCharsToMap(map, elem.textKO, filter);
            }
            char[] array = new char[map.Count];
            map.CopyTo(array);
            return array;
        }

        protected static char[] GetAllChars(LocalizationStorage storage, Func<char, bool> filter = null, int langs = LANG_ALL)
        {
            if (storage == null)
                return null;
            HashSet<char> map = new HashSet<char>();
            //Dictionary<char,bool> map = new Dictionary<char, bool>();
            for (int i = 0; i < storage.localizations.Count; i++)
            {
                LocalizationElement elem = storage.localizations[i];
                if ((langs & LANG_EFIGS) > 0)
                {
                    AddCharsToMap(map, elem.textEN_US, filter);
                    AddCharsToMap(map, elem.textPL, filter);
                    AddCharsToMap(map, elem.textDE, filter);
                    AddCharsToMap(map, elem.textFR, filter);
                    AddCharsToMap(map, elem.textES, filter);
                    AddCharsToMap(map, elem.textPTBR, filter);
                    AddCharsToMap(map, elem.textIT, filter);
                }
                if ((langs & LANG_RU) > 0)
                {
                    AddCharsToMap(map, elem.textRU, filter);
                }
                if ((langs & LANG_JP) > 0)
                {
                    AddCharsToMap(map, elem.textJA, filter);
                }
                if ((langs & LANG_ZHS) > 0)
                {
                    AddCharsToMap(map, elem.textZHS, filter);
                }
                if ((langs & LANG_ZHT) > 0)
                {
                    AddCharsToMap(map, elem.textZHT, filter);
                }
                if ((langs & LANG_KO) > 0)
                {
                    AddCharsToMap(map, elem.textKO, filter);
                }
            }
            char[] array = new char[map.Count];
            map.CopyTo(array);
            return array;
        }

        private static void AddCharsToMap(HashSet<char> map, string text, Func<char, bool> filter = null)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (filter == null || filter(ch))
                    map.Add(ch);
            }
        }

        #region static methods
        [MenuItem("Assets/Create/Config/LocalizationStorage")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<LocalizationStorage>();
        }

        [MenuItem("CONTEXT/LocalizationStorage/Import")]
        public static void Import()
        {
            LocalizationStorage conf = FindInstance();
            SerializedObject so = new SerializedObject(conf);
            so.Update();
            string filePath = XLSDatabase.FindSpreadSheetByName("LocalizationStorage");
            if (filePath != null)
            {
                XLSDatabase.ImportList(filePath, so.FindProperty("localizations"));
                so.ApplyModifiedProperties();
                Debug.Log("Import done");
            }
            else
            {
                Debug.Log("Import error: LocalizationStorage.xls does not exist");
            }
            so.ApplyModifiedProperties();
        }

        public static void SetLocalizedText(string code, LocalizationService.Language language, string value)
        {
            var instance = FindInstance();
            if (instance == null)
                return;
            var localizations = FindInstance().localizations;
            Undo.RecordObject(instance, "Set Localization from editor code");
            LocalizationElement locElement = localizations.Find(l => l.code == code);
            if (locElement != null)
            {
                locElement.SetText(language, value);
            }
            else
            {
                var newLoc = new LocalizationElement() { code = code };
                newLoc.SetText(language, value);
                localizations.Add(newLoc);
            }
            EditorUtility.SetDirty(instance);
        }

        protected static LocalizationStorage FindInstance()
        {
            LocalizationStorage[] tmp = Resources.FindObjectsOfTypeAll<LocalizationStorage>();
            if (tmp.Length < 1)
            {
                Debug.LogError("LocalizationStorage asset does not exist!");
                return null;
            }

            return tmp[0];
        }
        #endregion
    }
}
