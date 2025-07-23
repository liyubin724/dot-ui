namespace DotEngine.UI
{
    public class UIWidget : UIElement
    {
        private UIPanel m_Panel;

        public UIPanel panel => m_Panel;

        public void AttachToPanel(UIPanel panel)
        {
            if (m_Panel == panel)
            {
                return;
            }

            if (m_Panel != null)
            {
                DetachFromPanel();
            }

            m_Panel = panel;
            OnAttachedToPanel();
        }

        protected virtual void OnAttachedToPanel() { }

        public void DetachFromPanel()
        {
            if (m_Panel == null)
            {
                return;
            }

            OnDetachedFromPanel();

            m_Panel = null;
        }

        protected virtual void OnDetachedFromPanel()
        {

        }

        protected override void OnInitialized()
        {

        }

        protected override void OnActivated()
        {

        }

        protected override void OnDeactivated()
        {

        }

        protected override void OnDestroyed()
        {

        }
    }
}
