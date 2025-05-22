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

                    canvas.enabled = m_Visible;
                }
            }
        }

        public RectTransform rectTransform => (RectTransform)transform;
        public Canvas canvas { get; private set; }

        public void Initialize()
        {
            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = gameObject.AddComponent<Canvas>();
                }
            }

            var goName = $"UI {m_Identity} Stage";
            if (name != goName)
            {
                gameObject.name = goName;
            }

            if (canvas.enabled != m_Visible)
            {
                canvas.enabled = m_Visible;
            }
        }
    }
}

