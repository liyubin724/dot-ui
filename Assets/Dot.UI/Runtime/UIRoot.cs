using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DotEngine.UI
{
    [Serializable]
    public class UIHierarchyData
    {
        public string name;
        public bool isDefault;
        public UIHierarchy hierarchy;
    }

    public class UIRoot : MonoBehaviour
    {
        public static UIRoot root;

        public EventSystem eventSystem;
        public UIHierarchyData[] hierarchies;

        private string m_DefaultHierarchyName;
        private Dictionary<string, UIHierarchy> m_HierarachyDic = new Dictionary<string, UIHierarchy>();

        public Camera defaultCamera
        {
            get
            {
                if (string.IsNullOrEmpty(m_DefaultHierarchyName))
                {
                    return null;
                }
                if (m_HierarachyDic.TryGetValue(m_DefaultHierarchyName, out UIHierarchy uiHierarchy))
                {
                    return uiHierarchy.uiCamera;
                }
                return null;
            }
        }

        private void Awake()
        {
            root = this;

            if (hierarchies == null || hierarchies.Length == 0)
            {
                var hierarchyComponents = GetComponentsInChildren<UIHierarchy>(true);
                if (hierarchyComponents != null && hierarchyComponents.Length > 0)
                {
                    hierarchies = new UIHierarchyData[hierarchyComponents.Length];
                    for (int i = 0; i < hierarchies.Length; i++)
                    {
                        hierarchies[i] = new UIHierarchyData()
                        {
                            name = hierarchies[i].name,
                            hierarchy = hierarchies[i].hierarchy,
                            isDefault = false
                        };
                    }
                }
            }

            if (hierarchies != null)
            {
                for (int i = 0; i < hierarchies.Length; i++)
                {
                    m_HierarachyDic.Add(hierarchies[i].name, hierarchies[i].hierarchy);
                    if (hierarchies[i].isDefault)
                    {
                        m_DefaultHierarchyName = hierarchies[i].name;
                    }
                }
            }
            if (string.IsNullOrEmpty(m_DefaultHierarchyName) && hierarchies != null && hierarchies.Length > 0)
            {
                m_DefaultHierarchyName = hierarchies[0].name;
            }

            DontDestroyOnLoad(gameObject);
        }

        public UIHierarchy GetHierarchy(string name)
        {
            return m_HierarachyDic[name];
        }
    }
}
