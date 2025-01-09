using DotEngine.Core.Utilities;

namespace DotEngine.UI
{
    public abstract class UIWindow : UIContainer<UIPanel, IUIWindowController>
    {
        private string m_Guid;
        private string guid => m_Guid;

        private UILayerLevel m_LayerLevel = UILayerLevel.Default;
        public UILayerLevel layerLevel => m_LayerLevel;

        private void Awake()
        {
            m_Guid = GuidUtility.CreateNew();
        }
    }
}
