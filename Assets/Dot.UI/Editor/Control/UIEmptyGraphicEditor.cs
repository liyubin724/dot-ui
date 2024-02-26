using DotEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIEmptyGraphic))]
    public class UIEmptyGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(m_Script);
                RaycastControlsGUI();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
