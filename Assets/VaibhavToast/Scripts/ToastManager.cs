namespace Vaibhav.Toasts
{
    public static class ToastManager
    {
        private static IToastManager _instance;

        /// <summary>
        /// Global instance used by your game/app.
        /// </summary>
        public static IToastManager Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_ANDROID
                    _instance = new AndroidToastManager();
#elif UNITY_IOS
                    _instance = new iOSToastManager();
#else
                    _instance = new DebugToast();
#endif
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
