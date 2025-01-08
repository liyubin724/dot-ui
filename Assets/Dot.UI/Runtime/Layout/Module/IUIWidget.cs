namespace DotEngine.UI
{
    public interface IUIWidget : IUIElement
    {
        IUIView view { get; }

        void AttachToView(UIView view);
        void DetachFromView();
    }
}
