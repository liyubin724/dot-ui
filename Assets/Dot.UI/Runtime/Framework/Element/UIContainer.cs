using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEngine.UI
{
    public abstract class UIContainer<TChild> : UIWidget where TChild : UIWidget
    {
        [SerializeReference]
        public List<TChild> m_Childs = new List<TChild>();

        public TChild this[string identity]
        {
            get
            {
                return GetChild(identity);
            }
        }

        public TChild this[int index]
        {
            get
            {
                if (index >= 0 && index < m_Childs.Count)
                {
                    return m_Childs[index];
                }
                return null;
            }
        }

        public string[] GetChildNames()
        {
            return (
                from child in m_Childs
                where !string.IsNullOrEmpty(child.identity)
                select child.identity
                ).ToArray();
        }

        public int GetChildCount()
        {
            return m_Childs.Count;
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
            return (
                    from child in m_Childs
                    where child != null && child.identity == identity
                    select child
                ).ToArray();
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

            if (isActived)
            {
                if (!child.isActived)
                {
                    child.Activate();
                }
            }
            child.parent = gameObject;
            child.rectTransform.SetSiblingIndex(index);

            OnChildAdded(child);
        }

        public void RemoveChild(string identity, bool isAll = true)
        {
            int index = 0;
            bool isRemoved = false;
            for (int i = 0; i < m_Childs.Count;)
            {
                var child = m_Childs[i];
                if (child == null || child.identity == identity)
                {
                    m_Childs.RemoveAt(i);
                    isRemoved = true;

                    OnChildRemoved(child);

                    if (child != null)
                    {
                        child.Destroy();
                        Destroy(child);
                    }

                    if (!isAll)
                    {
                        break;
                    }
                }
                else
                {
                    if (isRemoved)
                    {
                        child.rectTransform.SetSiblingIndex(index);
                    }
                    i = i + 1;
                    index = index + 1;
                }
            }
        }

        protected override void OnInitialized()
        {
            for (int i = 0; i < m_Childs.Count - 1; i++)
            {
                var child = m_Childs[i];
                if (child != null)
                {
                    child.Initialize();
                    child.rectTransform.SetSiblingIndex(i);
                }
            }
        }

        protected override void OnActivated()
        {
            foreach (var child in m_Childs)
            {
                child?.Activate();
            }
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

        protected abstract void OnChildAdded(TChild child);
        protected abstract void OnChildRemoved(TChild child);
    }
}
