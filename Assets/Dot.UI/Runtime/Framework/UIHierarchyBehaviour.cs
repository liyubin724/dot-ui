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
    public class UIHierarchyBehaviour : MonoBehaviour
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
        private UILevelBehaviour[] m_Levels = new UILevelBehaviour[0];

        [SerializeField]
        [HideInInspector]
        private GameObject m_CachedGameObject;
        [SerializeField]
        [HideInInspector]
        private Transform m_CachedTransform;
        [SerializeField]
        [HideInInspector]
        private RectTransform m_CachedRectTransform;

        [SerializeField]
        [HideInInspector]
        private Canvas m_CachedCanvas;
        [SerializeField]
        [HideInInspector]
        private CanvasScaler m_CachedScaler;
        [SerializeField]
        [HideInInspector]
        private GraphicRaycaster m_CachedRaycaster;

        [SerializeField]
        public UICameraBehaviour m_CachedCamera;

        public GameObject cachedGameObject => m_CachedGameObject;
        public Transform cachedTransform => m_CachedTransform;
        public RectTransform cachedRectTransform => m_CachedRectTransform;
        public Canvas cachedCanvas => m_CachedCanvas;
        public CanvasScaler cachedScaler => m_CachedScaler;
        public GraphicRaycaster cachedRaycaster => m_CachedRaycaster;
        public UICameraBehaviour cachedCamera => m_CachedCamera;

        private Dictionary<string, UILevelBehaviour> m_LevelDic = new Dictionary<string, UILevelBehaviour>();

        private string[] m_LevelIdentities;
        public string[] levelIdentities => m_LevelIdentities;

        public void Initialize()
        {
            if (m_CachedGameObject == null)
            {
                m_CachedGameObject = gameObject;
            }
            if (m_CachedTransform == null)
            {
                m_CachedTransform = transform;
            }
            if (m_CachedRectTransform == null)
            {
                m_CachedRectTransform = (RectTransform)transform;
            }
            if (m_CachedCanvas == null)
            {
                m_CachedCanvas = cachedGameObject.GetOrAddComponent<Canvas>();
            }
            if (m_CachedScaler == null)
            {
                m_CachedScaler = cachedGameObject.GetOrAddComponent<CanvasScaler>();
            }
            if (m_CachedRaycaster == null)
            {
                m_CachedRaycaster = cachedGameObject.GetOrAddComponent<GraphicRaycaster>();
            }
            if (m_CachedCamera == null)
            {
                m_CachedCamera = cachedGameObject.GetOrAddComponent<UICameraBehaviour>();
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

        public UILevelBehaviour GetLevel(string identity)
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
