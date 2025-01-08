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
        private string m_Alias;
        [SerializeField]
        private bool m_Visible = true;
        [SerializeField]
        public UICamera m_UICamera;
        [SerializeField]
        private UILayer[] m_Layers = new UILayer[0];

        public string alias
        {
            get
            {
                return m_Alias;
            }
            set
            {
                if (m_Alias == value) return;

                m_Alias = value;
                name = value;
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

        private Dictionary<string, UILayer> m_NameToLayerDic = new Dictionary<string, UILayer>();
        private Dictionary<UILayerLevel, UILayer> m_LevelToLayerDic = new Dictionary<UILayerLevel, UILayer>();

        private void Awake()
        {
            m_GameObject = base.gameObject;
            m_Transform = base.transform;
            m_RectTransform = (RectTransform)base.transform;

            m_Canvas = GetComponent<Canvas>();
            m_CanvasScaler = GetComponent<CanvasScaler>();
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();

            if (m_Layers != null && m_Layers.Length > 0)
            {
                foreach (var layer in m_Layers)
                {
                    if (!m_NameToLayerDic.ContainsKey(layer.name))
                    {
                        m_NameToLayerDic.Add(layer.name, layer);
                    }
                    else
                    {
                        Debug.LogError($"The name({layer.name}) of layer has been added");
                    }

                    if (!m_LevelToLayerDic.ContainsKey(layer.level))
                    {
                        m_LevelToLayerDic.Add(layer.level, layer);
                    }
                    else
                    {
                        Debug.LogError($"The level({layer.level}) of layer has been added");
                    }
                }
            }

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

        public UILayer GetLayer(string alias)
        {
            if (m_NameToLayerDic.TryGetValue(alias, out var layer))
            {
                return layer;
            }
            return null;
        }

        public UILayer GetLayer(UILayerLevel level)
        {
            if (m_LevelToLayerDic.TryGetValue(level, out var layer))
            {
                return layer;
            }
            return null;
        }

        public UILayer[] GetLayers(string[] aliases)
        {
            if (aliases == null || aliases.Length == 0)
            {
                return null;
            }

            UILayer[] layers = new UILayer[aliases.Length];
            for (int i = 0; i < aliases.Length; i++)
            {
                layers[i] = GetLayer(aliases[i]);
            }
            return layers;
        }

        public UILayer[] GetLayers(UILayerLevel[] levels)
        {
            if (levels == null || levels.Length == 0)
            {
                return null;
            }

            UILayer[] layers = new UILayer[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                layers[i] = GetLayer(levels[i]);
            }
            return layers;
        }

        public void SetLayersVisible(UILayerLevel[] levels, bool visible)
        {
            if (levels == null || levels.Length == 0)
            {
                return;
            }

            foreach (var level in levels)
            {
                var layer = GetLayer(level);
                layer?.SetVisible(visible);
            }
        }
    }
}
