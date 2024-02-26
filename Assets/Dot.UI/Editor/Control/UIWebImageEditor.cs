using DotEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIWebImage))]
    public class UIWebImageEditor : GraphicEditor
    {
        SerializedProperty m_ImageUrlProperty;
        SerializedProperty m_TimeoutProperty;
        SerializedProperty m_FinishedActionProperty;
        SerializedProperty m_IsSetNativeSizeProperty;

        SerializedProperty m_Texture;
        SerializedProperty m_UVRect;
        GUIContent m_UVRectContent;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_ImageUrlProperty = serializedObject.FindProperty("m_ImageUrl");
            m_TimeoutProperty = serializedObject.FindProperty("m_Timeout");
            m_IsSetNativeSizeProperty = serializedObject.FindProperty("m_IsSetNativeSize");
            m_FinishedActionProperty = serializedObject.FindProperty("m_FinishedAction");

            m_UVRectContent = EditorGUIUtility.TrTextContent("UV Rect");

            m_Texture = serializedObject.FindProperty("m_Texture");
            m_UVRect = serializedObject.FindProperty("m_UVRect");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_ImageUrlProperty);
            EditorGUILayout.PropertyField(m_TimeoutProperty);
            EditorGUILayout.PropertyField(m_IsSetNativeSizeProperty);

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.PropertyField(m_Texture);
            }
            EditorGUI.EndDisabledGroup();

            AppearanceControlsGUI();
            RaycastControlsGUI();
            MaskableControlsGUI();
            EditorGUILayout.PropertyField(m_UVRect, m_UVRectContent);
            SetShowNativeSize(false);
            NativeSizeButtonGUI();

            EditorGUILayout.PropertyField(m_FinishedActionProperty);

            serializedObject.ApplyModifiedProperties();
        }

        void SetShowNativeSize(bool instant)
        {
            base.SetShowNativeSize(m_Texture.objectReferenceValue != null, instant);
        }

        private static Rect Outer(RawImage rawImage)
        {
            Rect outer = rawImage.uvRect;
            outer.xMin *= rawImage.rectTransform.rect.width;
            outer.xMax *= rawImage.rectTransform.rect.width;
            outer.yMin *= rawImage.rectTransform.rect.height;
            outer.yMax *= rawImage.rectTransform.rect.height;
            return outer;
        }

        public override bool HasPreviewGUI()
        {
            RawImage rawImage = target as RawImage;
            if (rawImage == null)
                return false;

            var outer = Outer(rawImage);
            return outer.width > 0 && outer.height > 0;
        }

        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
            RawImage rawImage = target as RawImage;
            Texture tex = rawImage.mainTexture;

            if (tex == null)
                return;

            var outer = Outer(rawImage);
            SpriteDrawUtility.DrawSprite(tex, rect, outer, rawImage.uvRect, rawImage.canvasRenderer.GetColor());
        }

        public override string GetInfoString()
        {
            RawImage rawImage = target as RawImage;

            // Image size Text
            string text = string.Format("RawImage Size: {0}x{1}",
                Mathf.RoundToInt(Mathf.Abs(rawImage.rectTransform.rect.width)),
                Mathf.RoundToInt(Mathf.Abs(rawImage.rectTransform.rect.height)));

            return text;
        }
    }
}
