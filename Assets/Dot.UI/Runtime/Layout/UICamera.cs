using UnityEngine;

namespace DotEngine.UI.Layout
{
    [RequireComponent(typeof(Camera))]
    public class UICamera : MonoBehaviour
    {
        [SerializeField]
        private Camera m_Camera;

        public new Camera camera
        {
            get
            {
                return m_Camera;
            }
        }

        private void Awake()
        {
            if (m_Camera == null)
            {
                m_Camera = GetComponent<Camera>();
            }
        }
    }
}
