using System;
using UnityEngine;

namespace DotEngine.UI.Layout
{
    [Flags]
    public enum UILayerMask
    {
        BackGround = 1 << 0,
        Main = 1 << 1,
        Default = 1 << 2,
        Popup = 1 << 3,
        Overlay = 1 << 4,
        System = 1 << 5,
    }

    [RequireComponent(typeof(Canvas))]
    public class UILayer : MonoBehaviour
    {
        [SerializeField]
        private int m_Layer;
        [SerializeField]
        private string m_Name;
        [SerializeField]
        private bool m_Visible = true;

        public UILayerMask layer => layer;
        public new string name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    base.name = value;
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
                if (m_Visible != value)
                {
                    m_Visible = value;
                    m_Canvas.enabled = value;
                }
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
            if (!string.IsNullOrEmpty(m_Name) && base.name != m_Name)
            {
                base.name = m_Name;
            }
        }
    }
}

