using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIWindow : UIContainer<UIPanel>
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

        //public SystemObject userdata { get; set; }

        //public override void SetOrderIndex(int index)
        //{
        //}

        //public override void SetOrderAsFirst()
        //{
        //}

        //public override void SetOrderAsLast()
        //{
        //}

        //protected override void OnChildAdded(UIPanel child)
        //{
        //}

        //protected override void OnChildRemoved(UIPanel child)
        //{
        //}
    }
}
