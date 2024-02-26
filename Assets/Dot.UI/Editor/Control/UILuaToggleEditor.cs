#if ENABLE_LUA
using DotEditor.Core.GUI.IMGUI.RList;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [CustomEditor(typeof(UILuaToggle), true)]
    public class UILuaToggleEditor : SelectableEditor
    {
        SerializedProperty binderBehaviourProperty;

        SerializedProperty changedFuncNameProperty;
        SerializedProperty changedParamValuesProperty;

        ReorderableListProperty changedParamValuesRLProperty;


        SerializedProperty m_TransitionProperty;
        SerializedProperty m_GraphicProperty;
        SerializedProperty m_GroupProperty;
        SerializedProperty m_IsOnProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            binderBehaviourProperty = serializedObject.FindProperty("binderBehaviour");

            changedFuncNameProperty = serializedObject.FindProperty("changedFuncName");
            changedParamValuesProperty = serializedObject.FindProperty("changedParamValues");

            changedParamValuesRLProperty = new ReorderableListProperty(changedParamValuesProperty);

            m_TransitionProperty = serializedObject.FindProperty("toggleTransition");
            m_GraphicProperty = serializedObject.FindProperty("graphic");
            m_GroupProperty = serializedObject.FindProperty("m_Group");
            m_IsOnProperty = serializedObject.FindProperty("m_IsOn");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(binderBehaviourProperty);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(changedFuncNameProperty);
                changedParamValuesRLProperty.OnGUILayout();
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            Toggle toggle = serializedObject.targetObject as Toggle;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_IsOnProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(toggle.gameObject.scene);

                ToggleGroup group = m_GroupProperty.objectReferenceValue as ToggleGroup;

                toggle.isOn = m_IsOnProperty.boolValue;

                if (group != null && group.isActiveAndEnabled && toggle.IsActive())
                {
                    if (toggle.isOn || (!group.AnyTogglesOn() && !group.allowSwitchOff))
                    {
                        toggle.isOn = true;
                        group.NotifyToggleOn(toggle);
                    }
                }
            }
            EditorGUILayout.PropertyField(m_TransitionProperty);
            EditorGUILayout.PropertyField(m_GraphicProperty);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_GroupProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(toggle.gameObject.scene);

                ToggleGroup group = m_GroupProperty.objectReferenceValue as ToggleGroup;
                toggle.group = group;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif