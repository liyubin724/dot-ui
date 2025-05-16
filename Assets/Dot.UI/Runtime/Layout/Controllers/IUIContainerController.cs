namespace DotEngine.UI
{
    public interface IUIContainerController<TChild> where TChild : UIElement<IUIElementController>
    {
        void OnAddChild(TChild child);
        void OnRemoveChild(TChild child);
    }
}
