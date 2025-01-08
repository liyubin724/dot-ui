namespace DotEngine.UI
{
    public class UIWidget : UIElement
    {
        private UIView m_View = null;
        public UIView view => m_View;

        public virtual void AttachToView(UIView view)
        {
            m_View = view;
        }

        public virtual void DetachFromView()
        {
            m_View = null;
        }
    }
}
