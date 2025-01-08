namespace DotEngine.UI
{
    public interface IUIElement
    {
        string identity { get; set; }

        void Initialize();
        void Activate();
        void Deactivate();
        void Destroy();
    }
}
