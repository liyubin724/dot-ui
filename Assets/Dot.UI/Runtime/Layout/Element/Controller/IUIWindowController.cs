namespace DotEngine.UI
{
    public interface IUIWindowController : IUIElementController
    {
        void OnPanelAdded(UIPanel panel);
        void OnPanelRemoved(UIPanel panel);
    }
}
