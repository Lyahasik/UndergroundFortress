namespace UndergroundFortress.Helpers
{
    public static class OSManager
    {
        public static bool IsEditor()
        {
#if UNITY_EDITOR
            return true;
#else
        return false;
#endif
        }
        
        public static bool IsWeb()
        {
#if UNITY_WEBGL
            return true;
#else
        return false;
#endif
        }
    
        public static bool IsAndroid()
        {
#if UNITY_ANDROID
        return true;
#else
            return false;
#endif
        }
    
        public static bool IsIos()
        {
#if UNITY_IOS
        return true;
#else
            return false;
#endif
        }
    }
}
