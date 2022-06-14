using UnityEngine;
using UnityEngine.UI;
using com;
using Text = TMPro.TextMeshProUGUI;

namespace game
{
    public class SettingsPopup : PopupBehaviour
    {
        public StandardOption option_sfx;
        public StandardOption option_music;
        public StandardOption option_vibrate;

        public LanguageOption option_En;
        public LanguageOption option_Fr;
        public LanguageOption option_Es;
        public LanguageOption option_De;
        public LanguageOption option_Zhs;
        public LanguageOption option_Zht;

        public Sprite spOn;
        public Sprite spOff;
        public Text versionTxt;

        public GameObject btnExit;

        public void Setup(bool hasExit)
        {
            LoadOptions();
            btnExit.SetActive(hasExit);
            //versionTxt.text = ConfigService.instance.coreConfig.GetVersionString();
            versionTxt.text = "Beta Version";
        }

        void LoadOptions()
        {
            //Debug.Log("LoadOptions");
            var cache = UxService.instance.settingsDataCache.cache;
            var sfxOn = cache.sfxOn;
            var musicOn = cache.musicOn;
            var vibrateOn = cache.vibrateOn;

            option_sfx.SetBan(!sfxOn);
            option_music.SetBan(!musicOn);
            option_vibrate.SetBan(!vibrateOn);

            var lange = cache.language;
            var isEn = (lange == LocalizationService.Language.en_US);
            var isEs = (lange == LocalizationService.Language.ES);
            var isFr = (lange == LocalizationService.Language.FR);
            var isDe = (lange == LocalizationService.Language.DE);
            var isZhs = (lange == LocalizationService.Language.ZHS);
            var isZht = (lange == LocalizationService.Language.ZHT);

            option_En.SetSp(isEn ? spOn : spOff);
            option_Es.SetSp(isEs ? spOn : spOff);
            option_Fr.SetSp(isFr ? spOn : spOff);
            option_De.SetSp(isDe ? spOn : spOff);
            option_Zhs.SetSp(isZhs ? spOn : spOff);
            option_Zht.SetSp(isZht ? spOn : spOff);
        }

        void SaveOptions()
        {
            var cache = UxService.instance.settingsDataCache.cache;
            UxService.instance.SaveSettingsData();
        }

        public void OnClickSfx()
        {
            Sound();
            var on = UxService.instance.settingsDataCache.cache.sfxOn;
            UxService.instance.settingsDataCache.cache.sfxOn = !on;
            SaveOptions();
            LoadOptions();

            CheatCode.Add(100);
        }

        public void OnClickMusic()
        {
            Sound();
            var on = UxService.instance.settingsDataCache.cache.musicOn;
            UxService.instance.settingsDataCache.cache.musicOn = !on;
            MusicService.instance.IsEnabled = !on;
            SaveOptions();
            LoadOptions();

            CheatCode.Add(1);
        }

        public void OnClickVibrate()
        {
            Sound();
            var on = UxService.instance.settingsDataCache.cache.vibrateOn;
            UxService.instance.settingsDataCache.cache.vibrateOn = !on;
            SaveOptions();
            LoadOptions();

            CheatCode.CheckDebugPanel();
            CheatCode.Clear();
        }

        void SetLangueCommon(LocalizationService.Language langue)
        {
            //Debug.Log("SetLangueCommon " + langue);
            Sound();
            LocalizationService.instance.currentLanguage = langue;
            //Debug.Log("保存语言 " + langue);
            UxService.instance.settingsDataCache.cache.language = langue;
            UxService.instance.settingsDataCache.cache.langueHasSet = true;
            SyncUi();
            SaveOptions();
            LoadOptions();
            LocalizeLabel.OnLanguageChange();
        }

        public void OnClickLangueEn()
        {
            SetLangueCommon(LocalizationService.Language.en_US);
        }

        public void OnClickLangueFr()
        {
            SetLangueCommon(LocalizationService.Language.FR);
        }

        public void OnClickLangueEs()
        {
            SetLangueCommon(LocalizationService.Language.ES);
        }

        public void OnClickLangueDe()
        {
            SetLangueCommon(LocalizationService.Language.DE);
        }

        public void OnClickLangueZht()
        {
            SetLangueCommon(LocalizationService.Language.ZHT);
        }

        public void OnClickLangueZhs()
        {
            SetLangueCommon(LocalizationService.Language.ZHS);
        }

        void SyncUi()
        {
            var instances = WindowBehaviour.instances;
            foreach (var i in instances)
                i.ReOpen();

            MissionService.instance.SyncUi();
        }

        public void OnClickExit()
        {
            OnClickBtnClose();
            WindowService.instance.HideAllWindows();
            WindowService.instance.HideMainButtons();
            WindowService.instance.HideMissions();
            MainHudBehaviour.instance.Hide();
            WindowService.instance.ShowLogin();
        }

        public void OnClickCredits()
        {
            SoundService.instance.Play("btn info");
            var confirmBoxData = new ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("Credits");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("CreditContent");
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("ok");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }
    }
}