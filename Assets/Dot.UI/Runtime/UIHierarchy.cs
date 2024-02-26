using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [Serializable]
    public class UILayerData
    {
        public string name;
        public UILayer layer;
    }

    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class UIHierarchy : MonoBehaviour
    {
        public Camera uiCamera;

        public Canvas canvas;
        public UILayerData[] layers;

        private Dictionary<string, UILayer> m_LayerDic = new Dictionary<string, UILayer>();
        private void Awake()
        {
            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
            }

            if (layers == null)
            {
                var layerComponents = GetComponentsInChildren<UILayer>(true);
                if (layerComponents != null && layerComponents.Length > 0)
                {
                    layers = new UILayerData[layerComponents.Length];
                    for (int i = 0; i < layerComponents.Length; i++)
                    {
                        layers[i] = new UILayerData()
                        {
                            name = layerComponents[i].name,
                            layer = layerComponents[i]
                        };
                    }
                }
            }
            if (layers != null)
            {
                for (int i = 0; i < layers.Length; i++)
                {
                    m_LayerDic.Add(layers[i].name, layers[i].layer);
                }
            }
        }

        public UILayer GetLayer(string layerName)
        {
            return m_LayerDic[layerName];
        }

        public void SetLayerVisible(string layerName, bool visible)
        {
            var layer = GetLayer(layerName);
            if (layer != null)
            {
                layer.cachedGameObject.SetActive(visible);
            }
        }

        public bool GetLayerVisible(string layerName)
        {
            var layer = GetLayer(layerName);
            if (layer != null)
            {
                return layer.cachedGameObject.activeSelf;
            }

            return false;
        }

        public RectTransform GetLayerTransform(string layerName)
        {
            var layer = GetLayer(layerName);
            if (layer != null)
            {
                return layer.cachedTransform;
            }

            return null;
        }
    }
}
