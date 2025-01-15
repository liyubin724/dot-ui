using DotEngine.Core.Extensions;
using UnityEngine;

namespace DotEngine.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        [SerializeField]
        private string m_Identity;
        public string identity => m_Identity;

        private UIElement m_Parent;
        public UIElement parent => m_Parent;

        private bool m_IsInited = false;
        public bool isInited => m_IsInited;

        private bool m_IsActive = false;
        public bool isActive => m_IsActive;

        private bool m_Visible = true;
        public bool visible => m_Visible;

        protected virtual void Awake()
        {
            m_Visible = gameObject.activeSelf;
        }

        public virtual void Initialize()
        {
            if (m_IsInited)
            {
                return;
            }

            m_IsInited = true;
        }

        public virtual void Activate()
        {
            if (m_IsActive)
            {
                return;
            }

            m_IsActive = true;
            OnActivated();
        }

        public virtual void Deactivate()
        {
            if (!m_IsActive)
            {
                return;
            }

            m_IsActive = false;
            OnDeactivated();
        }

        public virtual void Destroy()
        {
            if (!m_IsInited)
            {
                return;
            }

            if (m_IsActive)
            {
                Deactivate();
            }

            m_IsInited = false;
            OnDestroyed();
        }

        public void SetVisible(bool visible)
        {
            if (m_Visible != visible)
            {
                m_Visible = visible;

                gameObject.SetActive(visible);
            }
        }

        public void SetLayer(int layer)
        {
            gameObject.SetLayer(layer);
        }

        public virtual void AttachToParent(UIElement parent)
        {
            m_Parent = parent;
            transform.SetParent(parent.transform, false);

            OnAttached();
        }

        public virtual void DetachFromParent()
        {
            OnDetached();

            transform.SetParent(null, false);
            m_Parent = null;
        }

        protected abstract void OnInitialized();
        protected abstract void OnActivated();
        protected abstract void OnDeactivated();
        protected abstract void OnDestroyed();

        protected abstract void OnAttached();
        protected abstract void OnDetached();
    }
}
