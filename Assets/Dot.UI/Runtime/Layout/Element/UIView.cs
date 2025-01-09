namespace DotEngine.UI
{
    public abstract class UIView : UIContainer<UIWidget, IUIViewController>
    {
        public UIPanel panel => parent == null ? null : (UIPanel)parent;
    }
}
