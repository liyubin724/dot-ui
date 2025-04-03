using DotEngine.Core;
using UnityEngine.EventSystems;
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

        private bool m_InputEnable = true;
        public bool inputEnable => m_InputEnable;

        private UIRoot m_UIRoot;
        public EventSystem eventSystem => m_UIRoot.eventSystem;
        public UIHierarchy hierarchy => m_UIRoot.hierarchy;
        public UICamera uiCamera => hierarchy.uiCamera;

        private void OnInitialized()
        {
            m_UIRoot = UnityObject.FindObjectOfType<UIRoot>();
            if (m_UIRoot == null)
            {
                DLogger.Error("The root of ui is not found");
            }

            m_InputEnable = eventSystem.enabled;
        }

        public void SetInputEnable(bool enable)
        {
            if (m_InputEnable != enable)
            {
                m_InputEnable = enable;
                eventSystem.enabled = enable;
            }
        }

        private void OnDestroyed()
        {
            m_UIRoot = null;
        }
    }
}
