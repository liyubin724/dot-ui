using DotEngine.Core.Extensions;
using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIStage : MonoBehaviour
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
                    m_Identity = value;
                    name = $"UI {m_Identity} Stage";
                }
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
                if (m_Visible != visible)
                {
                    m_Visible = visible;

                    cachedCanvas.enabled = m_Visible;
                }
            }
        }

        public GameObject cachedGameObject { get; private set; }
        public Transform cachedTransform { get; private set; }
        public RectTransform cachedRectTransform { get; private set; }
        public Canvas cachedCanvas { get; private set; }

        public void Initialize()
        {
            cachedGameObject = gameObject;
            cachedTransform = transform;
            cachedRectTransform = (RectTransform)transform;
            if (cachedCanvas == null)
            {
                cachedCanvas = cachedGameObject.GetOrAddComponent<Canvas>();
            }

            var goName = $"UI {m_Identity} Stage";
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

