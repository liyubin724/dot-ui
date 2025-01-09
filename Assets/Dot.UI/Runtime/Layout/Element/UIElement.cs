using UnityEngine;

namespace DotEngine.UI
{
    public enum UIElementState
    {
        None = 0,

        Initialized = 1,
        Activated = 2,
        Deactivated = 3,
        Destroyed = 4,
    }

    public class UIElement : MonoBehaviour
    {
        [SerializeField]
        private string m_Identity;
        public string identity => m_Identity;

        public UIElement parent { get; private set; } = null;
        public bool isActive { get; private set; } = false;

        public virtual void Initialize()
        {

        }

        public virtual void Activate()
        {

        }

        public virtual void Deactivate()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
