using DotEngine.Core;
using UnityObject = UnityEngine.Object;

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
                instance.OnInitialized();
            }
            return instance;
        }

        public static UIManager GetInstance()
        {
            return instance;
        }

        public static void DestroyInstance()
        {
            if (instance != null)
            {
                instance.OnDestroyed();
            }
            instance = null;
        }

        private UIRoot m_UIRoot;
        private void OnInitialized()
        {
            m_UIRoot = UnityObject.FindObjectOfType<UIRoot>();
            if (m_UIRoot == null)
            {
                Logger.Error("The root of ui is not found");
            }
        }

        private void OnDestroyed()
        {

        }
    }
}
