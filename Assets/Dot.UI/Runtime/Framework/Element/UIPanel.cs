namespace DotEngine.UI
{
    public class UIPanel : UIContainer<UIWidget>
    {
        protected override void OnItemAdded(UIWidget item)
        {
            item.panel = this;
        }

        protected override void OnItemRemoved(UIWidget item)
        {
            item.panel = null;
        }
    }
}
