using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace com
{
    public class BannerAdExample : MonoBehaviour
    {
        // For the purpose of this example, these buttons are for functionality testing:
        [SerializeField] Button _loadBannerButton;
        [SerializeField] Button _showBannerButton;
        [SerializeField] Button _hideBannerButton;

        [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

        [SerializeField] string _androidAdUnitId = "Banner_Android";
        [SerializeField] string _iOsAdUnitId = "Banner_iOS";
        string _adUnitId;

        void Start()
        {
            // Disable the button until an ad is ready to show:
            _showBannerButton.interactable = false;
            _hideBannerButton.interactable = false;
#if UNITY_IOS
		_adUnitId = _iOsAdUnitId;
#elif UNITY_ANDROID
		_adUnitId = _androidAdUnitId;
#endif
            Debug.Log("--------------注意------------------ banner adUnitId " + _adUnitId);

            // Set the banner position:
            Advertisement.Banner.SetPosition(_bannerPosition);

            // Configure the Load Banner button to call the LoadBanner() method when clicked:
            _loadBannerButton.onClick.AddListener(LoadBanner);
            _loadBannerButton.interactable = true;
        }

        // Implement a method to call when the Load Banner button is clicked:
        public void LoadBanner()
        {
            Debug.Log("--------------注意------------------ LoadBanner");
            // Set up options to notify the SDK of load events:
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(_adUnitId, options);
        }

        // Implement code to execute when the loadCallback event triggers:
        void OnBannerLoaded()
        {
            Debug.Log("--------------注意------------------ OnBannerLoaded");
            // Configure the Show Banner button to call the ShowBannerAd() method when clicked:
            _showBannerButton.onClick.AddListener(ShowBannerAd);
            // Configure the Hide Banner button to call the HideBannerAd() method when clicked:
            _hideBannerButton.onClick.AddListener(HideBannerAd);

            // Enable both buttons:
            _showBannerButton.interactable = true;
            _hideBannerButton.interactable = true;
        }

        // Implement code to execute when the load errorCallback event triggers:
        void OnBannerError(string message)
        {
            Debug.Log("--------------注意------------------ OnBannerError");
            Debug.Log($"Banner Error: {message}");
            // Optionally execute additional code, such as attempting to load another ad.
        }

        // Implement a method to call when the Show Banner button is clicked:
        void ShowBannerAd()
        {
            Debug.Log("--------------注意------------------ ShowBannerAd");
            // Set up options to notify the SDK of show events:
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_adUnitId, options);
        }

        // Implement a method to call when the Hide Banner button is clicked:
        void HideBannerAd()
        {
            Debug.Log("--------------注意------------------ HideBannerAd");
            // Hide the banner:
            Advertisement.Banner.Hide();
        }

        void OnBannerClicked() { }
        void OnBannerShown() { }
        void OnBannerHidden() { }

        void OnDestroy()
        {
            Debug.Log("--------------注意------------------ OnDestroy");
            // Clean up the listeners:
            _loadBannerButton.onClick.RemoveAllListeners();
            _showBannerButton.onClick.RemoveAllListeners();
            _hideBannerButton.onClick.RemoveAllListeners();
        }
    }
}