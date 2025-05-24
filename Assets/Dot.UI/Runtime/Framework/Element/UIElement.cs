using DotEngine.Core.Extensions;
using System;
using UnityEngine;

namespace DotEngine.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        [SerializeField]
        private string m_Identity;
        public string identity
        {
            get
            {
                return m_Identity;
            }
            set
            {
                if (m_Identity != value)
                {
                    string oldIdentity = m_Identity;
                    m_Identity = value;

                    OnIdentityChanged(oldIdentity, value);
                }
            }
        }

        protected abstract void OnIdentityChanged(string from, string to);

        public bool isInited { get; private set; }
        public bool isActived { get; private set; }

        public int layer
        {
            get
            {
                return gameObject.layer;
            }
            set
            {
                var oldLayer = gameObject.layer;
                if (oldLayer != value)
                {
                    gameObject.SetLayer(value);

                    OnLayerChanged(oldLayer, value);
                }
            }
        }

        protected abstract void OnLayerChanged(int from, int to);

        public bool visible
        {
            get
            {
                return gameObject.activeInHierarchy;
            }
            set
            {
                if (gameObject.activeInHierarchy != value)
                {
                    gameObject.SetActive(value);

                    OnVisibleChanged();
                }
            }
        }

        protected abstract void OnVisibleChanged();

        public GameObject parent
        {
            get
            {
                return m_CachedTransform.parent?.gameObject;
            }

            set
            {
                var pGO = m_CachedTransform.parent?.gameObject;
                if (pGO != value)
                {
                    m_CachedTransform.SetParent(value?.transform, false);
                    OnParentChanged(pGO, value);
                }
            }
        }

        protected abstract void OnParentChanged(GameObject from, GameObject to);

        public int orderIndex
        {
            get
            {
                return m_CachedRectTransform.GetSiblingIndex();
            }
            set
            {
                m_CachedRectTransform.SetSiblingIndex(value);
            }
        }

        public abstract void SetOrderIndex(int index);
        public abstract void SetOrderAsFirst();
        public abstract void SetOrderAsLast();

        private GameObject m_CachedGameObject;
        private Transform m_CachedTransform;
        private RectTransform m_CachedRectTransform;

        protected GameObject cachedGameObject => m_CachedGameObject;
        protected Transform cachedTransform => m_CachedTransform;
        protected RectTransform cachedRectTransform => m_CachedRectTransform;

        public virtual void Initialize()
        {
            if (isInited)
            {
                return;
            }

            m_CachedGameObject = gameObject;
            m_CachedTransform = transform;
            m_CachedRectTransform = (RectTransform)transform;

            OnInitialized();
            isInited = true;
        }

        protected abstract void OnInitialized();

        public virtual void Activate()
        {
            if (!isInited)
            {
                throw new InvalidOperationException("The Element is not initialized");
            }
            if (isActived)
            {
                return;
            }

            isActived = true;
            OnActivated();
        }

        protected abstract void OnActivated();

        public virtual void Deactivate()
        {
            if (!isActived)
            {
                return;
            }

            isActived = false;
            OnDeactivated();
        }

        protected abstract void OnDeactivated();

        public virtual void Destroy()
        {
            if (!isInited)
            {
                return;
            }

            if (isActived)
            {
                Deactivate();
            }

            OnDestroyed();
            isInited = false;
        }

        protected abstract void OnDestroyed();
    }
}
