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
                    name = $"UI {value} Hierarchy";
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
                if (m_Visible != value)
                {
                    m_Visible = value;

                    canvas.enabled = value;
                }
            }
        }

        [SerializeField]
        public UICamera m_UICamera;
        public UICamera uiCamera
        {
            get
            {
                return m_UICamera;
            }
            set
            {
                if (m_UICamera != value)
                {
                    m_UICamera = value;
                }
            }
        }
        public new Camera camera
        {
            get
            {
                return m_UICamera.camera;
            }
        }

        [SerializeField]
        private UIStage[] m_Stages = new UIStage[0];

        private Dictionary<string, UIStage> m_StageDic = new Dictionary<string, UIStage>();
        public UIStage this[string identity]
        {
            get
            {
                return GetStage(identity);
            }
        }

        public new GameObject gameObject { get; private set; }
        public new Transform transform { get; private set; }
        public RectTransform rectTransform { get; private set; }
        public Canvas canvas { get; private set; }
        public CanvasScaler scaler { get; private set; }
        public GraphicRaycaster raycaster { get; private set; }

        public void Initialize()
        {
            gameObject = base.gameObject;
            transform = base.transform;
            rectTransform = (RectTransform)base.transform;

            canvas = GetComponent<Canvas>();
            scaler = GetComponent<CanvasScaler>();
            raycaster = GetComponent<GraphicRaycaster>();

            var goName = $"UI {m_Identity} Hierarchy";
            if (gameObject.name != goName)
            {
                gameObject.name = goName;
            }

            if (canvas.enabled != m_Visible)
            {
                canvas.enabled = m_Visible;
            }

            if (m_Stages != null && m_Stages.Length > 0)
            {
                foreach (var stage in m_Stages)
                {
                    stage.Initialize();
                    m_StageDic.Add(stage.identity, stage);
                }
            }
        }

        public bool HasStage(string identity)
        {
            return m_StageDic.ContainsKey(identity);
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
                stage.visible = visible;
            }
        }
    }
}
