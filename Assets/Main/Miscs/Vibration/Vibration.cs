//using MoreMountains.NiceVibrations;
using UnityEngine;

public class Vibration
{
#if UNITY_IOS
    [System.Runtime.InteropServices.DllImport("__Internal")]
#endif
    private static extern void playSystemSound(int n);

    public static bool IsEnabled()
    {
        var cache = vom.UxService.instance.settingsDataCache.cache;
        var vibrateOn = cache.vibrateOn;
        return vibrateOn;
    }

    public static void VibrateShort()
    {
        if (!IsEnabled())
            return;

#if UNITY_EDITOR
        //Debug.Log("VibrateShort");
#elif UNITY_ANDROID
        //MMVibrationManager.ContinuousHaptic(1, 1, 0.02f, HapticTypes.LightImpact, null, true);
        com.AndroidVibration.Vibrate(25, -1, true);
#elif UNITY_IOS
       playSystemSound(1519);
#endif
    }

    public static void VibrateLong()
    {
        if (!IsEnabled())
            return;

        Handheld.Vibrate();
        //MMVibrationManager.ContinuousHaptic(1, 1, 0.5f, HapticTypes.LightImpact, null, true);
    }
}