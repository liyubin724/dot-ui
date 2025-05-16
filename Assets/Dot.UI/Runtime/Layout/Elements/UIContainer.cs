using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public abstract class UIContainer<TChild, TController> : UIElement
        where TChild : UIElement<IUIElementController>
        where TController : IUIContainerController<TChild>
    {
        [SerializeField]
        private bool m_AutoFindChild = true;

        [SerializeReference]
        private TChild[] m_FixedChilds = new TChild[0];

        private TController controller { get; set; }

        private List<TChild> m_Childs = new List<TChild>();
        public TChild this[string identity]
        {
            get
            {
                return GetElement(identity);
            }
        }

        public int Count => m_Childs.Count;

        protected override void OnInitialized()
        {
            if (m_FixedChilds != null && m_FixedChilds.Length > 0)
            {
                foreach (var child in m_FixedChilds)
                {
                    if (child != null)
                    {
                        m_Childs.Add(child);
                    }
                }
            }

            foreach (var child in m_Childs)
            {
                child.Initialize();
            }
        }

        protected override void OnActivated()
        {
            foreach (var child in m_Childs)
            {
                child.Activate();
            }
        }

        protected override void OnDeactivated()
        {
            foreach (var child in m_Childs)
            {
                child.Deactivate();
            }
        }

        public override void Destroy()
        {
            foreach (var child in m_Childs)
            {
                child.Destroy();

                if (Array.IndexOf(m_FixedChilds, child) < 0)
                {
                    Destroy(child);
                }
            }
            m_Childs.Clear();
        }

        //public override void Initialize()
        //{
        //    base.Initialize();
        //    m_Controller?.OnInitialized();
        //}

        //protected override void OnInitialized()
        //{
        //    m_Elements.Clear();
        //    if (m_FixedElements != null && m_FixedElements.Length > 0)
        //    {
        //        foreach (var element in m_FixedElements)
        //        {
        //            if (element != null)
        //            {
        //                m_Elements.Add(element);
        //            }
        //        }
        //    }

        //    foreach (var element in m_Elements)
        //    {
        //        element.Initialize();
        //    }
        //}

        //public override void Activate()
        //{
        //    base.Activate();
        //    m_Controller?.OnActivated();
        //}

        //public override void Deactivate()
        //{
        //    base.Deactivate();
        //    m_Controller?.OnDeactivated();
        //}

        //public override void Destroy()
        //{
        //    base.Destroy();
        //    m_Controller?.OnDestroyed();
        //}

        //public override void AttachToParent(UIElement parent)
        //{
        //    base.AttachToParent(parent);
        //    m_Controller?.OnAttached();
        //}

        //public override void DetachFromParent()
        //{
        //    base.DetachFromParent();
        //    m_Controller?.OnDetached();
        //}

        //public bool HasElement(string identity)
        //{
        //    foreach (var element in m_Elements)
        //    {
        //        if (element != null && element.identity == identity)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public bool HasElement<E>(string identity) where E : TElement
        //{
        //    foreach (var element in m_Elements)
        //    {
        //        if (element != null &&
        //            element.identity == identity &&
        //            typeof(E).IsAssignableFrom(element.GetType()))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public TElement GetElement(string identity)
        //{
        //    foreach (var element in m_Elements)
        //    {
        //        if (element != null && element.identity == identity)
        //        {
        //            return element;
        //        }
        //    }
        //    return null;
        //}

        //public TElement[] GetElements(string identity)
        //{
        //    var list = ListPool<TElement>.Pop();

        //    foreach (var element in m_Elements)
        //    {
        //        if (element != null &&
        //            element.identity == identity)
        //        {
        //            list.Add(element);
        //        }
        //    }

        //    var result = list.ToArray();
        //    ListPool<TElement>.Push(list);
        //    return result;
        //}

        //public E[] GetElements<E>(string identity) where E : TElement
        //{
        //    List<E> list = ListPool<E>.Pop();

        //    foreach (var element in m_Elements)
        //    {
        //        if (element != null &&
        //            element.identity == identity &&
        //            typeof(E).IsAssignableFrom(element.GetType()))
        //        {
        //            list.Add((E)element);
        //        }
        //    }

        //    var result = list.ToArray();
        //    ListPool<E>.Push(list);
        //    return result;
        //}

        //public void AddElement(TElement element)
        //{
        //    m_Elements.Add(element);

        //    if (isInited)
        //    {
        //        if (!element.isInited)
        //        {
        //            element.Initialize();
        //        }

        //        if (isActive)
        //        {
        //            if (!element.isActive)
        //            {
        //                element.Activate();
        //            }
        //        }
        //    }

        //    element.AttachToParent(this);
        //    OnElementAdded(element);

        //    m_Controller?.OnElementAdded(element);
        //}

        //public void RemoveElement(TElement element)
        //{
        //    for (int i = 0; i < m_Elements.Count; i++)
        //    {
        //        if (element == m_Elements[i])
        //        {
        //            m_Elements.RemoveAt(i);

        //            m_Controller?.OnElementRemoved(element);
        //            OnElementRemoved(element);
        //            element.DetachFromParent();

        //            if (element.isActive)
        //            {
        //                element.Deactivate();
        //            }
        //            if (element.isInited)
        //            {
        //                element.Destroy();
        //            }
        //            break;
        //        }
        //    }
        //}

        //public void RemoveElement(string identity, bool isAll)
        //{
        //    for (int i = 0; i < m_Elements.Count;)
        //    {
        //        var element = m_Elements[i];
        //        if (element != null &&
        //            element.identity == identity)
        //        {
        //            m_Elements.RemoveAt(i);

        //            m_Controller?.OnElementRemoved(element);
        //            OnElementRemoved(element);
        //            element.DetachFromParent();

        //            if (element.isActive)
        //            {
        //                element.Deactivate();
        //            }
        //            if (element.isInited)
        //            {
        //                element.Destroy();
        //            }

        //            if (!isAll)
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            ++i;
        //        }
        //    }
        //}

        //protected abstract void OnElementAdded(TElement element);
        //protected abstract void OnElementRemoved(TElement element);
    }
}
