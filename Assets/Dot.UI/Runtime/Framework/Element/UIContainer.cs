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
            m_Childs.Add(child);

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

            OnChildAdded(child);
        }

        public void RemoveChild(string identity, bool isAll = true)
        {
            for (int i = m_Childs.Count - 1; i >= 0; i--)
            {
                var child = m_Childs[i];
                if (child == null || child.identity == identity)
                {
                    m_Childs.RemoveAt(i);

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
            }
        }

        protected override void OnInitialized()
        {
            foreach (var child in m_Childs)
            {
                child?.Initialize();
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
