﻿using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIPanel : UIContainer<UIWidget>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnActivated()
        {
            throw new System.NotImplementedException();
        }


        protected override void OnDeactivated()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDestroyed()
        {
            throw new System.NotImplementedException();
        }

        //private UIWindow m_Window;
        //public UIWindow window
        //{
        //    get
        //    {
        //        return m_Window;
        //    }
        //    set
        //    {
        //        if (m_Window != value)
        //        {
        //            if (value == null)
        //            {
        //                OnDetachFromWindow();

        //                m_Window = null;
        //                parent = null;
        //            }
        //            else
        //            {
        //                m_Window = value;
        //                parent = m_Window.gameObject;

        //                OnAttachToWindow();
        //            }
        //        }
        //    }
        //}

        //public override void SetOrderIndex(int index)
        //{
        //    m_Window?.SetChildOrder(this, index);
        //}

        //public override void SetOrderAsFirst()
        //{
        //    m_Window?.SetChildAsFirst(this);
        //}

        //public override void SetOrderAsLast()
        //{
        //    m_Window?.SetChildAsLast(this);
        //}

        //protected override void OnChildAdded(UIWidget child)
        //{
        //    child.panel = this;
        //}

        //protected override void OnChildRemoved(UIWidget child)
        //{
        //    child.panel = null;
        //}

        //protected virtual void OnAttachToWindow()
        //{

        //}

        //protected virtual void OnDetachFromWindow()
        //{

        //}
    }
}
