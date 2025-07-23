using DotEngine.UI;
using UnityEditor;
using UnityEngine;

using static DotEditor.UI.UIDefaultControls;

namespace DotEditor.UI
{
    static internal class UIMenuOptions
    {
        [MenuItem("GameObject/UI/UI Root", false, 900)]
        static public void AddUIRoot(MenuCommand menuCommand)
        {
            UIRoot root = CreateDefault();
            Selection.activeObject = root.gameObject;
        }

        [MenuItem("GameObject/UI/UI Hierarchy", false, 901)]
        static public void AddUIHierarchy()
        {
            UIRoot root = null;
            var activeObject = Selection.activeGameObject;
            if (activeObject is GameObject go)
            {
                root = go.GetComponent<UIRoot>();
            }
            if (root == null)
            {
                root = FindUIRoot();
            }
            var hierarchy = CreateUIHierarchy(root, kUIHierarchyName);

            Selection.activeObject = hierarchy.gameObject;
        }

        [MenuItem("GameObject/UI/UI Stage", false, 902)]
        static public void AddUIStage()
        {
            UIHierarchy hierarchy = null;

            var activeObject = Selection.activeGameObject;
            if (activeObject is GameObject go)
            {
                hierarchy = go.GetComponentInChildren<UIHierarchy>();
            }
            if (hierarchy == null)
            {
                hierarchy = FindUIHierarchy();
            }

            UILevel layer = CreateUILevel(hierarchy, UIDefines.UI_LEVEL_DEFAULT);
            Selection.activeObject = layer.gameObject;
        }

        [MenuItem("GameObject/UI/UI Empty Image", false, 1000)]
        static public void AddEmptyImage(MenuCommand menuCommand)
        {
            GameObject go = CreateUIEmptyImage();
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/UI Atlas Image", false, 1010)]
        static public void AddAtlasImage(MenuCommand menuCommand)
        {
            GameObject go = CreateUIAtlasImage();
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/UI Web Image", false, 1020)]
        static public void AddWebImage(MenuCommand menuCommand)
        {
            GameObject go = CreateUIWebImage();
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/UI Dynamic Atlas Image", false, 1030)]
        static public void AddDynamicAtlasImage(MenuCommand menuCommand)
        {
            GameObject go = CreateUIDynamicAtlasImage();
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/UI Atlas Image Animation", false, 1040)]
        static public void AddAtlasImageAnimation(MenuCommand menuCommand)
        {
            GameObject go = CreateUIAtlasImageAnimation();
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/UI Transparent Button", false, 1050)]
        static public void AddTransparentButton(MenuCommand menuCommand)
        {
            GameObject go = CreateUITransparentButton();
            PlaceUIElementRoot(go, menuCommand);
        }
    }
}
