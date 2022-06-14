using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace com
{
    public class AdInstanceVideo : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
        [SerializeField] string _editorAdUnitId = "Rewarded_Android";
        string _adUnitId;

        private Action _cbFail;
        private Action _cbSuc;
        private Action _cbCease;

        void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _adUnitId = null; // This will remain null for unsupported platforms
#if UNITY_EDITOR
            _adUnitId = _editorAdUnitId;
#elif UNITY_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
		_adUnitId = _androidAdUnitId;
#endif

            Debug.Log("RewardedAds adUnitId " + _adUnitId);
        }


        public void Play(Action cbFail, Action cbSuc, Action cbCease)
        {
            _cbFail = cbFail;
            _cbSuc = cbSuc;
            _cbCease = cbCease;

            ShowAd();
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.LogWarning("!load Unity vad " + _adUnitId);
            Debug.Log("--------------注意------------------  vad LoadAd");
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("vad Loaded: " + adUnitId);
            Debug.Log("--------------注意------------------  vad Loaded " + adUnitId);
            if (adUnitId.Equals(_adUnitId))
            {
                //ShowAd();
            }
            else
            {
                Debug.LogWarning("adUnitId not equal " + adUnitId + " " + _adUnitId);
            }
        }

        // Implement a method to execute when the user clicks the button.
        public void ShowAd()
        {
            Debug.Log("--------------注意------------------  vad ShowAd");
            Advertisement.Show(_adUnitId, this as IUnityAdsShowListener);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log("--------------注意------------------  vad OnUnityAdsShowComplete " + adUnitId);
            Debug.Log(showCompletionState);
            if (adUnitId.Equals(_adUnitId))
            {
                if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
                {
                    Debug.Log("vad Completed");
                    // Grant a reward.
                    _cbSuc?.Invoke();
                    // Load another ad:
                    Advertisement.Load(_adUnitId, this);
                }
                else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
                {
                    _cbCease?.Invoke();
                }
                else
                {
                    _cbCease?.Invoke();
                }
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
            _cbFail?.Invoke();
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
            _cbFail?.Invoke();
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            Debug.Log("--------------注意------------------  vad OnUnityAdsShowStart " + adUnitId);
        }
        public void OnUnityAdsShowClick(string adUnitId)
        {
            Debug.Log("--------------注意------------------  vad OnUnityAdsShowClick " + adUnitId);
        }

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log("--------------  vad OnUnityAdsDidStart " + placementId);
            //throw new NotImplementedException();
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log("--------------  vad OnUnityAdsDidError ");
            Debug.Log(message);
            //throw new NotImplementedException();
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log("--------------  vad OnUnityAdsDidStart " + placementId);
            //throw new NotImplementedException();
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            Debug.Log("--------------  vad OnUnityAdsDidFinish " + placementId);
            Debug.Log(showResult);
            // throw new NotImplementedException();
        }
    }
}