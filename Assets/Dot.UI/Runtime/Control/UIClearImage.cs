using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Clear Image", 10)]
    public class UIClearImage : Image
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
