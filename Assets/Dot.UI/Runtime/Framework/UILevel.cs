using DotEngine.Core.Extensions;
using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UILevel : MonoBehaviour
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
        private GameObject m_GameObject;
        [SerializeField]
        [HideInInspector]
        private Transform m_Transform;
        [SerializeField]
        [HideInInspector]
        private RectTransform m_RectTransform;
        [SerializeField]
        [HideInInspector]
        private Canvas m_Canvas;

        public GameObject cachedGameObject => m_GameObject;
        public Transform cachedTransform => m_Transform;
        public RectTransform cachedRectTransform => m_RectTransform;
        public Canvas cachedCanvas => m_Canvas;

        public void Initialize()
        {
            if (m_GameObject == null)
            {
                m_GameObject = gameObject;
            }
            if (m_Transform == null)
            {
                m_Transform = transform;
            }
            if (m_RectTransform == null)
            {
                m_RectTransform = (RectTransform)transform;
            }
            if (m_Canvas == null)
            {
                m_Canvas = cachedGameObject.GetOrAddComponent<Canvas>();
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

