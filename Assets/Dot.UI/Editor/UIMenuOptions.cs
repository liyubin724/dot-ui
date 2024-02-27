using DotEngine.UI;
using UnityEditor;
using UnityEngine;

namespace DotEditor.UI
{
    static internal class UIMenuOptions
    {
        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";

        static private UIDefaultControls.Resources s_StandardResources;

        static private UIDefaultControls.Resources GetStandardResources()
        {
            if (s_StandardResources.standard == null)
            {
                s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
                s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
                s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
                s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
                s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
                s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
                s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            }
            return s_StandardResources;
        }

        [MenuItem("GameObject/UI/UI Root", false, 900)]
        static public void AddUIRoot(MenuCommand menuCommand)
        {
            UIRoot root = UIDefaultControls.CreateUIRoot();

            Selection.activeObject = root.gameObject;
        }

        [MenuItem("GameObject/UI/UI Hierarchy", false, 901)]
        static public void AddUIHierarchy()
        {
            UIHierarchy hierarchy = UIDefaultControls.CreateUIHierarchy();

            Selection.activeObject = hierarchy.gameObject;
        }

        [MenuItem("GameObject/UI/UI Layer", false, 902)]
        static public void AddUILayer()
        {
            UILayer layer = UIDefaultControls.CreateUILayer();

            Selection.activeObject = layer.gameObject;
        }

        [MenuItem("GameObject/UI/Clear Image", false, 1000)]
        static public void AddClearImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateClearImage(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Atlas Image", false, 1010)]
        static public void AddAtlasImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateUIAtlasImage(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Web Image", false, 1020)]
        static public void AddWebImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateWebImage(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Dynamic Atlas Image", false, 1030)]
        static public void AddDynamicAtlasImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateDynamicAtlasImage(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Atlas Image Animation", false, 1040)]
        static public void AddAtlasImageAnimation(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateAtlasImageAnimation(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Transparent Button", false, 1050)]
        static public void AddTransparentButton(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateTransparentButton(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

#if ENABLE_LUA
        [MenuItem("GameObject/Lua UI/Lua Button", false, 1102)]
        static public void AddLuaButton(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaButton(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/Lua UI/Lua Input Field", false, 1103)]
        static public void AddLuaInputField(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaInputField(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/Lua UI/Lua Toggle", false, 1104)]
        static public void AddLuaToggle(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaToggle(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/Lua UI/Lua Slider", false, 1104)]
        static public void AddLuaSlider(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaSlider(GetStandardResources());
            UIDefaultControls.PlaceUIElementRoot(go, menuCommand);
        }
#endif
    }
}
