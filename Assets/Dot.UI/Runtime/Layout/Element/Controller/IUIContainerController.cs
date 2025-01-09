namespace DotEngine.UI
{
    public interface IUIContainerController<T> : IUIElementController where T : UIElement
    {
        void OnElementAdded(T element);
        void OnElementRemoved(T element);
    }
}
