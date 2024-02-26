using DotEditor.Core.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using SystemObject = System.Object;

namespace DotEditor.UI
{
    public static class AtlasExporter
    {
        public static string[] Export(SpriteAtlas atlas, string dirPath)
        {
            var assetPath = AssetDatabase.GetAssetPath(atlas);

            string platformName = "Standalone";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                platformName = "Android";
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                platformName = "iPhone";
            }

            TextureImporterPlatformSettings tips = atlas.GetPlatformSettings(platformName);
            TextureImporterPlatformSettings cachedTips = new TextureImporterPlatformSettings();
            tips.CopyTo(cachedTips);

            tips.overridden = true;
            tips.format = TextureImporterFormat.RGBA32;
            atlas.SetPlatformSettings(tips);

            List<string> texturePathList = new List<string>();

            SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { atlas }, EditorUserBuildSettings.activeBuildTarget);
            MethodInfo getPreviewTextureMI = typeof(SpriteAtlasExtensions).GetMethod("GetPreviewTextures", BindingFlags.Static | BindingFlags.NonPublic);
            Texture2D[] atlasTextures = (Texture2D[])getPreviewTextureMI.Invoke(null, new SystemObject[] { atlas });
            if (atlasTextures != null && atlasTextures.Length > 0)
            {
                for (int i = 0; i < atlasTextures.Length; i++)
                {
                    Texture2D packTexture = atlasTextures[i];
                    byte[] rawBytes = packTexture.GetRawTextureData();

                    Texture2D nTexture = new Texture2D(packTexture.width, packTexture.height, TextureFormat.RGBA32, false, false, true);
                    nTexture.LoadRawTextureData(rawBytes);
                    nTexture.Apply();
                    string textPath = string.Format("{0}/{1}_{2}.png", dirPath, atlas.name, i);
                    File.WriteAllBytes(textPath, nTexture.EncodeToPNG());

                    texturePathList.Add(textPath);
                }
            }

            atlas.SetPlatformSettings(cachedTips);

            return texturePathList.ToArray();
        }

        private static bool CheckSetting(out string error)
        {
            error = null;

            var packerMode = EditorSettings.spritePackerMode;
            if (packerMode == SpritePackerMode.Disabled)
            {
                error = "The SpritePacker is disabled";
                return false;
            }

            if (packerMode != SpritePackerMode.AlwaysOnAtlas && packerMode != SpritePackerMode.BuildTimeOnlyAtlas)
            {
                error = "Only support for SpriteAtlasV1";
                return false;
            }

            return true;
        }


        [MenuItem("Dot/UI/Atlas/Export Selected", priority = 11)]
        private static void ExportSelectedAltas()
        {
            if (!CheckSetting(out var error))
            {
                EditorUtility.DisplayDialog("Tips", error, "OK");
                return;
            }

            var selectedAtlas = Selection.GetFiltered<SpriteAtlas>(SelectionMode.Assets);
            if (selectedAtlas == null || selectedAtlas.Length == 0)
            {
                EditorUtility.DisplayDialog("Tips", "Please selected a SpriteAtlas", "OK");
                return;
            }

            string dirPath = EditorUtility.OpenFolderPanel("Save Dir", "D:/", "");
            if (string.IsNullOrEmpty(dirPath))
            {
                EditorUtility.DisplayDialog("Tips", "Please Selected a folder", "OK");
                return;
            }

            foreach (var atlas in selectedAtlas)
            {
                Export(atlas, dirPath);
            }
        }

        [MenuItem("Dot/UI/Atlas/Export All", priority = 10)]
        private static void ExportAllAltas()
        {
            if (!CheckSetting(out var error))
            {
                EditorUtility.DisplayDialog("Tips", error, "OK");
                return;
            }

            var allAtlas = AssetDatabaseUtility.FindInstances<SpriteAtlas>();

            string dirPath = EditorUtility.OpenFolderPanel("Save Dir", "D:/", "");
            if (string.IsNullOrEmpty(dirPath))
            {
                EditorUtility.DisplayDialog("Tips", "Please Selected a folder", "OK");
                return;
            }

            foreach (var atlas in allAtlas)
            {
                Export(atlas, dirPath);
            }
        }
    }
}
