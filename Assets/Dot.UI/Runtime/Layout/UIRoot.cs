using UnityEngine;
using UnityEngine.EventSystems;

namespace DotEngine.UI.Layout
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField]
        private UICamera m_UICamera;
        [SerializeField]
        private EventSystem m_EventSystem;
        [SerializeField]
        private UIHierarchy[] m_Hierarchies = new UIHierarchy[0];

        public UICamera uiCamera => m_UICamera;
        public new Camera camera => m_UICamera.camera;
        public EventSystem eventSystem => m_EventSystem;

        public UIHierarchy GetHierarchy(string name)
        {
            if (m_Hierarchies == null || m_Hierarchies.Length == 0)
            {
                return null;
            }

            foreach (var hierarchy in m_Hierarchies)
            {
                if (hierarchy.name == name)
                {
                    return hierarchy;
                }
            }

            return null;
        }

        public UIHierarchy GetHierarchy(UILayer layer)
        {
            if (m_Hierarchies == null || m_Hierarchies.Length == 0)
            {
                return null;
            }

            foreach (var hierarchy in m_Hierarchies)
            {
                if ((hierarchy.layer & layer) > 0)
                {
                    return hierarchy;
                }
            }

            return null;
        }
    }
}