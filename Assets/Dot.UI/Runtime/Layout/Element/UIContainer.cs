using DotEngine.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public abstract class UIContainer<T, C> : UIElement where T : UIElement where C : class, IUIContainerController<T>
    {
        [SerializeReference]
        private T[] m_FixedElements = new T[0];

        private C m_Controller = null;
        public C controller
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

        private List<T> m_Elements = new List<T>();
        public T this[string identity]
        {
            get
            {
                return GetElement(identity);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Controller?.OnInitialized();
        }

        protected override void OnInitialized()
        {
            m_Elements.Clear();
            if (m_FixedElements != null && m_FixedElements.Length > 0)
            {
                foreach (var element in m_FixedElements)
                {
                    if (element != null)
                    {
                        m_Elements.Add(element);
                    }
                }
            }

            foreach (var element in m_Elements)
            {
                element.Initialize();
            }
        }

        public override void Activate()
        {
            base.Activate();
            m_Controller?.OnActivated();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            m_Controller?.OnDeactivated();
        }

        public override void Destroy()
        {
            base.Destroy();
            m_Controller?.OnDestroyed();
        }

        public override void AttachToParent(UIElement parent)
        {
            base.AttachToParent(parent);
            m_Controller?.OnAttached();
        }

        public override void DetachFromParent()
        {
            base.DetachFromParent();
            m_Controller?.OnDetached();
        }

        public bool HasElement(string identity)
        {
            foreach (var element in m_Elements)
            {
                if (element != null && element.identity == identity)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasElement<E>(string identity) where E : T
        {
            foreach (var element in m_Elements)
            {
                if (element != null &&
                    element.identity == identity &&
                    typeof(E).IsAssignableFrom(element.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        public T GetElement(string identity)
        {
            foreach (var element in m_Elements)
            {
                if (element != null && element.identity == identity)
                {
                    return element;
                }
            }
            return null;
        }

        public T[] GetElements(string identity)
        {
            var list = ListPool<T>.Pop();

            foreach (var element in m_Elements)
            {
                if (element != null &&
                    element.identity == identity)
                {
                    list.Add(element);
                }
            }

            var result = list.ToArray();
            ListPool<T>.Push(list);
            return result;
        }

        public E[] GetElements<E>(string identity) where E : T
        {
            List<E> list = ListPool<E>.Pop();

            foreach (var element in m_Elements)
            {
                if (element != null &&
                    element.identity == identity &&
                    typeof(E).IsAssignableFrom(element.GetType()))
                {
                    list.Add((E)element);
                }
            }

            var result = list.ToArray();
            ListPool<E>.Push(list);
            return result;
        }

        public void AddElement(T element)
        {
            m_Elements.Add(element);

            if (isInited)
            {
                if (!element.isInited)
                {
                    element.Initialize();
                }

                if (isActive)
                {
                    if (!element.isActive)
                    {
                        element.Activate();
                    }
                }
            }

            element.AttachToParent(this);
            OnElementAdded(element);

            m_Controller?.OnElementAdded(element);
        }

        public void RemoveElement(T element)
        {
            for (int i = 0; i < m_Elements.Count; i++)
            {
                if (element == m_Elements[i])
                {
                    m_Elements.RemoveAt(i);

                    m_Controller?.OnElementRemoved(element);
                    OnElementRemoved(element);
                    element.DetachFromParent();

                    if (element.isActive)
                    {
                        element.Deactivate();
                    }
                    if (element.isInited)
                    {
                        element.Destroy();
                    }
                    break;
                }
            }
        }

        public void RemoveElement(string identity, bool isAll)
        {
            for (int i = 0; i < m_Elements.Count;)
            {
                var element = m_Elements[i];
                if (element != null &&
                    element.identity == identity)
                {
                    m_Elements.RemoveAt(i);

                    m_Controller?.OnElementRemoved(element);
                    OnElementRemoved(element);
                    element.DetachFromParent();

                    if (element.isActive)
                    {
                        element.Deactivate();
                    }
                    if (element.isInited)
                    {
                        element.Destroy();
                    }

                    if (!isAll)
                    {
                        break;
                    }
                }
                else
                {
                    ++i;
                }
            }
        }

        protected abstract void OnElementAdded(T element);
        protected abstract void OnElementRemoved(T element);
    }
}
