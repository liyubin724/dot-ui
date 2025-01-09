using DotEngine.Core.Pool;
using DotEngine.Core.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public class UIWindow : UIElement
    {
        [SerializeField]
        private UIPanel[] m_FixedPanels = new UIPanel[0];

        private string m_Guid;
        private string guid => m_Guid;

        private UILayerLevel m_LayerLevel = UILayerLevel.Default;
        public UILayerLevel layerLevel => m_LayerLevel;

        private IUIWindowController m_Controller;
        private IUIWindowController controller
        {
            get
            {
                return m_Controller;
            }
            set
            {
                m_Controller = value;
            }
        }

        private List<UIPanel> m_Panels = new List<UIPanel>();
        public UIPanel this[string identity]
        {
            get
            {
                return GetPanel(identity);
            }
        }

        private void Awake()
        {
            m_Guid = GuidUtility.CreateNew();
        }

        public override void Initialize()
        {
            if (m_FixedPanels != null && m_FixedPanels.Length > 0)
            {
                foreach (var panel in m_FixedPanels)
                {
                    m_Panels.Add(panel);
                }
            }

            foreach (var panel in m_Panels)
            {
                panel.Initialize();
            }

            OnInitialized();
            m_Controller?.OnInitialized();
        }

        public override void Activate()
        {
            foreach (var panel in m_Panels)
            {
                panel.Activate();
            }

            OnActivated();
            m_Controller?.OnActivated();
        }

        public override void Deactivate()
        {
            foreach (var panel in m_Panels)
            {
                panel.Deactivate();
            }

            OnDeactivated();
            m_Controller?.OnDeactivated();
        }

        public override void Destroy()
        {
            m_Controller?.OnDestroyed();
            OnDestroyed();

            foreach (var panel in m_Panels)
            {
                panel.Destroy();
            }

            m_Panels.Clear();
        }

        public bool HasPanel(string identity)
        {
            foreach (var panel in m_Panels)
            {
                if (panel != null && panel.identity == identity)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasPanel<T>(string identity) where T : UIPanel
        {
            foreach (var panel in m_Panels)
            {
                if (panel != null && panel.identity == identity && typeof(T).IsAssignableFrom(panel.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        public UIPanel GetPanel(string identity)
        {
            foreach (var panel in m_Panels)
            {
                if (panel != null && panel.identity == identity)
                {
                    return panel;
                }
            }
            return null;
        }

        public T GetPanel<T>(string identity) where T : UIPanel
        {
            foreach (var panel in m_Panels)
            {
                if (panel != null && panel.identity == identity && typeof(T).IsAssignableFrom(panel.GetType()))
                {
                    return (T)panel;
                }
            }
            return null;
        }

        public UIPanel[] GetPanels(string identity)
        {
            var list = ListPool<UIPanel>.Pop();
            foreach (var panel in m_Panels)
            {
                if (panel != null && panel.identity == identity)
                {
                    list.Add(panel);
                }
            }
            var panels = list.ToArray();
            ListPool<UIPanel>.Push(list);
            return panels;
        }

        public T[] GetPanels<T>(string identity) where T : UIPanel
        {
            var list = ListPool<T>.Pop();

            foreach (var panel in m_Panels)
            {
                if (panel != null && panel.identity == identity && typeof(T).IsAssignableFrom(panel.GetType()))
                {
                    list.Add((T)panel);
                }
            }

            var views = list.ToArray();
            ListPool<T>.Push(list);
            return views;
        }

        public void AddPanel(UIPanel panel)
        {
            m_Panels.Add(panel);

            OnPanelAdded(panel);
        }

        public void RemovePanel(string identity, bool isAll)
        {
            for (int i = 0; i < m_Panels.Count;)
            {
                var panel = m_Panels[i];
                if (panel != null && panel.identity == identity)
                {
                    m_Panels.RemoveAt(i);
                    OnPanelRemoved(panel);

                    if (!isAll)
                    {
                        return;
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        protected virtual void OnInitialized() { }
        protected virtual void OnActivated() { }
        protected virtual void OnDeactivated() { }
        protected virtual void OnDestroyed() { }

        protected virtual void OnPanelAdded(UIPanel panel) { }
        protected virtual void OnPanelRemoved(UIPanel panel) { }
    }
}
