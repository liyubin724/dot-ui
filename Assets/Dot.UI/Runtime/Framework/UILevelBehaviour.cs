using DotEngine.Core.Extensions;
using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UILevelBehaviour : MonoBehaviour
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
                if (m_Identity == value) return;

                m_Identity = value;
                cachedGameObject.name = string.Format(UIDefines.UI_LEVEL_NAME_FORMAT, m_Identity);
            }
        }

        [SerializeField]
        private bool m_Visible = true;
        public bool visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if (m_Visible == value) return;

                m_Visible = visible;
                cachedCanvas.enabled = m_Visible;
            }
        }

        [SerializeField]
        [HideInInspector]
        private GameObject m_CachedGameObject;
        [SerializeField]
        [HideInInspector]
        private Transform m_CachedTransform;
        [SerializeField]
        [HideInInspector]
        private RectTransform m_CachedRectTransform;
        [SerializeField]
        [HideInInspector]
        private Canvas m_CachedCanvas;

        public GameObject cachedGameObject => m_CachedGameObject;
        public Transform cachedTransform => m_CachedTransform;
        public RectTransform cachedRectTransform => m_CachedRectTransform;
        public Canvas cachedCanvas => m_CachedCanvas;

        public void Initialize()
        {
            if (m_CachedGameObject == null)
            {
                m_CachedGameObject = gameObject;
            }
            if (m_CachedTransform == null)
            {
                m_CachedTransform = transform;
            }
            if (m_CachedRectTransform == null)
            {
                m_CachedRectTransform = (RectTransform)transform;
            }
            if (m_CachedCanvas == null)
            {
                m_CachedCanvas = cachedGameObject.GetOrAddComponent<Canvas>();
            }

            var goName = string.Format(UIDefines.UI_LEVEL_NAME_FORMAT, m_Identity);
            if (name != goName)
            {
                cachedGameObject.name = goName;
            }

            if (cachedCanvas.enabled != m_Visible)
            {
                cachedCanvas.enabled = m_Visible;
            }
        }
    }
}

