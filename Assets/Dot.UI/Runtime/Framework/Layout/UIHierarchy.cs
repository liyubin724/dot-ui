using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIHierarchy : MonoBehaviour
    {
        [SerializeField]
        private string m_Identity;
        [SerializeField]
        private bool m_Visible = true;
        [SerializeField]
        public UICamera m_UICamera;
        [SerializeField]
        private UIStage[] m_Stages = new UIStage[0];

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
                name = $"UI {value} Hierarchy";
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

        public UICamera uiCamera => m_UICamera;
        public new Camera camera => m_UICamera?.camera;

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

        private Dictionary<string, UIStage> m_StageDic = new Dictionary<string, UIStage>();

        private void Awake()
        {
            m_GameObject = base.gameObject;
            m_Transform = base.transform;
            m_RectTransform = (RectTransform)base.transform;

            m_Canvas = GetComponent<Canvas>();
            m_CanvasScaler = GetComponent<CanvasScaler>();
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();

            if (m_Stages != null && m_Stages.Length > 0)
            {
                foreach (var layer in m_Stages)
                {
                    if (!m_StageDic.ContainsKey(layer.name))
                    {
                        m_StageDic.Add(layer.name, layer);
                    }
                    else
                    {
                        Debug.LogError($"The name({layer.name}) of layer has been added");
                    }
                }
            }

            if (canvas.enabled != m_Visible)
            {
                canvas.enabled = m_Visible;
            }
            if (string.IsNullOrEmpty(m_Identity))
            {
                m_Identity = name;
            }
            else if (m_Identity != name)
            {
                name = m_Identity;
            }
        }

        public UIStage GetStage(string identity)
        {
            if (m_StageDic.TryGetValue(identity, out var stage))
            {
                return stage;
            }
            return null;
        }

        public void SetStageVisible(string identity, bool visible)
        {
            if (m_StageDic.TryGetValue(identity, out var stage))
            {
                stage.SetVisible(visible);
            }
        }
    }
}
