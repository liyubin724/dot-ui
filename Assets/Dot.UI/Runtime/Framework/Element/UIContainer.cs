using DotEngine.Core;
using DotEngine.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public enum UIContainerState
    {
        None = 0,
        Initialized,
        Activated,
        Appearing,
        Appeared,
        Disappearing,
        Disappeared,
        Deactivated,
        Destroyed,
    }

    public abstract class UIContainer<TChild> : UIElement
        where TChild : UIElement
    {
        [SerializeReference]
        public List<TChild> m_Childs = new List<TChild>();

        public string[] GetChildIdentities()
        {
            var list = ListPool<string>.Pop();
            foreach (var child in m_Childs)
            {
                if (child != null)
                {
                    if (!string.IsNullOrEmpty(child.identity))
                    {
                        list.Add(child.identity);
                    }
                }
            }

            var result = list.ToArray();
            ListPool<string>.Push(list);
            return result;
        }

        public bool HasChild(string identity)
        {
            foreach (var child in m_Childs)
            {
                if (child != null && child.identity == identity)
                {
                    return true;
                }
            }

            return false;
        }

        public TChild GetChild(string identity)
        {
            foreach (var child in m_Childs)
            {
                if (child != null && child.identity == identity)
                {
                    return child;
                }
            }

            return null;
        }

        public TChild[] GetChilds(string identity)
        {
            var list = ListPool<TChild>.Pop();
            foreach (var child in m_Childs)
            {
                if (child != null && child.identity == identity)
                {
                    list.Add(child);
                }
            }

            var widgets = list.ToArray();
            ListPool<TChild>.Push(list);
            return widgets;
        }

        public void AddChild(TChild child)
        {
            InsertChild(child, m_Childs.Count);
        }

        public void InsertChild(TChild child, int index)
        {
            if (index < 0)
            {
                index = 0;
            }
            else if (index >= m_Childs.Count)
            {
                index = m_Childs.Count;
            }

            m_Childs.Insert(index, child);

            if (isInited)
            {
                if (!child.isInited)
                {
                    child.Initialize();
                }
            }

            child.parent = gameObject;
            OnChildAdded(child);

            if (isActived)
            {
                if (!child.isActived)
                {
                    child.Activate();
                }
            }

            for (int i = index; i < m_Childs.Count; i++)
            {
                if (m_Childs[i] != null)
                {
                    m_Childs[i].orderIndex = i;
                }
            }
        }

        public void RemoveItem(string identity, bool isRemoveAll = true)
        {
            int index = 0;
            bool isRemoved = false;
            while (index < m_Childs.Count)
            {
                var child = m_Childs[index];
                if (child == null)
                {
                    m_Childs.RemoveAt(index);
                    continue;
                }

                if (!isRemoved || isRemoveAll)
                {
                    if (child.identity == identity)
                    {
                        isRemoved = true;
                        m_Childs.RemoveAt(index);

                        OnChildRemoved(child);

                        child.Destroy();
                        Destroy(child.gameObject);

                        continue;
                    }
                }

                child.orderIndex = index;
                index++;
            }
        }

        public void SetChildOrder(TChild child, int order)
        {
            int indexOf = m_Childs.IndexOf(child);
            if (indexOf < 0)
            {
                return;
            }

            if (indexOf == order)
            {
                return;
            }

            m_Childs.Insert(order, child);
            if (indexOf > order)
            {
                m_Childs.RemoveAt(indexOf + 1);
                for (int i = order; i < m_Childs.Count; i++)
                {
                    var tChild = m_Childs[i];
                    if (tChild != null)
                    {
                        tChild.orderIndex = i;
                    }
                }
            }
            else
            {
                m_Childs.RemoveAt(indexOf);
                for (int i = indexOf; i < m_Childs.Count; i++)
                {
                    var tChild = m_Childs[i];
                    if (tChild != null)
                    {
                        tChild.orderIndex = i;
                    }
                }
            }
        }

        public void SetChildAsFirst(TChild child)
        {
            int indexOf = m_Childs.IndexOf(child);
            if (indexOf < 0)
            {
                return;
            }

            m_Childs.RemoveAt(indexOf);
            m_Childs.Insert(0, child);
            for (int i = 0; i < indexOf; i++)
            {
                var tChild = m_Childs[i];
                if (tChild != null)
                {
                    tChild.orderIndex = i;
                }
            }
        }

        public void SetChildAsLast(TChild child)
        {
            int indexOf = m_Childs.IndexOf(child);
            if (indexOf < 0)
            {
                return;
            }

            m_Childs.RemoveAt(indexOf);
            m_Childs.Add(child);
            for (int i = indexOf; i < m_Childs.Count; i++)
            {
                var tChild = m_Childs[i];
                if (tChild != null)
                {
                    tChild.orderIndex = i;
                }
            }
        }

        private Animation m_Animation;
        private Animator m_Animator;

        protected override void OnInitialized()
        {
            m_Animation = GetComponent<Animation>();
            if (m_Animation == null)
            {
                m_Animator = GetComponent<Animator>();
            }

            for (int i = 0; i < m_Childs.Count - 1; i++)
            {
                var child = m_Childs[i];
                if (child == null)
                {
                    DLogger.Error("The child of container is null");
                    continue;
                }
                if (string.IsNullOrEmpty(child.identity))
                {
                    DLogger.Error("The identity of child is empty");
                    continue;
                }

                child.Initialize();
                child.parent = gameObject;

                OnChildAdded(child);

                child.orderIndex = i;
            }
        }

        protected override void OnActivated()
        {
            foreach (var child in m_Childs)
            {
                child?.Activate();
            }

            OnWillAppear();
        }

        protected virtual void OnWillAppear()
        {

        }

        protected virtual void OnAppear()
        {

        }

        protected virtual void OnAppeared()
        {

        }

        protected virtual void OnWillDisappear()
        {

        }

        protected virtual void OnDisappear()
        {

        }

        protected virtual void OnDisappeared()
        {

        }

        protected override void OnDeactivated()
        {
            foreach (var child in m_Childs)
            {
                child?.Deactivate();
            }
        }

        protected override void OnDestroyed()
        {
            foreach (var child in m_Childs)
            {
                child?.Destroy();
            }
            m_Childs.Clear();
        }

        protected override void OnIdentityChanged(string from, string to)
        {
        }

        protected override void OnLayerChanged(int from, int to)
        {
        }

        protected override void OnVisibleChanged()
        {
        }

        protected override void OnParentChanged(GameObject from, GameObject to)
        {
        }

        protected abstract void OnChildAdded(TChild child);
        protected abstract void OnChildRemoved(TChild child);

    }
}
