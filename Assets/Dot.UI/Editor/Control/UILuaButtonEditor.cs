#if ENABLE_LUA
using DotEditor.Core.GUI.IMGUI.RList;
using UnityEditor;
using UnityEditor.UI;

namespace DotEngine.UI
{
    [CustomEditor(typeof(UILuaButton), true)]
    public class UILuaButtonEditor : SelectableEditor
    {
        SerializedProperty binderBehaviourProperty;
        SerializedProperty clickedFuncNameProperty;
        SerializedProperty paramValuesProperty;

        ReorderableListProperty paramValuesRLProperty;
        protected override void OnEnable()
        {
            base.OnEnable();
            binderBehaviourProperty = serializedObject.FindProperty("behaviour");
            clickedFuncNameProperty = serializedObject.FindProperty("funcName");
            paramValuesProperty = serializedObject.FindProperty("funcValues");

            paramValuesRLProperty = new ReorderableListProperty(paramValuesProperty);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(binderBehaviourProperty);
                EditorGUILayout.PropertyField(clickedFuncNameProperty);

                paramValuesRLProperty.OnGUILayout();
            }
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}
#endif