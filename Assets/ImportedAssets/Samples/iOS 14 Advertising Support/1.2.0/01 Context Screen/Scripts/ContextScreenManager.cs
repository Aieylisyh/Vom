using Unity.Advertisement.IosSupport.Components;
using UnityEngine;
using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Unity.Advertisement.IosSupport.Samples
{
    /// <summary>
    /// This component will trigger the context screen to appear when the scene starts,
    /// if the user hasn't already responded to the iOS tracking dialog.
    /// </summary>
    public class ContextScreenManager : MonoBehaviour
    {
        /// <summary>
        /// The prefab that will be instantiated by this component.
        /// The prefab has to have an ContextScreenView component on its root GameObject.
        /// </summary>
        public ContextScreenView contextScreenPrefab;

        void Start()
        {
#if UNITY_IOS
            // check with iOS to see if the user has accepted or declined tracking
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            string deviceVer = Device.systemVersion;
            Debug.Log("隐私政策ATTrackingStatusBinding UNITY_IOS device sys ver "+ deviceVer);
            float f = 0;
            if(deviceVer==null){
                deviceVer="12.0";
                Debug.Log("device sys ver null");
            }
            float.TryParse(deviceVer, out f);
            if(f<=0f){
                deviceVer="12.0";
                Debug.Log("device sys ver bad format");
            }

            Version currentVersion = new Version(deviceVer); 
            Version ios14 = new Version("14.0"); 
            Debug.Log("status "+status);
            Debug.Log("currentVersion "+currentVersion);
            //NOT_DETERMINED = 0,
            //RESTRICTED = 1,
            //DENIED = 2,
            //AUTHORIZED = 3

            // && currentVersion >= ios14

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Debug.Log("show ATT");
                var contextScreen = Instantiate(contextScreenPrefab).GetComponent<ContextScreenView>();
                // after the Continue button is pressed, and the tracking request
                // has been sent, automatically destroy the popup to conserve memory
                contextScreen.sentTrackingAuthorizationRequest += () => Destroy(contextScreen.gameObject);
            }
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif
        }
    }
}
