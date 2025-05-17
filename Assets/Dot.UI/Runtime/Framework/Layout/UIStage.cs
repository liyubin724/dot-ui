using UnityEngine;

namespace DotEngine.UI
{
    public enum UILayerLevel
    {
        Background = -2,
        Main = -1,
        Default = 0,
        Popup = 1,
        Overlay = 2,
    }

    [RequireComponent(typeof(Canvas))]
    public class UIStage : MonoBehaviour
    {
        [SerializeField]
        private string m_Identity;
        public string identity => m_Identity;

        [SerializeField]
        private bool m_Visible = true;

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
            if (m_GameObject == null)
            {
                m_GameObject = base.gameObject;
            }
            if (m_Transform == null)
            {
                m_Transform = base.transform;
            }
            if (m_RectTransform == null)
            {
                m_RectTransform = (RectTransform)base.transform;
            }
            if (m_Canvas == null)
            {
                m_Canvas = GetComponent<Canvas>();
            }

            if (canvas.enabled != m_Visible)
            {
                canvas.enabled = m_Visible;
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

