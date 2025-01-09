namespace DotEngine.UI
{
    public abstract class UIWidget : UIElement
    {
        public UIView view => parent == null ? null : (UIView)parent;
    }
}
