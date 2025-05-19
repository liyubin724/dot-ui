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
                return transform.parent?.gameObject; ;
            }

            set
            {
                var pGO = transform.parent.gameObject;
                if (pGO != value)
                {
                    transform.SetParent(value?.transform, false);
                    OnParentChanged(pGO, value);
                }
            }
        }

        protected abstract void OnParentChanged(GameObject from, GameObject to);

        public new GameObject gameObject { get; private set; }
        public new Transform transform { get; private set; }
        public RectTransform rectTransform { get; private set; }

        public virtual void Initialize()
        {
            if (isInited)
            {
                return;
            }

            gameObject = base.gameObject;
            transform = base.transform;
            rectTransform = (RectTransform)transform;

            OnInitialized();
            isInited = true;
        }

        protected abstract void OnInitialized();

        public virtual void Activate()
        {
            if (!isInited)
            {
                throw new InvalidOperationException("The Element is not inited");
            }
            if (isActived)
            {
                return;
            }

            OnActivated();
            isActived = true;
        }

        protected abstract void OnActivated();

        public virtual void Deactivate()
        {
            if (!isActived)
            {
                return;
            }

            OnDeactivated();
            isActived = false;
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
