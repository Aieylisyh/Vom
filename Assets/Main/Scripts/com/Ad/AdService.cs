using UnityEngine;
using System;
using game;
using UnityEngine.Advertisements;
 
namespace com
{
    public class AdService : MonoBehaviour
    {
        public static AdService instance;

        public AdInstanceVideo vad;

        public enum AdProvider
        {
            Android_default,
            Ios_default,
            Other_default,
        }

        public AdProvider adProvider { get; private set; }

        private bool _hasInited;

        private void Awake()
        {
            _hasInited = false;
            instance = this;
        }

        public void Setup()
        {
            if (_hasInited)
                return;

            _hasInited = true;
            adProvider = AdProvider.Other_default;
#if UNITY_TVOS
            adProvider=AdProvider.Other_default;
#endif

#if UNITY_EDITOR
            adProvider = AdProvider.Other_default;
#endif

#if UNITY_IOS
            adProvider = AdProvider.Ios_default;
#endif

#if UNITY_ANDROID
            adProvider=AdProvider.Android_default;
#endif

            Debug.Log("adProvider " + adProvider);
            AdsInitializer.instance.InitializeAds();
        }

        public bool CanPlayAd(bool enableAdFreeSuc)
        {
            Debug.Log("是否能播放广告? " + enableAdFreeSuc + " " + ConfigService.instance.adConfig.AdFreeSuc);
            if (enableAdFreeSuc && ConfigService.instance.adConfig.AdFreeSuc)
                return true;

            bool res = false;
            Debug.Log("adProvider " + adProvider);
            switch (adProvider)
            {
                case AdProvider.Android_default:
                    res = Advertisement.isInitialized;
                    break;

                case AdProvider.Ios_default:
                    res = Advertisement.isInitialized;
                    break;

                case AdProvider.Other_default:
                    res = Advertisement.isInitialized;
                    break;
            }

            return res;
        }

        public void PlayAd(Action cbFail, Action cbSuc, Action cbCease, Action cbCanPlay, Action cbCanNotPlay)
        {
            if (CanPlayAd(true))
            {
                cbCanPlay?.Invoke();
                PlayAd(cbFail, cbSuc, cbCease);
            }
            else
            {
                cbCanNotPlay?.Invoke();
            }
        }

        public void PlayAd(Action cbFail, Action cbSuc, Action cbCease)
        {
            Debug.Log("PlayAd");
            if (ConfigService.instance.adConfig.AdFreeSuc)
            {
                cbSuc?.Invoke();
                return;
            }

            vad.Play(cbFail, cbSuc, cbCease);
        }

        public void CommonFeedback_Suc()
        {
            //FloatingTextPanelBehaviour.instance.Create("ad suc!!", 0.5f, 0.7f, true);
        }

        public void CommonFeedback_Fail()
        {
            FloatingTextPanelBehaviour.instance.Create(LocalizationService.instance.GetLocalizedText("AdFail"), 0.5f, 0.7f, true);
            return;
            var confirmBoxData = new game.ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("Not success");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("AdFail");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        public void CommonFeedback_Cease()
        {
            FloatingTextPanelBehaviour.instance.Create(LocalizationService.instance.GetLocalizedText("AdCease"), 0.5f, 0.7f, true);
            return;
            var confirmBoxData = new game.ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("Not success");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("AdCease");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }

        public void CommonFeedback_CanNotPlay()
        {
            FloatingTextPanelBehaviour.instance.Create(LocalizationService.instance.GetLocalizedText("AdCanNotPlay"), 0.5f, 0.7f, true);
            return;
            var confirmBoxData = new game.ConfirmBoxPopup.ConfirmBoxData();
            confirmBoxData.btnClose = false;
            confirmBoxData.btnBgClose = true;
            confirmBoxData.btnLeft = true;
            confirmBoxData.btnRight = false;
            confirmBoxData.btnLeftTxt = LocalizationService.instance.GetLocalizedText("Continue");
            confirmBoxData.title = LocalizationService.instance.GetLocalizedText("Not success");
            confirmBoxData.content = LocalizationService.instance.GetLocalizedText("AdCanNotPlay");
            WindowService.instance.ShowConfirmBoxPopup(confirmBoxData);
        }
    }
}
