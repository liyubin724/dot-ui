using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIWindow : UIContainer<UIPanel>
    {
        protected override void OnItemAdded(UIPanel child)
        {
        }

        protected override void OnItemRemoved(UIPanel child)
        {
        }
    }
}
