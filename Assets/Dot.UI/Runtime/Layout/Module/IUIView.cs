namespace DotEngine.UI
{
    public interface IUIView : IUIElement
    {
        IUIPanel panel { get; }
        IUIWidget[] widgets { get; }

        void AttachToPanel(UIPanel panel);
        void RemoveFromPanel();

        bool HasWidget(string identity);
        bool HasWidget<T>(string identity) where T : IUIWidget;
        IUIWidget GetWidget(string identity);
        T GetWidget<T>(string identity) where T : IUIWidget;
        IUIWidget[] GetWidgets(string identity);
        T[] GetWidgets<T>(string identity) where T : IUIWidget;
        void AddWidget(UIWidget widget);
        void RemoveWidget(UIWidget widget);
        void RemoveWidget(string identity);

    }
}
