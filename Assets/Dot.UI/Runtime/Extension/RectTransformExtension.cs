using UnityEngine;

namespace DotEngine.UI
{
    public static class RectTransformExtension
    {
        public static void SetStretchAnchorAll(this RectTransform t)
        {
            t.pivot = Vector2.one * 0.5f;
            t.anchorMin = Vector2.zero;
            t.anchorMax = Vector2.one;
            t.anchoredPosition = Vector2.zero;
            t.sizeDelta = Vector2.zero;
        }

        public static void SetStretchAnchorLeft(this RectTransform t)
        {
            SetStretchAnchorToSide(t, Vector2.up);
        }
        public static void SetStretchAnchorRight(this RectTransform t)
        {
            SetStretchAnchorToFarSide(t, Vector2.right);
        }
        public static void SetStretchAnchorTop(this RectTransform t)
        {
            SetStretchAnchorToFarSide(t, Vector2.up);
        }
        public static void SetStretchAnchorBottom(this RectTransform t)
        {
            SetStretchAnchorToSide(t, Vector2.right);
        }
        static void SetStretchAnchorToSide(this RectTransform t, Vector2 stretch)
        {
            var old_size = t.rect.size;
            var perpendicular = Vector2.one - stretch;
            t.pivot = stretch * 0.5f;
            t.anchorMin = Vector2.zero;
            t.anchorMax = stretch;
            t.anchoredPosition = Vector2.zero;
            t.sizeDelta = Vector2.Scale(perpendicular, old_size);
        }
        static void SetStretchAnchorToFarSide(this RectTransform t, Vector2 stretch)
        {
            var old_size = t.rect.size;
            t.pivot = (Vector2.one + stretch) * 0.5f;
            t.anchorMin = stretch;
            t.anchorMax = Vector2.one;
            t.anchoredPosition = Vector2.zero;
            t.sizeDelta = Vector2.Scale(stretch, old_size);
        }
    }
}
