using System.Collections.Generic;
using UnityEngine;
using static DotEngine.UI.DynamicAtlas;
using HeuristicMethod = DotEngine.UI.MaxRectsBinPack.FreeRectChoiceHeuristic;

namespace DotEngine.UI
{
    internal class RawImageAtlasData
    {
        public string ImagePath { get; set; } = "";
        public DynamicAtlas Atlas { get; set; } = null;

        private int retainCount = 0;

        public void Retain() => ++retainCount;
        public void Release() => --retainCount;
        public bool IsInUsing() => retainCount > 0;
    }

    public class DynamicAtlasAssembly
    {
        private string name;
        private int width = 0;
        private int height = 0;
        private HeuristicMethod method;
        private TextureFormat format;

        private List<DynamicAtlas> atlasList = new List<DynamicAtlas>();
        private Dictionary<string, RawImageAtlasData> rawImageDic = new Dictionary<string, RawImageAtlasData>();

        public DynamicAtlasAssembly(string name, int width, int height, HeuristicMethod method, TextureFormat format)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.method = method;
            this.format = format;
        }

        public bool Contains(string rawImagePath)
        {
            return rawImageDic.ContainsKey(rawImagePath);
        }

        public void AddRawImage(string rawImagePath, Texture2D texture)
        {
            if (texture == null) return;
            if (!texture.isReadable)
            {
                Debug.LogError("DynamicAtlasAssembly::AddRawImageSprite->texture is not readable.path =" + rawImagePath);
                return;
            }

            if (rawImageDic.ContainsKey(rawImagePath)) return;

            if (texture.width > width || texture.height > height)
            {
                Debug.LogError("DynamicAtlasAssembly::AddRawImageSprite->texture is too large,path = " + rawImagePath);
                return;
            }

            RawImageAtlasData imageAtlasData = new()
            {
                ImagePath = rawImagePath
            };

            for (int i = atlasList.Count - 1; i >= 0; --i)
            {
                DynamicAtlas atlas = atlasList[i];
                if (atlas.Insert(texture, rawImagePath))
                {
                    imageAtlasData.Atlas = atlas;
                    break;
                }
            }

            if (imageAtlasData.Atlas == null)
            {
                DynamicAtlas atlas = new DynamicAtlas(width, height, name, method, format);
                atlasList.Add(atlas);

                if (atlas.Insert(texture, rawImagePath))
                {
                    imageAtlasData.Atlas = atlas;
                }
            }
            if (imageAtlasData.Atlas == null)
            {
                Debug.LogError("DynamicAtlasAssembly::AddRawImageSprite->texture add failed,path = " + rawImagePath);
                return;
            }

            rawImageDic.Add(rawImagePath, imageAtlasData);
        }

        public Sprite GetRawImageAsSprite(string rawImagePath)
        {
            if (rawImageDic.TryGetValue(rawImagePath, out RawImageAtlasData data))
            {
                SourceInfo sInfo = data.Atlas.Get(rawImagePath);
                if (sInfo != null)
                {
                    data.Retain();
                    return sInfo.GetSprite();
                }
            }
            return null;
        }

        public void RemoveRawImageSprite(string rawImagePath)
        {
            if (rawImageDic.TryGetValue(rawImagePath, out RawImageAtlasData data))
            {
                data.Release();
                if (!data.IsInUsing())
                {
                    DynamicAtlas atlas = data.Atlas;
                    atlas.Remove(rawImagePath);
                    rawImageDic.Remove(rawImagePath);

                    if (atlas.Lenght == 0)
                    {
                        atlasList.Remove(atlas);
                    }
                }
            }
        }
    }
}
