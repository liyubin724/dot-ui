using UnityEngine;

namespace DotEditor.UI
{
    public class AtlasPackerSetting : ScriptableObject
    {
        public string atlasDirPath = null;//"Assets/ArtRes/UI/Atlas";

        public int pixelsPerUnit = 100;

        public bool isRotation = false;
        public bool isTightPacking = false;
        public int padding = 4;

        public bool isReadOrWrite = false;
        public bool isMipmap = false;
        public bool isSRGB = true;
        public FilterMode filterMode = FilterMode.Bilinear;

        public int maxSize = 2048;
        public int winTextureFormat = 4;
        public int androidTextureFormat = 4;
        public int iosTextureFormat = 4;

        public void CopyFrom(AtlasPackerSetting setting)
        {
            atlasDirPath = setting.atlasDirPath;
            pixelsPerUnit = setting.pixelsPerUnit;
            isRotation = setting.isRotation;
            isTightPacking = setting.isTightPacking;
            padding = setting.padding;
            isReadOrWrite = setting.isReadOrWrite;
            isMipmap = setting.isMipmap;
            isSRGB = setting.isSRGB;
            filterMode = setting.filterMode;
            maxSize = setting.maxSize;
            winTextureFormat = setting.winTextureFormat;
            androidTextureFormat = setting.androidTextureFormat;
            iosTextureFormat = setting.iosTextureFormat;
        }
    }
}
