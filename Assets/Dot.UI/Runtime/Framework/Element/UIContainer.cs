using DotEngine.Core.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public abstract class UIContainer<TElement> : UIElement where TElement : UIElement
    {
        [SerializeReference]
        public List<TElement> m_Items = new List<TElement>();

        private List<string> m_ItemIdentities = new List<string>();

        public TElement this[string identity]
        {
            get
            {
                return GetItem(identity);
            }
        }

        public TElement this[int index]
        {
            get
            {
                if (index >= 0 && index < m_Items.Count)
                {
                    return m_Items[index];
                }
                return null;
            }
        }

        public string[] GetItemIdentities()
        {
            return m_ItemIdentities.ToArray();
        }

        public int GetItemCount()
        {
            return m_Items.Count;
        }

        public bool HasItem(string identity)
        {
            foreach (var item in m_Items)
            {
                if (item != null && item.identity == identity)
                {
                    return true;
                }
            }

            return false;
        }

        public TElement GetItem(string identity)
        {
            foreach (var item in m_Items)
            {
                if (item != null && item.identity == identity)
                {
                    return item;
                }
            }

            return null;
        }

        public TElement[] GetItems(string identity)
        {
            var list = ListPool<TElement>.Pop();
            foreach (var item in m_Items)
            {
                if (item != null && item.identity == identity)
                {
                    list.Add(item);
                }
            }

            var widgets = list.ToArray();
            ListPool<TElement>.Push(list);
            return widgets;
        }

        public void AddItem(TElement item)
        {
            InsertItem(item, m_Items.Count);
        }

        public void InsertItem(TElement item, int index)
        {
            if (index < 0)
            {
                index = 0;
            }
            else if (index >= m_Items.Count)
            {
                index = m_Items.Count;
            }

            m_Items.Insert(index, item);

            if (isInited)
            {
                if (!item.isInited)
                {
                    item.Initialize();
                }
            }

            if (isActived)
            {
                if (!item.isActived)
                {
                    item.Activate();
                }
            }
            item.parent = gameObject;

            for (int i = index; i < m_Items.Count; i++)
            {
                if (m_Items[i] != null)
                {
                    m_Items[i].SetIndex(i);
                }
            }

            OnItemAdded(item);
        }

        public void RemoveItem(string identity, bool isAll = true)
        {
            int index = 0;
            bool isRemoved = false;
            for (int i = 0; i < m_Items.Count;)
            {
                var item = m_Items[i];
                if (item == null || item.identity == identity)
                {
                    m_Items.RemoveAt(i);
                    isRemoved = true;

                    OnItemRemoved(item);

                    if (item != null)
                    {
                        item.Destroy();
                        Destroy(item);
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
                        item.SetIndex(index);
                    }

                    i++;
                    index++;
                }
            }
        }

        public void SetItemIndex(TElement item, int index)
        {
            m_Items.IndexOf(item);
        }

        public void SetItemFirst(TElement item)
        {
            SetItemIndex(item, 0);
        }

        public void SetItemLast(TElement item)
        {
            SetItemIndex(item, m_Items.Count);
        }

        protected override void OnInitialized()
        {
            for (int i = 0; i < m_Items.Count - 1; i++)
            {
                var item = m_Items[i];
                if (item != null)
                {
                    item.Initialize();
                    item.SetIndex(i);
                }
            }
        }

        protected override void OnActivated()
        {
            foreach (var item in m_Items)
            {
                item?.Activate();
            }
        }

        protected override void OnDeactivated()
        {
            foreach (var item in m_Items)
            {
                item?.Deactivate();
            }
        }

        protected override void OnDestroyed()
        {
            foreach (var item in m_Items)
            {
                item?.Destroy();
            }
            m_Items.Clear();
        }

        protected abstract void OnItemAdded(TElement item);
        protected abstract void OnItemRemoved(TElement item);
    }
}
