using DotEditor.Core.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace DotEditor.UI
{
    public static class AtlasPackerUtil
    {
        private const string DEFAULT_PACKER_SETTING_NAME = "atlas-packer-setting.asset";

        public static AtlasPackerSetting LoadSetting(bool createIfNotExist = true)
        {
            var setting = AssetDatabaseUtility.FindInstance<AtlasPackerSetting>();
            if (setting == null)
            {
                setting = ScriptableObject.CreateInstance<AtlasPackerSetting>();
                setting.hideFlags = HideFlags.DontSave;
            }

            return setting;
        }

        public static AtlasPackerSetting LoadSetting(string assetDir)
        {
            string[] settingPaths = AssetDatabaseUtility.FindAssetInFolder<AtlasPackerSetting>(assetDir);
            if (settingPaths != null && settingPaths.Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath<AtlasPackerSetting>(settingPaths[0]);
            }

            return LoadSetting();
        }

        [MenuItem("Dot/UI/Atlas/Create Packer Setting", priority = 1)]
        public static void CreateSpriteAtlasSetting()
        {
            string[] selectedDirs = SelectionUtility.GetSelectionDirs();
            if (selectedDirs == null || selectedDirs.Length == 0)
            {
                EditorUtility.DisplayDialog("Warning", "Please selected a directory", "OK");
                return;
            }
            foreach (var dir in selectedDirs)
            {
                string assetPath = $"{dir}/{DEFAULT_PACKER_SETTING_NAME}";
                AtlasPackerSetting setting = AssetDatabase.LoadAssetAtPath<AtlasPackerSetting>(assetPath);
                if (setting == null)
                {
                    setting = ScriptableObject.CreateInstance<AtlasPackerSetting>();
                    AssetDatabase.CreateAsset(setting, assetPath);
                    AssetDatabase.ImportAsset(assetPath);
                }
            }
        }

        [MenuItem("Dot/UI/Atlas/Pack Atlas", priority = 2)]
        public static void AutoPackSelectedAtlas()
        {
            string[] selectedDirs = SelectionUtility.GetSelectionDirs();
            if (selectedDirs == null || selectedDirs.Length == 0)
            {
                EditorUtility.DisplayDialog("Warning", "Please selected a directory", "OK");
                return;
            }

            List<SpriteAtlas> atlasList = new List<SpriteAtlas>();
            foreach (string dir in selectedDirs)
            {
                AtlasPackerSetting setting = LoadSetting(dir);

                SpriteAtlas atlas = PackSpriteAtlas(dir, setting);
                if (atlas != null)
                {
                    atlasList.Add(atlas);
                }
            }
            SelectionUtility.ActiveObjects(atlasList.ToArray());
        }

        public static SpriteAtlas PackSpriteAtlas(string spriteAssetInputDir, AtlasPackerSetting setting)
        {
            var atlasDirPath = setting.atlasDirPath;
            if (string.IsNullOrEmpty(atlasDirPath))
            {
                atlasDirPath = EditorUtility.OpenFolderPanel("Save Dir", Application.dataPath, "");
                if (string.IsNullOrEmpty(atlasDirPath))
                {
                    EditorUtility.DisplayDialog("Tips", "Please Selected a folder", "OK");
                    return null;
                }

                atlasDirPath = PathUtility.GetAssetPath(atlasDirPath);
            }

            if (AssetDatabase.IsValidFolder(atlasDirPath))
            {
                string atlasName = spriteAssetInputDir.Substring(spriteAssetInputDir.LastIndexOf("/") + 1).ToLower();
                string[] assetPaths = DirectoryUtility.GetAssetsByFileNameFilter(spriteAssetInputDir, true, null, new string[] { ".meta" });

                List<Sprite> sprites = new List<Sprite>();
                foreach (var assetPath in assetPaths)
                {
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
                    if (texture != null)
                    {
                        SetTextureToSprite(assetPath, setting);
                        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                        sprites.Add(s);
                    }
                }

                if (sprites.Count == 0)
                {
                    return null;
                }

                string atlasAssetPath = string.Format("{0}/{1}_atlas.spriteatlas", atlasDirPath, atlasName);
                SpriteAtlas packedAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasAssetPath);
                if (packedAtlas == null)
                {
                    packedAtlas = new SpriteAtlas();
                    AssetDatabase.CreateAsset(packedAtlas, atlasAssetPath);
                }
                packedAtlas.Remove(packedAtlas.GetPackables());
                EditorUtility.SetDirty(packedAtlas);
                AssetDatabase.SaveAssets();

                packedAtlas.Add(sprites.ToArray());

                SetSpriteAtlasPlatformSetting(setting, packedAtlas);
                EditorUtility.SetDirty(packedAtlas);
                AssetDatabase.SaveAssets();

                return packedAtlas;
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", $"Pack failed.\n{atlasDirPath} is not valid!", "OK");
            }
            return null;
        }

        private static void SetTextureToSprite(string textAssetPath, AtlasPackerSetting setting)
        {
            TextureImporter texImp = AssetImporter.GetAtPath(textAssetPath) as TextureImporter;
            texImp.textureType = TextureImporterType.Sprite;
            texImp.spriteImportMode = SpriteImportMode.Single;
            texImp.spritePackingTag = "";
            texImp.spritePixelsPerUnit = setting.pixelsPerUnit;
            texImp.sRGBTexture = setting.isSRGB;
            texImp.alphaIsTransparency = true;
            texImp.alphaSource = TextureImporterAlphaSource.FromInput;
            texImp.isReadable = false;
            texImp.mipmapEnabled = false;
            texImp.SaveAndReimport();
        }

        private static void SetSpriteAtlasPlatformSetting(AtlasPackerSetting setting, SpriteAtlas packAtlas)
        {
            SpriteAtlasTextureSettings sats = packAtlas.GetTextureSettings();
            sats.readable = setting.isReadOrWrite;
            sats.sRGB = setting.isSRGB;
            sats.generateMipMaps = setting.isMipmap;
            sats.filterMode = setting.filterMode;
            packAtlas.SetTextureSettings(sats);

            SpriteAtlasPackingSettings saps = packAtlas.GetPackingSettings();
            saps.enableRotation = setting.isRotation;
            saps.padding = setting.padding;
            saps.enableTightPacking = setting.isTightPacking;
            packAtlas.SetPackingSettings(saps);

            TextureImporterPlatformSettings winTips = packAtlas.GetPlatformSettings("Standalone");
            winTips.overridden = true;
            winTips.maxTextureSize = setting.maxSize;
            winTips.format = (TextureImporterFormat)setting.winTextureFormat;
            packAtlas.SetPlatformSettings(winTips);

            TextureImporterPlatformSettings androidTips = packAtlas.GetPlatformSettings("Android");
            androidTips.maxTextureSize = setting.maxSize;
            androidTips.overridden = true;
            androidTips.format = (TextureImporterFormat)setting.androidTextureFormat;
            packAtlas.SetPlatformSettings(androidTips);

            TextureImporterPlatformSettings iOSTips = packAtlas.GetPlatformSettings("iPhone");
            iOSTips.maxTextureSize = setting.maxSize;
            iOSTips.overridden = true;
            iOSTips.format = (TextureImporterFormat)setting.iosTextureFormat;
            packAtlas.SetPlatformSettings(iOSTips);
        }
    }
}
