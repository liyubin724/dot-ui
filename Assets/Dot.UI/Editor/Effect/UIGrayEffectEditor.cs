using DotEngine.UI;
using UnityEditor;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIGrayEffect))]
    public class UIGrayEffectEditor : Editor
    {
        private SerializedProperty m_GrayMatProperty;
        private SerializedProperty m_AnimationTypeProperty;
        private SerializedProperty m_IsGrayProperty;
        private SerializedProperty m_FromGrayProperty;
        private SerializedProperty m_ToGrayProperty;
        private SerializedProperty m_AnimTimeProperty;

        private void OnEnable()
        {
            m_GrayMatProperty = serializedObject.FindProperty("m_GrayMat");
            m_AnimationTypeProperty = serializedObject.FindProperty("m_AnimationType");
            m_IsGrayProperty = serializedObject.FindProperty("m_IsGray");
            m_FromGrayProperty = serializedObject.FindProperty("m_FromGray");
            m_ToGrayProperty = serializedObject.FindProperty("m_ToGray");
            m_AnimTimeProperty = serializedObject.FindProperty("m_AnimTime");
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
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
