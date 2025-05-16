namespace DotEngine.UI
{
    public abstract class UIPanel : UIContainer<UIView, IUIPanelController>
    {
        public UIWindow window => parent == null ? null : (UIWindow)parent;
    }
}
