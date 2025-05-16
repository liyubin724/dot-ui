using UnityEngine;

namespace DotEngine.UI
{
    public class UIWidgetController : IUIElementController
    {
        public virtual void OnInitialized()
        {
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        public virtual void OnDestroyed()
        {
        }

        public virtual void OnIdentityChanged(string from, string to)
        {
        }


        public virtual void OnLayerChanged(int from, int to)
        {
        }

        public virtual void OnParentChanged(GameObject from, GameObject to)
        {
        }

        public virtual void OnVisibleChanged()
        {
        }
    }
}
