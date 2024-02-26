using DotEditor.Core.GUI;
using UnityEditor;

namespace DotEditor.UI.Atlas
{
    [CustomEditor(typeof(AtlasPackerSetting))]
    public class AtlasPackerSettingEditor : Editor
    {
        SerializedProperty atlasDirPath = null;
        SerializedProperty pixelsPerUnit = null;
        SerializedProperty isRotation = null;
        SerializedProperty isTightPacking = null;
        SerializedProperty padding = null;
        SerializedProperty isReadOrWrite = null;
        SerializedProperty isMipmap = null;
        SerializedProperty isSRGB = null;
        SerializedProperty filterMode = null;
        SerializedProperty maxSize = null;
        SerializedProperty winTextureFormat = null;
        SerializedProperty androidTextureFormat = null;
        SerializedProperty iosTextureFormat = null;

        private int[] atlasMaxSizes = new int[]
        {
            128,256,512,1024,2048,4096,
        };
        private string[] atlasMaxSizeContents = new string[]
        {
            "128*128","256*256","512*512","1024*1024","2048*2048","4096*4096",
        };

        private int[] winAtlasFormats = new int[]
        {
            (int)TextureImporterFormat.RGBA32,
            (int)TextureImporterFormat.ARGB16,
            (int)TextureImporterFormat.DXT5,
            (int)TextureImporterFormat.DXT5Crunched,
        };
        private string[] winAtlasFormatContents = new string[]
        {
            TextureImporterFormat.RGBA32.ToString(),
            TextureImporterFormat.RGBA16.ToString(),
            TextureImporterFormat.DXT5.ToString(),
            TextureImporterFormat.DXT5Crunched.ToString(),
    };

        private int[] androidAtlasFormats = new int[]
        {
            (int)TextureImporterFormat.RGBA32,
            (int) TextureImporterFormat.RGBA16,
            (int) TextureImporterFormat.ETC2_RGBA8,
            (int) TextureImporterFormat.ETC2_RGBA8Crunched
        };
        private string[] androidAtlasFormatContents = new string[]
        {
            TextureImporterFormat.RGBA32.ToString(),
            TextureImporterFormat.RGBA16.ToString(),
            TextureImporterFormat.ETC2_RGBA8.ToString(),
            TextureImporterFormat.ETC2_RGBA8Crunched.ToString(),
        };

        private int[] iosAtlasFormats = new int[]
        {
            (int)TextureImporterFormat.RGBA32,
            (int) TextureImporterFormat.RGBA16,
            (int) TextureImporterFormat.ASTC_4x4,
            (int) TextureImporterFormat.ASTC_6x6,
            (int) TextureImporterFormat.ASTC_8x8,
            (int) TextureImporterFormat.ASTC_12x12
        };
        private string[] iosAtlasFormatContents = new string[]
        {
            TextureImporterFormat.RGBA32.ToString(),
            TextureImporterFormat.RGBA16.ToString(),
            TextureImporterFormat.ASTC_4x4.ToString(),
            TextureImporterFormat.ASTC_6x6.ToString(),
            TextureImporterFormat.ASTC_8x8.ToString(),
            TextureImporterFormat.ASTC_12x12.ToString(),
        };

        private void OnEnable()
        {
            atlasDirPath = serializedObject.FindProperty("atlasDirPath");
            pixelsPerUnit = serializedObject.FindProperty("pixelsPerUnit");
            isRotation = serializedObject.FindProperty("isRotation");
            isTightPacking = serializedObject.FindProperty("isTightPacking");
            padding = serializedObject.FindProperty("padding");
            isReadOrWrite = serializedObject.FindProperty("isReadOrWrite");
            isMipmap = serializedObject.FindProperty("isMipmap");
            isSRGB = serializedObject.FindProperty("isSRGB");
            filterMode = serializedObject.FindProperty("filterMode");
            maxSize = serializedObject.FindProperty("maxSize");
            winTextureFormat = serializedObject.FindProperty("winTextureFormat");
            androidTextureFormat = serializedObject.FindProperty("androidTextureFormat");
            iosTextureFormat = serializedObject.FindProperty("iosTextureFormat");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();
            {
                atlasDirPath.stringValue = EGUILayout.DrawAssetFolderSelection("Atlas Save Dir", atlasDirPath.stringValue, true);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(pixelsPerUnit);
                EditorGUILayout.PropertyField(isRotation);
                EditorGUILayout.PropertyField(isTightPacking);
                EditorGUILayout.PropertyField(padding);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(isReadOrWrite);
                EditorGUILayout.PropertyField(isMipmap);
                EditorGUILayout.PropertyField(isSRGB);
                EditorGUILayout.PropertyField(filterMode);
                EditorGUILayout.Space();

                maxSize.intValue = EGUILayout.DrawPopup<int>("Max Size", atlasMaxSizeContents, atlasMaxSizes, maxSize.intValue);
                winTextureFormat.intValue = EGUILayout.DrawPopup<int>("Win Format", winAtlasFormatContents, winAtlasFormats, winTextureFormat.intValue);
                androidTextureFormat.intValue = EGUILayout.DrawPopup<int>("Android Format", androidAtlasFormatContents, androidAtlasFormats, androidTextureFormat.intValue);
                iosTextureFormat.intValue = EGUILayout.DrawPopup<int>("iOS Format", iosAtlasFormatContents, iosAtlasFormats, iosTextureFormat.intValue);
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
