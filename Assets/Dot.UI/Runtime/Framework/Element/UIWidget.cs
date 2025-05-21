using UnityEngine;

namespace DotEngine.UI
{
    public class UIWidget : UIElement
    {
        private UIPanel m_Panel;
        public UIPanel panel
        {
            get
            {
                return m_Panel;
            }
            set
            {
                if (m_Panel != value)
                {
                    if (value == null)
                    {
                        OnDetachFromPanel();

                        m_Panel = null;
                        parent = null;
                    }
                    else
                    {
                        m_Panel = value;
                        parent = m_Panel.gameObject;

                        OnAttachToPanel();
                    }
                }
            }
        }

        public override void SetOrderIndex(int index)
        {
            m_Panel?.SetChildOrder(this, index);
        }

        public override void SetOrderAsFirst()
        {
            m_Panel?.SetChildAsFirst(this);
        }

        public override void SetOrderAsLast()
        {
            m_Panel?.SetChildAsLast(this);
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

        protected override void OnIdentityChanged(string from, string to)
        {

        }

        protected override void OnLayerChanged(int from, int to)
        {

        }

        protected override void OnVisibleChanged()
        {

        }

        protected override void OnParentChanged(GameObject from, GameObject to)
        {

        }

        protected virtual void OnAttachToPanel()
        {

        }

        protected virtual void OnDetachFromPanel()
        {

        }
    }
}
