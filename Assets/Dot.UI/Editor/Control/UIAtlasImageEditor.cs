﻿using DotEditor.Core.IMGUI;
using DotEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;
using UIIMageType = UnityEngine.UI.Image.Type;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIAtlasImage), true)]
    public class UIAtlasImageEditor : ImageEditor
    {
        private SerializedProperty m_SpriteAtlas;
        private SerializedProperty m_SpriteName;

        private AnimBool animShowType;
        private SerializedProperty m_PreserveAspect;
        private SerializedProperty m_Type;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SpriteAtlas = serializedObject.FindProperty("m_SpriteAtlas");
            m_SpriteName = serializedObject.FindProperty("m_SpriteName");
            m_Type = serializedObject.FindProperty("m_Type");
            m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");

            animShowType = new AnimBool(m_SpriteAtlas.objectReferenceValue && !string.IsNullOrEmpty(m_SpriteName.stringValue));
            animShowType.valueChanged.AddListener(new UnityAction(base.Repaint));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSpriteAtlas();
            AppearanceControlsGUI();
            RaycastControlsGUI();
            DrawTypeGUI();
            DrawImageType();
            DrawNativeSize();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawSpriteAtlas()
        {
            DrawAtlasPopupLayout(new GUIContent("Sprite Atlas"), new GUIContent("----"), m_SpriteAtlas);
            EGUI.BeginIndent();
            {
                DrawSpritePopup(m_SpriteAtlas.objectReferenceValue as SpriteAtlas, m_SpriteName);
            }
            EGUI.EndIndent();
        }

        protected void DrawTypeGUI()
        {
            animShowType.target = m_SpriteAtlas.objectReferenceValue && !string.IsNullOrEmpty(m_SpriteName.stringValue);
            if (EditorGUILayout.BeginFadeGroup(animShowType.faded))
                this.TypeGUI();
            EditorGUILayout.EndFadeGroup();
        }

        protected void DrawImageType()
        {
            UIIMageType imageType = (UIIMageType)m_Type.intValue;
            base.SetShowNativeSize(imageType == UIIMageType.Simple || imageType == UIIMageType.Filled, false);
        }

        protected void DrawNativeSize()
        {
            if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_PreserveAspect);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            base.NativeSizeButtonGUI();
        }

        private void DrawAtlasPopupLayout(GUIContent label, GUIContent nullLabel, SerializedProperty atlas, UnityAction<SpriteAtlas> onChange = null, params GUILayoutOption[] option)
        {
            DrawAtlasPopup(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.popup, option), label, nullLabel, atlas, onChange);
        }

        private void DrawAtlasPopup(Rect rect, GUIContent label, GUIContent nullLabel, SerializedProperty atlas, UnityAction<SpriteAtlas> onSelect = null)
        {
            DrawAtlasPopup(rect, label, nullLabel, atlas.objectReferenceValue as SpriteAtlas, obj =>
            {
                atlas.objectReferenceValue = obj;
                onSelect?.Invoke(obj as SpriteAtlas);
                atlas.serializedObject.ApplyModifiedProperties();
            });
        }

        private void DrawAtlasPopup(Rect rect, GUIContent label, GUIContent nullLabel, SpriteAtlas atlas, UnityAction<SpriteAtlas> onSelect = null)
        {
            rect = EditorGUI.PrefixLabel(rect, label);

            if (GUI.Button(rect, atlas ? new GUIContent(atlas.name) : nullLabel, EditorStyles.popup))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(nullLabel, !atlas, () => onSelect(null));

                foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
                {
                    string displayName = Path.GetFileNameWithoutExtension(path);
                    gm.AddItem(
                        new GUIContent(displayName),
                        atlas && (atlas.name == displayName),
                        x => onSelect(x == null ? null : AssetDatabase.LoadAssetAtPath((string)x, typeof(SpriteAtlas)) as SpriteAtlas),
                        path
                    );
                }

                gm.DropDown(rect);
            }
        }

        private void DrawSpritePopup(SpriteAtlas atlas, SerializedProperty spriteProperty)
        {
            GUIContent label = new GUIContent(spriteProperty.displayName, spriteProperty.tooltip);
            string spriteName = string.IsNullOrEmpty(spriteProperty.stringValue) ? "----" : spriteProperty.stringValue;

            using (new EditorGUI.DisabledGroupScope(!atlas))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.PrefixLabel(label);
                    if (GUILayout.Button(string.IsNullOrEmpty(spriteName) ? "-" : spriteName, "minipopup") && atlas)
                    {
                        AtlasSpriteSelector.Show(atlas, spriteName, (selectedSpriteName) =>
                        {
                            OnSpriteSelectedCallback(spriteProperty, selectedSpriteName);
                        });
                    }
                }
            }
        }

        protected virtual void OnSpriteSelectedCallback(SerializedProperty spriteProperty, string spriteName)
        {
            if (string.IsNullOrEmpty(spriteName))
                return;

            spriteProperty.serializedObject.Update();
            spriteProperty.stringValue = spriteName;
            spriteProperty.serializedObject.ApplyModifiedProperties();
        }

        public class PropertyObj
        {
            public string propertyName;
            public System.Object sysObject;
        }

        [MenuItem("Dot/UI/Control/Image To AtlasImage", false, 1000)]
        static public void UseAtlasImageReplaceImage()
        {
            GameObject[] selectedGOs = Selection.gameObjects;
            if (selectedGOs != null && selectedGOs.Length > 0)
            {
                List<SpriteAtlas> atlases = new List<SpriteAtlas>();
                foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
                {
                    SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                    if (atlas != null)
                    {
                        atlases.Add(atlas);
                    }
                }

                foreach (var go in selectedGOs)
                {
                    Image image = go.GetComponent<Image>();
                    if (image != null && image.sprite != null)
                    {
                        string spriteName = image.sprite.name;
                        foreach (var atals in atlases)
                        {
                            if (atals.GetSprite(spriteName) != null)
                            {
                                List<PropertyObj> objs = new List<PropertyObj>();
                                PropertyInfo[] pInfos = typeof(Image).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
                                foreach (var pInfo in pInfos)
                                {
                                    if (pInfo.GetGetMethod() == null || pInfo.GetSetMethod() == null)
                                    {
                                        continue;
                                    }

                                    if (pInfo.Name == "sprite" || pInfo.Name == "overrideSprite")
                                    {
                                        continue;
                                    }

                                    objs.Add(new PropertyObj()
                                    {
                                        propertyName = pInfo.Name,
                                        sysObject = pInfo.GetValue(image),
                                    });
                                }

                                Object.DestroyImmediate(image);
                                UIAtlasImage atlasImage = go.AddComponent<UIAtlasImage>();

                                foreach (var pObj in objs)
                                {
                                    PropertyInfo pInfo = typeof(UIAtlasImage).GetProperty(pObj.propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
                                    if (pInfo != null)
                                    {
                                        pInfo.SetValue(atlasImage, pObj.sysObject);
                                    }
                                }

                                atlasImage.Atlas = atals;
                                atlasImage.SpriteName = spriteName;

                                EditorUtility.SetDirty(go);
                                break;
                            }
                        }
                    }
                }
            }

        }
    }
}
