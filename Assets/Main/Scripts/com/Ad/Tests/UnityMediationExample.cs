using UnityEngine;
//using Unity.Services.Core;
//using Unity.Services.Mediation;

namespace com
{
    public class UnityMediationExample : MonoBehaviour
    {
        private void Start()
        {
            Init();
        }

        async void Init()
        {
            var gameId = "";
#if UNITY_IOS
		gameId = "4536450";
#elif UNITY_ANDROID
		gameId = "4536451";
#endif
            Debug.Log("--------------注意------------------ UnityMediationExample gameId" + gameId);
            // Initialize package with a custom Game ID
            //InitializationOptions options = new InitializationOptions();
            //options.SetGameId(gameId);
            //await UnityServices.InitializeAsync(options);
        }
    }
}
