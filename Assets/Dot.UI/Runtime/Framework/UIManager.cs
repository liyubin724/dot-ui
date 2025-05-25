using DotEngine.Core;
using System.Collections.Generic;
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

                    if (uiRoot != null && uiRoot.eventSystem != null)
                    {
                        uiRoot.eventSystem.enabled = value;
                    }
                }
            }
        }

        public UIRoot uiRoot { get; private set; }

        private Dictionary<string, Dictionary<string, List<UIWindowAgent>>> m_AgentDic = new Dictionary<string, Dictionary<string, List<UIWindowAgent>>>();
        private Dictionary<string, UIWindowAgent> m_WindowDic = new Dictionary<string, UIWindowAgent>();

        private void OnInitialized()
        {
            uiRoot = UnityObject.FindObjectOfType<UIRoot>();
            if (uiRoot != null)
            {
                m_InputEnable = uiRoot.eventSystem.enabled;

                var hierarchyIdentities = uiRoot.hierarchyIdentities;
                foreach (var hIdentity in hierarchyIdentities)
                {
                    var hierarchy = uiRoot.GetHierarchy(hIdentity);
                    if (hierarchy != null)
                    {
                        var agentDic = new Dictionary<string, List<UIWindowAgent>>();
                        m_AgentDic.Add(hIdentity, agentDic);

                        var sIdentities = hierarchy.stageIdentities;
                        foreach (var sIdentity in sIdentities)
                        {
                            agentDic.Add(sIdentity, new List<UIWindowAgent>());
                        }
                    }
                }
            }
            else
            {
                DLogger.Error("The root of ui is not found");
            }
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

        public bool HasWindow(string identity)
        {
            if (m_WindowDic.TryGetValue(identity, out var agent))
            {
                return true;
            }

            return false;
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

        public void CloseWindow(string identity)
        {
            if (!m_WindowDic.TryGetValue(identity, out var agent))
            {
                return;
            }

            var hierarchyIdentity = agent.hierarchy;
            var stageIdentity = agent.stage;

        }

        private void OnDestroyed()
        {
            uiRoot = null;
        }
    }
}
