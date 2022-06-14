using UnityEngine;
using UnityEngine.Advertisements;

namespace com
{
    //this is to checkup unity ads SDK in runtime
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string _androidGameId;
        [SerializeField] string _iOSGameId;
        //Test mode allows you to test your integration without serving live ads.
        [SerializeField] bool _testMode = true;

        [SerializeField] bool _enablePerPlacementMode = true;

        private string _gameId;

        public static AdsInitializer instance { get; private set; }

        void Awake()
        {
            instance = this;
            //InitializeAds();
        }

        public void InitializeAds()
        {
            Debug.LogWarning("!广告初始化 Application.platform " + Application.platform);
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.LogWarning("!Unity Ads initialization complete.广告初始化完毕");
            AdService.instance.vad.LoadAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogWarning("广告初始化完毕失败");
            Debug.LogWarning($"!Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}