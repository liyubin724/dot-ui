namespace DotEngine.UI
{
    public interface IUIPanel : IUIElement
    {
        IUIWindow window { get; }
        IUIPanel[] panels { get; }

        void AttachToWindow(IUIWindow window);
        void DetachFromWindow();

        void AddView(IUIView view);
        void RemoveView(IUIView view);
    }
}
