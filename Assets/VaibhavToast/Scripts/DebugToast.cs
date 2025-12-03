using UnityEngine;

namespace Vaibhav.Toasts
{
    public class DebugToast : IToastManager
    {
        public void ShowMessage(string message)
        {
            Debug.Log($"[DebugToast] {message}");
        }
    }
}
