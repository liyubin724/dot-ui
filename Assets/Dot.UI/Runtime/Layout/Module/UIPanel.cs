using DotEngine.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public class UIPanel : UIElement
    {
        [SerializeField]
        private List<UIView> m_Views = new List<UIView>();

        private UIWindow m_Window = null;
        public UIWindow window => m_Window;

        public virtual void AttachToWindow(UIWindow window)
        {
            m_Window = window;
        }

        public virtual void DetachFromWindow()
        {
            m_Window = null;
        }

        public bool HasView(string identity)
        {
            foreach (var view in m_Views)
            {
                if (view != null && view.identity == identity)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasView<T>(string identity) where T : UIElement
        {
            foreach (var view in m_Views)
            {
                if (view != null && view.identity == identity && typeof(T).IsAssignableFrom(view.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        public UIView GetView(string identity)
        {
            foreach (var view in m_Views)
            {
                if (view != null && view.identity == identity)
                {
                    return view;
                }
            }
            return null;
        }

        public T GetView<T>(string identity) where T : UIView
        {
            foreach (var view in m_Views)
            {
                if (view != null && view.identity == identity && typeof(T).IsAssignableFrom(view.GetType()))
                {
                    return (T)view;
                }
            }
            return null;
        }

        public UIView[] GetViews(string identity)
        {
            var list = ListPool<UIView>.Pop();

            foreach (var view in m_Views)
            {
                if (view != null && view.identity == identity)
                {
                    list.Add(view);
                }
            }

            var views = list.ToArray();
            ListPool<UIView>.Push(list);

            return views;
        }

        public T[] GetViews<T>(string identity) where T : UIView
        {
            var list = ListPool<T>.Pop();

            foreach (var view in m_Views)
            {
                if (view != null && view.identity == identity && typeof(T).IsAssignableFrom(view.GetType()))
                {
                    list.Add((T)view);
                }
            }

            var views = list.ToArray();
            ListPool<T>.Push(list);
            return views;
        }

        public void AddView(UIView view)
        {
            m_Views.Add(view);
            view.AttachToPanel(this);

            OnViewAdded(view);
        }

        public void RemoveView(UIView view)
        {
            for (int i = 0; i < m_Views.Count; i++)
            {
                if (m_Views[i] == view)
                {
                    m_Views.RemoveAt(i);
                    OnViewRemoved(view);

                    view.DetachFromPanel();
                    return;
                }
            }
        }

        public void RemoveView(string identity, bool isAll)
        {
            for (int i = 0; i < m_Views.Count;)
            {
                var view = m_Views[i];
                if (view != null && view.identity == identity)
                {
                    m_Views.RemoveAt(i);
                    OnViewRemoved(view);

                    view.DetachFromPanel();
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

        protected virtual void OnViewAdded(UIView view) { }
        protected virtual void OnViewRemoved(UIView view) { }
    }
}
