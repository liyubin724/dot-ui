using UnityEngine;

namespace DotEngine.UI.Layout
{
    public enum UILayerLevel
    {
        BackGround = -2,
        Main = -1,
        Default = 0,
        Popup = 1,
        Overlay = 2,
    }

    [RequireComponent(typeof(Canvas))]
    public class UILayer : MonoBehaviour
    {
        [SerializeField]
        private UILayerLevel m_Level = UILayerLevel.Default;
        [SerializeField]
        private string m_Alias;
        [SerializeField]
        private bool m_Visible = true;

        public UILayerLevel level => m_Level;
        public string alias
        {
            get
            {
                return m_Alias;
            }
            set
            {
                if (m_Alias != value)
                {
                    m_Alias = value;
                    name = value;
                }
            }
        }
        public bool visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                SetVisible(value);
            }
        }

        private GameObject m_GameObject;
        private Transform m_Transform;
        private RectTransform m_RectTransform;

        private Canvas m_Canvas;

        public new GameObject gameObject => m_GameObject;
        public new Transform transform => m_Transform;
        public RectTransform rectTransform => m_RectTransform;
        public Canvas canvas => m_Canvas;

        private void Awake()
        {
            m_GameObject = base.gameObject;
            m_Transform = base.transform;
            m_RectTransform = (RectTransform)base.transform;

            m_Canvas = GetComponent<Canvas>();

            if (canvas.enabled != m_Visible)
            {
                canvas.enabled = m_Visible;
            }
            if (string.IsNullOrEmpty(m_Alias))
            {
                m_Alias = name;
            }
            else if (m_Alias != name)
            {
                name = m_Alias;
            }
        }

        public void SetVisible(bool visible)
        {
            if (m_Visible != visible)
            {
                m_Visible = visible;
                m_Canvas.enabled = m_Visible;
            }
        }
    }
}

