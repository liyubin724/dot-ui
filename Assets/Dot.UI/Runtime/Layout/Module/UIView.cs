using DotEngine.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public class UIView : UIElement
    {
        [SerializeField]
        private List<UIWidget> m_Widgets = new List<UIWidget>();

        public UIPanel panel { get; private set; }

        public virtual void AttachToPanel(UIPanel panel)
        {

        }

        public virtual void DetachFromPanel()
        {

        }

        public bool HasWidget(string identity)
        {
            foreach (var widget in m_Widgets)
            {
                if (widget.identity == identity)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasWidget<T>(string identity) where T : UIWidget
        {
            foreach (var widget in m_Widgets)
            {
                if (widget.identity == identity && typeof(T).IsAssignableFrom(widget.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        public UIWidget GetWidget(string identity)
        {
            foreach (var widget in m_Widgets)
            {
                if (widget.identity == identity)
                {
                    return widget;
                }
            }
            return null;
        }

        public T GetWidget<T>(string identity) where T : UIWidget
        {
            foreach (var widget in m_Widgets)
            {
                if (widget.identity == identity && typeof(T).IsAssignableFrom(widget.GetType()))
                {
                    return (T)widget;
                }
            }
            return null;
        }

        public UIWidget[] GetWidgets(string identity)
        {
            List<UIWidget> list = ListPool<UIWidget>.Pop();
            foreach (var widget in m_Widgets)
            {
                if (widget.identity == identity)
                {
                    list.Add(widget);
                }
            }
            var widgets = list.ToArray();
            ListPool<UIWidget>.Push(list);
            return widgets;
        }

        public T[] GetWidgets<T>(string identity) where T : UIWidget
        {
            List<T> list = ListPool<T>.Pop();
            foreach (var widget in m_Widgets)
            {
                if (widget.identity == identity && typeof(T).IsAssignableFrom(widget.GetType()))
                {
                    list.Add((T)widget);
                }
            }
            var widgets = list.ToArray();
            ListPool<T>.Push(list);
            return widgets;
        }

        public void AddWidget(UIWidget widget)
        {
            m_Widgets.Add(widget);

            OnWidgetAdded(widget);
        }

        public void RemoveWidget(UIWidget widget)
        {
            for (int i = 0; i < m_Widgets.Count; i++)
            {
                if (m_Widgets[i] == widget)
                {
                    m_Widgets.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveWidget(string identity, bool isAll)
        {
            for (int i = 0; i < m_Widgets.Count;)
            {
                if (m_Widgets[i].identity == identity)
                {
                    m_Widgets.RemoveAt(i);
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

        protected virtual void OnWidgetAdded(UIWidget widget) { }
        protected virtual void OnWidgetRemoved(UIWidget widget) { }
    }
}
