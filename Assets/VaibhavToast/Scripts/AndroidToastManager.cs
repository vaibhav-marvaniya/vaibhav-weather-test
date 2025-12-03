using UnityEngine;

namespace Vaibhav.Toasts
{
    public class AndroidToastManager : IToastManager
    {
        public void ShowMessage(string message)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    if (activity == null)
                    {
                        Debug.LogWarning("AndroidToast currentActivity is null.");
                        return;
                    }

                    activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        using (var toastClass = new AndroidJavaClass("android.widget.Toast"))
                        {
                            const int LENGTH_SHORT = 0;

                            using (var toast = toastClass.CallStatic<AndroidJavaObject>("makeText", activity, message, LENGTH_SHORT))
                            {
                                toast.Call("show");
                            }
                        }
                    }));
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"AndroidToast exception: {ex}");
            }
#else
            Debug.Log($"[AndroidToast] {message}");
#endif
        }
    }
}
