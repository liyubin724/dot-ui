using DotEngine.Core;
using UnityEngine.EventSystems;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UI
{
    public class UIManager
    {
        private static UIManager sm_Instance;

        public static UIManager CreateInstance()
        {
            if (sm_Instance == null)
            {
                sm_Instance = new UIManager();
                sm_Instance.OnInitialized();
            }
            return sm_Instance;
        }

        public static UIManager GetInstance()
        {
            return sm_Instance;
        }

        public static void DestroyInstance()
        {
            sm_Instance?.OnDestroyed();
            sm_Instance = null;
        }

        private bool m_InputEnable = true;
        public bool inputEnable
        {
            get
            {
                return m_InputEnable;
            }

            set
            {
                if (m_InputEnable != value)
                {
                    m_InputEnable = value;
                    eventSystem.enabled = value;
                }
            }
        }

        public UIRoot uiRoot { get; private set; }
        public EventSystem eventSystem => uiRoot.eventSystem;
        public UIHierarchy hierarchy => uiRoot.hierarchy;
        public UICamera uiCamera => hierarchy.uiCamera;

        private void OnInitialized()
        {
            uiRoot = UnityObject.FindObjectOfType<UIRoot>();
            if (uiRoot == null)
            {
                DLogger.Error("The root of ui is not found");
            }

            m_InputEnable = eventSystem.enabled;
        }

        public UIHierarchy GetHierarchy(string hierarchyIdentity)
        {
            if (uiRoot != null)
            {
                return uiRoot.GetHierarchy(hierarchyIdentity);
            }
            return null;
        }

        public UIStage GetStage(string hierarchyIdentity, string stageIdentity)
        {
            if (uiRoot != null)
            {
                return uiRoot.GetStage(hierarchyIdentity, stageIdentity);
            }
            return null;
        }

        public void OpenWindow(
            string hierarchy,
            string stage,
            string identity,
            string assetPath,
            UIWindowMode mode,
            SystemObject userdata)
        {

        }

        public void CloseWindow()
        {

        }

        private void OnDestroyed()
        {
            uiRoot = null;
        }
    }
}
