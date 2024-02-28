using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Empty Image", 10)]
    public class UIEmptyImage : Image
    {
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (sprite == null)
            {
                toFill.Clear();
            }
            else
            {
                base.OnPopulateMesh(toFill);
            }
        }
    }
}
