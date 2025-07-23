using DotEngine.Core;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UI
{
    public sealed class UIManager : Singleton<UIManager>
    {
        private UIRootBehaviour m_Root;
        public UIRootBehaviour uiRoot => m_Root;

        private bool m_InputEnable = true;
        public bool inputEnable
        {
            get
            {
                return m_InputEnable;
            }

            set
            {
                if (m_InputEnable == value)
                {
                    return;
                }

                m_InputEnable = value;
                if (uiRoot != null && uiRoot.eventSystem != null)
                {
                    uiRoot.eventSystem.enabled = value;
                }
            }
        }

        protected override void OnInitialized()
        {
            m_Root = UnityObject.FindObjectOfType<UIRootBehaviour>();
            if (uiRoot != null)
            {
                m_InputEnable = uiRoot.eventSystem.enabled;
            }
            else
            {
                DLogger.Error("The root of ui is not found");
            }
        }

        public UIHierarchyBehaviour GetHierarchy(string hierarchyIdentity)
        {
            if (uiRoot != null)
            {
                return uiRoot.GetHierarchy(hierarchyIdentity);
            }
            return null;
        }

        public UILevelBehaviour GetLevel(string hierarchyIdentity, string levelIdentity)
        {
            if (uiRoot != null)
            {
                return uiRoot.GetLevel(hierarchyIdentity, levelIdentity);
            }
            return null;
        }


        //public bool HasWindow(string identity)
        //{
        //    if (m_WindowDic.TryGetValue(identity, out var agent))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public void OpenWindow(
        //    string hierarchy,
        //    string stage,
        //    string identity,
        //    string assetPath,
        //    UIWindowMode mode,
        //    SystemObject userdata)
        //{

        //}

        //public void CloseWindow(string identity)
        //{
        //    if (!m_WindowDic.TryGetValue(identity, out var agent))
        //    {
        //        return;
        //    }

        //    var hierarchyIdentity = agent.hierarchy;
        //    var stageIdentity = agent.stage;

        //}

        //private void OnDestroyed()
        //{
        //    uiRoot = null;
        //}
    }
}
