using UnityEngine.UI;
using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIEmptyGraphic : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }

}
