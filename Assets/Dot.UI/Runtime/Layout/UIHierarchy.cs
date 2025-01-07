using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI.Layout
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIHierarchy : MonoBehaviour
    {
        [SerializeField]
        private string m_Name = string.Empty;
        [SerializeField]
        private bool m_Visible = true;
        [SerializeField]
        private UILayer[] m_layers = new UILayer[0];

        public new string name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                base.name = value;
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
                if (m_Visible == value) return;

                m_Visible = value;
                m_Canvas.enabled = value;
            }
        }

        private GameObject m_GameObject;
        private Transform m_Transform;
        private RectTransform m_RectTransform;
        private Canvas m_Canvas;
        private CanvasScaler m_CanvasScaler;
        private GraphicRaycaster m_GraphicRaycaster;

        public new GameObject gameObject => m_GameObject;
        public new Transform transform => m_Transform;
        public RectTransform rectTransform => m_RectTransform;
        public Canvas canvas => m_Canvas;
        public CanvasScaler scaler => m_CanvasScaler;
        public GraphicRaycaster raycaster => m_GraphicRaycaster;

        private void Awake()
        {
            m_GameObject = base.gameObject;
            m_Transform = base.transform;
            m_RectTransform = (RectTransform)base.transform;

            m_Canvas = GetComponent<Canvas>();
            m_CanvasScaler = GetComponent<CanvasScaler>();
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();

            if (canvas.enabled != m_Visible)
            {
                canvas.enabled = m_Visible;
            }
            if (!string.IsNullOrEmpty(m_Name) && base.name != m_Name)
            {
                base.name = m_Name;
            }
        }

        public UILayer GetLayer(string layerName)
        {
            if (m_layers == null || m_layers.Length == 0)
            {
                return null;
            }

            foreach (var layer in m_layers)
            {
                if (layer.name == layerName)
                {
                    return layer;
                }
            }
            return null;
        }

        public UILayer[] GetLayer(UILayerMask layerMask)
        {
            if (m_layers == null || m_layers.Length == 0)
            {
                return null;
            }


        }
    }
}
