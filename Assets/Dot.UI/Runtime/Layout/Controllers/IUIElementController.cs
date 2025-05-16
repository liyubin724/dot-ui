using UnityEngine;

namespace DotEngine.UI
{
    public interface IUIElementController
    {
        void OnInitialized();
        void OnActivated();
        void OnDeactivated();
        void OnDestroyed();

        void OnIdentityChanged(string from, string to);
        void OnLayerChanged(int from, int to);
        void OnVisibleChanged();
        void OnParentChanged(GameObject from, GameObject to);
    }
}
