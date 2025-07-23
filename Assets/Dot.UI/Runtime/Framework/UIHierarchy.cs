using DotEngine.Core;
using DotEngine.Core.Extensions;
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

                    cachedCanvas.enabled = value;
                }
            }
        }

        [SerializeField]
        private UILevel[] m_Levels = new UILevel[0];

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
        [SerializeField]
        [HideInInspector]
        private CanvasScaler m_Scaler;
        [SerializeField]
        [HideInInspector]
        private GraphicRaycaster m_Raycaster;

        [SerializeField]
        public UICamera m_UICamera;

        public GameObject cachedGameObject => m_GameObject;
        public Transform cachedTransform => m_Transform;
        public RectTransform cachedRectTransform => m_RectTransform;
        public Canvas cachedCanvas => m_Canvas;
        public CanvasScaler cachedScaler => m_Scaler;
        public GraphicRaycaster cachedRaycaster => m_Raycaster;
        public UICamera cachedCamera => m_UICamera;

        private Dictionary<string, UILevel> m_LevelDic = new Dictionary<string, UILevel>();

        private string[] m_LevelIdentities;
        public string[] levelIdentities => m_LevelIdentities;

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
            if (m_Scaler == null)
            {
                m_Scaler = cachedGameObject.GetOrAddComponent<CanvasScaler>();
            }
            if (m_Raycaster == null)
            {
                m_Raycaster = cachedGameObject.GetOrAddComponent<GraphicRaycaster>();
            }
            if (m_UICamera == null)
            {
                m_UICamera = cachedGameObject.GetOrAddComponent<UICamera>();
            }

            var goName = $"UI {m_Identity} Hierarchy";
            if (cachedGameObject.name != goName)
            {
                cachedGameObject.name = goName;
            }

            if (cachedCanvas.enabled != m_Visible)
            {
                cachedCanvas.enabled = m_Visible;
            }

            if (m_Levels != null && m_Levels.Length > 0)
            {
                m_LevelIdentities = new string[m_Levels.Length];
                for (int i = 0; i < m_Levels.Length; i++)
                {
                    var level = m_Levels[i];
                    if (!string.IsNullOrEmpty(level.identity))
                    {
                        level.Initialize();

                        levelIdentities[i] = level.identity;
                        if (!m_LevelDic.ContainsKey(level.identity))
                        {
                            m_LevelDic.Add(level.identity, level);
                        }
                        else
                        {
                            DLogger.ErrorWithFormat("The identity({0}) of level is exist", level.identity);
                        }
                    }
                    else
                    {
                        DLogger.Error("The identity of stage is empty");
                    }
                }
            }
        }

        public bool HasLevel(string identity)
        {
            return m_LevelDic.ContainsKey(identity);
        }

        public UILevel GetLevel(string identity)
        {
            if (m_LevelDic.TryGetValue(identity, out var level))
            {
                return level;
            }

            return null;
        }

        public void SetLevelVisible(string identity, bool visible)
        {
            if (m_LevelDic.TryGetValue(identity, out var level))
            {
                level.visible = visible;
            }
        }
    }
}
