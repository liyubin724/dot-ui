namespace DotEngine.UI
{
    public interface IUIElementController
    {
        void OnInitialized();
        void OnActivated();
        void OnDeactivated();
        void OnDestroyed();
    }
}
