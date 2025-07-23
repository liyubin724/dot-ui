using DotEngine.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DotEngine.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField]
        private EventSystem m_EventSystem;
        public EventSystem eventSystem => m_EventSystem;

        [SerializeField]
        private UIHierarchy[] m_Hierarchies = new UIHierarchy[0];

        private Dictionary<string, UIHierarchy> m_HierarchyDic = new Dictionary<string, UIHierarchy>();

        public string[] hierarchyIdentities { get; private set; } = new string[0];

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (m_Hierarchies != null && m_Hierarchies.Length > 0)
            {
                hierarchyIdentities = new string[m_Hierarchies.Length];
                for (int i = 0; i < m_Hierarchies.Length; i++)
                {
                    var hierarchy = m_Hierarchies[i];
                    if (!string.IsNullOrEmpty(hierarchy.identity))
                    {
                        hierarchy.Initialize();

                        hierarchyIdentities[i] = hierarchy.identity;
                        if (!m_HierarchyDic.ContainsKey(hierarchy.identity))
                        {
                            m_HierarchyDic.Add(hierarchy.identity, hierarchy);
                        }
                        else
                        {
                            DLogger.ErrorWithFormat("The identity({0}) of hierarchy is exist", hierarchy.identity);
                        }
                    }
                    else
                    {
                        DLogger.Error("The identity of hierarchy is empty");
                    }
                }
            }
        }

        public bool HasHierarchy(string identity)
        {
            return m_HierarchyDic.ContainsKey(identity);
        }

        public UIHierarchy GetHierarchy(string identity)
        {
            if (m_HierarchyDic.TryGetValue(identity, out var hierarchy))
            {
                return hierarchy;
            }

            return null;
        }

        public bool HasLevel(string hierarchyIdentity, string levelIdentity)
        {
            UIHierarchy hierarchy = GetHierarchy(hierarchyIdentity);
            if (hierarchy == null)
            {
                return false;
            }

            return hierarchy.HasLevel(levelIdentity);
        }

        public UILevel GetLevel(string hierarchyIdentity, string levelIdentity)
        {
            UIHierarchy hierarchy = GetHierarchy(hierarchyIdentity);
            if (hierarchy == null)
            {
                return null;
            }

            return hierarchy.GetLevel(levelIdentity);
        }
    }
}