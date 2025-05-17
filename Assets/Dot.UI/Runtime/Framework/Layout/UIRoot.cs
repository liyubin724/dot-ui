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

        private Dictionary<string, UIHierarchy> m_NameToHierarchyDic = new Dictionary<string, UIHierarchy>();

        private void Awake()
        {
            if (m_Hierarchies != null && m_Hierarchies.Length > 0)
            {
                foreach (var hierarchy in m_Hierarchies)
                {
                    if (!m_NameToHierarchyDic.ContainsKey(hierarchy.name))
                    {
                        m_NameToHierarchyDic.Add(hierarchy.name, hierarchy);
                    }
                    else
                    {
                        Debug.LogError($"The name({hierarchy.name}) has been added");
                    }
                }
            }

            DontDestroyOnLoad(this);
        }

        public UIHierarchy GetHierarchy(string hierarchyName)
        {
            if (m_NameToHierarchyDic.TryGetValue(hierarchyName, out var hierarchy))
            {
                return hierarchy;
            }

            return null;
        }

        public UIStage GetLayer(string hierarchyIdentity, string stageIdentity)
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