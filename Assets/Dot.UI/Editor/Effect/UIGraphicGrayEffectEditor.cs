using DotEngine.UI;
using UnityEditor;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIGrayGroupEffect))]
    public class UIGraphicGrayEffectEditor : Editor
    {
        private SerializedProperty m_GrayMatProperty;
        private SerializedProperty m_AnimationTypeProperty;
        private SerializedProperty m_IsGrayProperty;
        private SerializedProperty m_FromGrayProperty;
        private SerializedProperty m_ToGrayProperty;
        private SerializedProperty m_AnimTimeProperty;

        private SerializedProperty m_IsAllGraphicProperty;
        private SerializedProperty m_GraphicsProperty;

        private void OnEnable()
        {
            m_GrayMatProperty = serializedObject.FindProperty("m_GrayMat");
            m_AnimationTypeProperty = serializedObject.FindProperty("m_AnimationType");
            m_IsGrayProperty = serializedObject.FindProperty("m_IsGray");
            m_FromGrayProperty = serializedObject.FindProperty("m_FromGray");
            m_ToGrayProperty = serializedObject.FindProperty("m_ToGray");
            m_AnimTimeProperty = serializedObject.FindProperty("m_AnimTime");

            m_IsAllGraphicProperty = serializedObject.FindProperty("m_IsAllGraphic");
            m_GraphicsProperty = serializedObject.FindProperty("m_Graphics");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(m_GrayMatProperty);
                EditorGUILayout.PropertyField(m_AnimationTypeProperty);
                if (m_AnimationTypeProperty.intValue == (int)UIGrayAnimationType.None)
                {
                    EditorGUILayout.PropertyField(m_IsGrayProperty);
                }
                else
                {
                    EditorGUILayout.PropertyField(m_FromGrayProperty);
                    EditorGUILayout.PropertyField(m_ToGrayProperty);
                    EditorGUILayout.PropertyField(m_AnimTimeProperty);
                }

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(m_IsAllGraphicProperty);
                if (!m_IsAllGraphicProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(m_GraphicsProperty);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
