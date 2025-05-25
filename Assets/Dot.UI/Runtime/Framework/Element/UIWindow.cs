using UnityEngine;
using UnityEngine.UI;
using SystemObject = System.Object;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIWindow : UIContainer<UIPanel>
    {
        public SystemObject userdata { get; set; }

        public override void SetOrderIndex(int index)
        {
        }

        public override void SetOrderAsFirst()
        {
        }

        public override void SetOrderAsLast()
        {
        }

        protected override void OnChildAdded(UIPanel child)
        {
        }

        protected override void OnChildRemoved(UIPanel child)
        {
        }
    }
}
