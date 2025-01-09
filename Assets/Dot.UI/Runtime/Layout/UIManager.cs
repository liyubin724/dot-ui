namespace DotEngine.UI
{
    public sealed class UIManager
    {
        private static UIManager instance;

        public static UIManager CreateInstance()
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }

        public static UIManager GetInstance()
        {
            return instance;
        }

        public static void DestroyInstance()
        {
            instance = null;
        }
    }
}
