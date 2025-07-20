using DotEngine.Core;
using System;
using UnityEngine;

namespace DotEngine.UI
{
    public class UIArgData
    {
    }

    public class UIInitializeArgData : UIArgData
    {

    }

    public class UIActivateArgData : UIArgData
    {

    }

    public class UIDeactivateArgData : UIArgData
    {
    }

    public class UIDestroyArgData : UIArgData
    {

    }

    public abstract class UIElement : MonoBehaviour
    {
        [SerializeField]
        private string m_Identity;
        [SerializeField]
        private bool m_Visible = true;

        private bool m_IsInited = false;
        private bool m_IsActived = false;

        private GameObject m_CachedGameObject;
        private Transform m_CachedTransform;
        private RectTransform m_CachedRectTransform;

        public string identity
        {
            get
            {
                return m_Identity;
            }
            set
            {
                if (m_Identity == identity)
                {
                    return;
                }

                SetIdentity(value);
            }
        }

        protected virtual void OnIdentityChanged() { }

        public void SetIdentity(string identity)
        {
            m_Identity = identity;

#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(m_Identity))
            {
                name = m_Identity;
            }
#endif

            OnIdentityChanged();
        }

        public void SetIdentityWithoutNotify(string identity)
        {
            m_Identity = identity;
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(m_Identity))
            {
                name = m_Identity;
            }
#endif
        }

        public bool visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if (m_Visible == value)
                {
                    return;
                }

                SetVisible(value);
            }
        }

        protected virtual void OnVisibleChanged() { }

        public void SetVisible(bool visible)
        {
            m_Visible = visible;

            if (m_CachedGameObject.activeSelf != visible)
            {
                m_CachedGameObject.SetActive(m_Visible);
            }

            OnVisibleChanged();
        }

        public void SetVisibleWithoutNotify(bool visible)
        {
            m_Visible = visible;

            if (m_CachedGameObject.activeSelf != visible)
            {
                m_CachedGameObject.SetActive(m_Visible);
            }
        }

        public bool isInited => m_IsInited;
        public bool isActived => m_IsActived;
        protected GameObject cachedGameObject => m_CachedGameObject;
        protected Transform cachedTransform => m_CachedTransform;
        protected RectTransform cachedRectTransform => m_CachedRectTransform;

        public virtual void Initialize(UIInitializeArgData argData)
        {
            if (isInited)
            {
                DLogger.Error("The element has been initialized");
                return;
            }

            m_CachedGameObject = gameObject;
            m_CachedTransform = gameObject.transform;
            m_CachedRectTransform = (RectTransform)gameObject.transform;

            m_IsInited = true;

            OnInitialized(argData);
        }

        protected abstract void OnInitialized(UIInitializeArgData argData);

        public virtual void Activate(UIActivateArgData argData)
        {
            if (!m_IsInited)
            {
                throw new InvalidOperationException("The Element is not initialized");
            }

            if (m_IsActived)
            {
                DLogger.Error("The element has been activated");
                return;
            }

            m_IsActived = true;

            OnActivated(argData);
        }

        protected abstract void OnActivated(UIActivateArgData argData);

        public virtual void Deactivate(UIDeactivateArgData argData)
        {
            if (!m_IsInited)
            {
                throw new InvalidOperationException("The Element is not initialized");
            }

            if (!m_IsActived)
            {
                DLogger.Error("The element has been deactivated");
                return;
            }

            m_IsActived = false;

            OnDeactivated(argData);
        }

        protected abstract void OnDeactivated(UIDeactivateArgData argData);

        public virtual void Destroy(UIDestroyArgData argData)
        {
            if (!m_IsInited)
            {
                DLogger.Error("The element is not initialized");
                return;
            }

            OnDestroyed(argData);
            m_IsInited = false;

            m_CachedGameObject = null;
            m_CachedTransform = null;
            m_CachedRectTransform = null;
        }

        protected abstract void OnDestroyed(UIDestroyArgData argData);
    }
}
