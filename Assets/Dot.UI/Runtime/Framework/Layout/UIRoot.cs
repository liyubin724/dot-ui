using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DotEngine.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField]
        private EventSystem m_EventSystem;
        [SerializeField]
        private UIHierarchy[] m_Hierarchies = new UIHierarchy[0];

        private Dictionary<string, UIHierarchy> m_HierarchyDic = new Dictionary<string, UIHierarchy>();

        public EventSystem eventSystem => m_EventSystem;

        public UIHierarchy hierarchy
        {
            get
            {
                if (m_Hierarchies != null && m_Hierarchies.Length > 0)
                {
                    return m_Hierarchies[0];
                }
                return null;
            }
        }

        public UIHierarchy this[string identity]
        {
            get
            {
                return GetHierarchy(identity);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (m_Hierarchies != null && m_Hierarchies.Length > 0)
            {
                foreach (var hierarchy in m_Hierarchies)
                {
                    hierarchy.Initialize();
                    m_HierarchyDic.Add(hierarchy.identity, hierarchy);
                }
            }
        }

        public bool HasHierarchy(string hierarchyName)
        {
            return m_HierarchyDic.ContainsKey(hierarchyName);
        }

        public UIHierarchy GetHierarchy(string hierarchyName)
        {
            if (m_HierarchyDic.TryGetValue(hierarchyName, out var hierarchy))
            {
                return hierarchy;
            }

            return null;
        }

        public bool HasStage(string hierarchyIdentity, string stageIdentity)
        {
            UIHierarchy hierarchy = GetHierarchy(hierarchyIdentity);
            if (hierarchy == null)
            {
                return false;
            }

            return hierarchy.HasStage(stageIdentity);
        }

        public UIStage GetStage(string hierarchyIdentity, string stageIdentity)
        {
            UIHierarchy hierarchy = GetHierarchy(hierarchyIdentity);
            if (hierarchy == null)
            {
                return null;
            }

            return hierarchy.GetStage(stageIdentity);
        }
    }
}