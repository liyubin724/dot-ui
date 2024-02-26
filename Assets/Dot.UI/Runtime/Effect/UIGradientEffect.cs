using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Effects/Gradient")]
    [RequireComponent(typeof(Graphic))]
    public class UIGradientEffect : BaseMeshEffect
    {
        [SerializeField]
        private Color32 m_LeftTopColor = Color.white;
        [SerializeField]
        private Color32 m_LeftBottomColor = Color.white;
        [SerializeField]
        private Color32 m_RightTopColor = Color.white;
        [SerializeField]
        private Color32 m_RightBottomColor = Color.white;

        private List<UIVertex> vertexs = new List<UIVertex>();
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            var vertexCount = vh.currentVertCount;
            if (vertexCount == 0)
            {
                return;
            }

            vertexs.Clear();

            for (var i = 0; i < vertexCount; i++)
            {
                var vertex = new UIVertex();
                vh.PopulateUIVertex(ref vertex, i);
                vertexs.Add(vertex);
            }

            var topY = vertexs[0].position.y;
            var bottomY = vertexs[0].position.y;
            var leftX = vertexs[0].position.x;
            var rightX = vertexs[0].position.x;

            for (var i = 1; i < vertexCount; i++)
            {
                var y = vertexs[i].position.y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }

                var x = vertexs[i].position.x;
                if (x > leftX)
                {
                    leftX = x;
                }
                else if (x < rightX)
                {
                    rightX = x;
                }
            }

            var height = topY - bottomY;
            var width = rightX - leftX;
            for (var i = 0; i < vertexCount; i++)
            {
                var vertex = vertexs[i];

                var rateY = (vertex.position.y - bottomY) / height;
                var color1 = Color32.Lerp(m_RightBottomColor, m_RightTopColor, rateY);
                var color2 = Color32.Lerp(m_LeftBottomColor, m_LeftTopColor, rateY);

                var rateX = (vertex.position.x - leftX) / width;

                vertex.color = Color32.Lerp(color1, color2, rateX);

                vh.SetUIVertex(vertex, i);
            }
        }
    }

}