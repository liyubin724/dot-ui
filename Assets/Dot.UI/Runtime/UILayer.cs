using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class UILayer : MonoBehaviour
    {
        public GameObject cachedGameObject;
        public RectTransform cachedTransform;

        private void Awake()
        {
            cachedGameObject = gameObject;
            cachedTransform = GetComponent<RectTransform>();
        }
    }
}
