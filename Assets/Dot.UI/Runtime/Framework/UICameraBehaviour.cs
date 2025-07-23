using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Camera))]
    public class UICameraBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera m_CachedCamera;

        public Camera cachedCamera
        {
            get
            {
                return m_CachedCamera;
            }
            set
            {
                m_CachedCamera = value;
            }
        }

        private void Awake()
        {
            if (m_CachedCamera == null)
            {
                m_CachedCamera = GetComponent<Camera>();
            }
        }
    }
}
