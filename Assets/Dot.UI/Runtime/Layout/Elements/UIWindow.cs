namespace DotEngine.UI
{
    public abstract class UIWindow : UIContainer<UIPanel, IUIWindowController>
    {
        private string m_Guid;
        private string guid => m_Guid;

        private UILayerLevel m_Level = UILayerLevel.Default;
        public UILayerLevel level => m_Level;
    }
}
