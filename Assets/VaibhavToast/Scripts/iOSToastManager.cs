using UnityEngine;
using System.Runtime.InteropServices;

namespace Vaibhav.Toasts
{
    public class iOSToastManager : IToastManager
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void show_message(string message);
#endif

        public void ShowMessage(string message)
        {
#if UNITY_IOS && !UNITY_EDITOR
            show_message(message);
#else
            Debug.Log($"[iOSToast] {message}");
#endif
        }
    }
}
