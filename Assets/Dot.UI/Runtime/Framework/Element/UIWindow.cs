using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIWindow : UIContainer<UIPanel>
    {
        protected override void OnChildAdded(UIPanel child)
        {
        }

        protected override void OnChildRemoved(UIPanel child)
        {
        }
    }
}
