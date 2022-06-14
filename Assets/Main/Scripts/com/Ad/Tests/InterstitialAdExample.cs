using UnityEngine;
using UnityEngine.Advertisements;

//To display a full-screen interstitial ad using the Advertisements API, initialize the SDK, 
//then use the Load function to load ad content to an Ad Unit and the Show function to show the ad.
namespace com
{
    public class InterstitialAdExample : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] string _androidAdUnitId = "Interstitial_Android";
        [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
        string _adUnitId;

        void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
    ? _iOsAdUnitId
    : _androidAdUnitId;
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Debug.Log("--------------注意------------------ int LoadAd");
            Advertisement.Load(_adUnitId, this);
        }

        // Show the loaded content in the Ad Unit: 
        public void ShowAd()
        {
            // Note that if the ad content wasn't previously loaded, this method will fail
            Debug.Log("Showing Ad: " + _adUnitId);
            Debug.Log("--------------注意------------------ int ShowAd");
            Advertisement.Show(_adUnitId, this);
        }

        // Implement Load Listener and Show Listener interface methods:  
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            // Optionally execute code if the Ad Unit successfully loads content.
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
            Debug.Log("--------------注意------------------ int OnUnityAdsFailedToLoad");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            Debug.Log("--------------注意------------------ int OnUnityAdsShowFailure");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
    }
}