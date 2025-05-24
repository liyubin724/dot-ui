using DotEngine.Core;
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
        public UICamera m_UICamera;
        public UICamera uiCamera
        {
            get
            {
                return m_UICamera;
            }
            set
            {
                m_UICamera = value;
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

                    cachedCanvas.enabled = value;
                }
            }
        }

        [SerializeField]
        private UIStage[] m_Stages = new UIStage[0];

        public GameObject cachedGameObject { get; private set; }
        public Transform cachedTransform { get; private set; }
        public RectTransform cachedRectTransform { get; private set; }
        public Canvas cachedCanvas { get; private set; }
        public CanvasScaler cachedScaler { get; private set; }
        public GraphicRaycaster cachedRaycaster { get; private set; }
        public Camera cachedCamera => m_UICamera?.cachedCamera;

        private Dictionary<string, UIStage> m_StageDic = new Dictionary<string, UIStage>();

        public string[] stageIdentities { get; private set; }

        public void Initialize()
        {
            cachedGameObject = gameObject;
            cachedTransform = transform;
            cachedRectTransform = (RectTransform)transform;
            cachedCanvas = cachedGameObject.GetComponent<Canvas>();
            cachedScaler = cachedGameObject.GetComponent<CanvasScaler>();
            cachedRaycaster = cachedGameObject.GetComponent<GraphicRaycaster>();

            var goName = $"UI {m_Identity} Hierarchy";
            if (cachedGameObject.name != goName)
            {
                cachedGameObject.name = goName;
            }

            if (cachedCanvas.enabled != m_Visible)
            {
                cachedCanvas.enabled = m_Visible;
            }

            if (m_Stages != null && m_Stages.Length > 0)
            {
                stageIdentities = new string[m_Stages.Length];
                for (int i = 0; i < m_Stages.Length; i++)
                {
                    var stage = m_Stages[i];
                    if (!string.IsNullOrEmpty(stage.identity))
                    {
                        stage.Initialize();

                        stageIdentities[i] = stage.identity;
                        m_StageDic.Add(stage.identity, stage);
                    }
                    else
                    {
                        DLogger.Error("The identity of stage is empty");
                    }
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
