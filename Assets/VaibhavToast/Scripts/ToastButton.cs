using UnityEngine;

namespace Vaibhav.Toasts
{
    public class ToastButton : MonoBehaviour
    {
        [TextArea]
        [SerializeField]
        private string message = "Toast Working";

        public void Show()
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("ToastButton: message is empty.");
                return;
            }
            ToastManager.Instance.ShowMessage(message);
        }

        public void SetMessage(string newMessage)
        {
            message = newMessage;
        }
    }
}
