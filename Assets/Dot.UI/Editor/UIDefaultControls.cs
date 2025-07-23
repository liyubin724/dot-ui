using DotEngine.Core.Extensions;
using DotEngine.Core.Utilities;
using DotEngine.UI;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ArrayUtility = DotEngine.Core.Utilities.ArrayUtility;
using UnityObject = UnityEngine.Object;

namespace DotEditor.UI
{
    public static class UIDefaultControls
    {
        public struct UIControlResources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";

        static private UIControlResources s_StandardResources;

        public static UIControlResources GetStandardResources()
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

        public const string kLayerName = "UI";
        public const string kUIRootName = "UI Root";
        public const string kEventSystemName = "EventSystem";
        public const string kUICameraName = "UI Camera";
        public const string kUIHierarchyName = "UI Hierarchy";
        public const string kUILayerName = "UI Layer";

        public const float kWidth = 160f;
        public const float kThickHeight = 30f;
        public const float kThinHeight = 20f;
        public static Vector2 ThickElementSize = new Vector2(kWidth, kThickHeight);
        public static Vector2 ThinElementSize = new Vector2(kWidth, kThinHeight);
        public static Vector2 ImageElementSize = new Vector2(100f, 100f);
        public static Color DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        public static Color PanelColor = new Color(1f, 1f, 1f, 0.392f);
        public static Color TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        public static UIRoot CreateDefault()
        {
            var root = CreateUIRoot();
            var hierarchy = CreateUIHierarchy(root, kUIHierarchyName);
            CreateUILevel(hierarchy, UIDefines.UI_LEVEL_BACKGROUND);
            CreateUILevel(hierarchy, UIDefines.UI_LEVEL_MAIN);
            CreateUILevel(hierarchy, UIDefines.UI_LEVEL_DEFAULT);
            CreateUILevel(hierarchy, UIDefines.UI_LEVEL_POPUP);
            CreateUILevel(hierarchy, UIDefines.UI_LEVEL_OVERLAY);

            return root;
        }

        public static UIRoot FindUIRoot(bool createIfNot = true)
        {
            var root = UnityObject.FindObjectOfType<UIRoot>();
            if (root == null && createIfNot)
            {
                root = CreateUIRoot();
            }
            return root;
        }

        public static UIRoot CreateUIRoot()
        {
            var rootGo = new GameObject(kUIRootName);
            rootGo.layer = LayerMask.NameToLayer(kLayerName);

            UIRoot root = rootGo.AddComponent<UIRoot>();

            var eventSystem = FindEventSystem() ?? CreateEventSystem(rootGo);
            ReflectionUtility.TrySetValue(root, "m_EventSystem", eventSystem);

            Undo.RegisterCreatedObjectUndo(rootGo, "Create " + rootGo.name);

            return root;
        }

        public static EventSystem FindEventSystem()
        {
            var evtSystem = UnityObject.FindObjectOfType<EventSystem>();
            if (evtSystem != null)
            {
                return evtSystem;
            }

            return null;
        }

        public static EventSystem CreateEventSystem(GameObject parent)
        {
            var evtSystem = UnityObject.FindObjectOfType<EventSystem>();
            if (evtSystem != null)
            {
                return evtSystem;
            }

            var evtSystemGo = new GameObject(kEventSystemName);
            evtSystem = evtSystemGo.AddComponent<EventSystem>();
            evtSystemGo.AddComponent<StandaloneInputModule>();

            if (parent != null)
            {
                evtSystemGo.transform.SetParent(parent.transform, false);
                evtSystemGo.layer = parent.layer;
            }

            Undo.RegisterCreatedObjectUndo(evtSystemGo, "Create " + evtSystemGo.name);

            return evtSystem;
        }

        public static UIHierarchy FindUIHierarchy(bool createIfNot = true)
        {
            var hierarchy = UnityObject.FindObjectOfType<UIHierarchy>();
            if (hierarchy == null && createIfNot)
            {
                var root = FindUIRoot();
                hierarchy = CreateUIHierarchy(root, kUIHierarchyName);
            }

            return hierarchy;
        }

        public static UIHierarchy CreateUIHierarchy(UIRoot root, string name)
        {
            var hierarchyGo = CreateUIObject(name, root.gameObject, typeof(UIHierarchy));

            var hierarchy = hierarchyGo.GetComponent<UIHierarchy>();
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_Identity", name);
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_Visible", true);
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedGameObject", hierarchyGo);
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedTransform", hierarchyGo.transform);
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedRectTransform", (RectTransform)hierarchyGo.transform);

            UICamera uiCamera = CreateUICamera(root.gameObject);
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedCamera", uiCamera);

            var hierarchyCanvas = hierarchyGo.GetComponent<Canvas>();
            hierarchyCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            hierarchyCanvas.worldCamera = uiCamera.GetComponent<Camera>();
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedCanvas", hierarchyCanvas);

            var canvasScaler = hierarchyGo.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referenceResolution = new Vector2(UIDefines.DESIGN_SCREEN_WIDTH, UIDefines.DESIGN_SCREEN_HEIGHT);
            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedScaler", canvasScaler);

            ReflectionUtility.TrySetFieldValue(hierarchy, "m_CachedRaycaster", hierarchyGo.GetComponent<GraphicRaycaster>());

            Undo.RegisterCreatedObjectUndo(hierarchyGo, "Create " + hierarchyGo.name);

            if (ReflectionUtility.TryGetFieldValue(root, "m_Hierarchies", out var hierarchies))
            {
                UIHierarchy[] tHierarchies = (UIHierarchy[])hierarchies;
                ArrayUtility.Add(ref tHierarchies, hierarchy);
                ReflectionUtility.TrySetFieldValue(root, "m_Hierarchies", tHierarchies);
            }
            else
            {
                ReflectionUtility.TrySetFieldValue(root, "m_Hierarchies", new UIHierarchy[1] { hierarchy });
            }

            Undo.RegisterCreatedObjectUndo(root, "Add " + hierarchyGo.name);

            return hierarchy;
        }

        public static UICamera CreateUICamera(GameObject parent)
        {
            var uiCameraGO = new GameObject(kUICameraName);
            uiCameraGO.transform.SetParent(parent.transform, false);
            uiCameraGO.layer = parent.layer;

            var uiCamera = uiCameraGO.AddComponent<UICamera>();
            var camera = uiCameraGO.GetComponent<Camera>();
            ReflectionUtility.TrySetFieldValue(uiCamera, "m_CachedCamera", camera);

            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask = 1 << parent.layer;
            camera.orthographic = true;
            camera.depth = 1;
            camera.nearClipPlane = -1;
            camera.farClipPlane = 1000;


            Undo.RegisterCreatedObjectUndo(uiCameraGO, "Create " + uiCameraGO.name);

            return uiCamera;
        }

        public static UILevel FindUILevel(bool createIfNot = true)
        {
            var uiLevel = UnityObject.FindObjectOfType<UILevel>();
            if (uiLevel == null && createIfNot)
            {
                var hierarchy = FindUIHierarchy();

                CreateUILevel(hierarchy, UIDefines.UI_LEVEL_BACKGROUND);
                CreateUILevel(hierarchy, UIDefines.UI_LEVEL_MAIN);
                uiLevel = CreateUILevel(hierarchy, UIDefines.UI_LEVEL_DEFAULT);
                CreateUILevel(hierarchy, UIDefines.UI_LEVEL_POPUP);
                CreateUILevel(hierarchy, UIDefines.UI_LEVEL_OVERLAY);
            }

            return uiLevel;
        }

        public static UILevel CreateUILevel(UIHierarchy hierarchy, string identity)
        {
            var uiLevel = hierarchy.GetLevel(identity);
            if (uiLevel != null)
            {
                return uiLevel;
            }

            var name = string.Format(UIDefines.UI_LEVEL_NAME_FORMAT, identity);
            var levelGo = CreateUIObject(name, hierarchy.gameObject, typeof(UILevel));

            uiLevel = levelGo.GetComponent<UILevel>();
            ReflectionUtility.TrySetFieldValue(uiLevel, "m_Identity", identity);

            ReflectionUtility.TrySetFieldValue(uiLevel, "m_CachedGameObject", levelGo);
            ReflectionUtility.TrySetFieldValue(uiLevel, "m_CachedTransform", levelGo.transform);
            ReflectionUtility.TrySetFieldValue(uiLevel, "m_CachedRectTransform", (RectTransform)levelGo.transform);

            var canvas = levelGo.GetComponent<Canvas>();
            var canvasTransform = (RectTransform)canvas.transform;
            canvasTransform.anchorMin = Vector2.zero;
            canvasTransform.anchorMax = Vector2.one;
            canvasTransform.offsetMin = Vector2.zero;
            canvasTransform.offsetMax = Vector2.zero;
            ReflectionUtility.TrySetFieldValue(uiLevel, "m_CachedCanvas", canvas);

            Undo.RegisterCreatedObjectUndo(levelGo, "Create " + levelGo.name);

            if (ReflectionUtility.TryGetFieldValue(hierarchy, "m_Levels", out var layers))
            {
                UILevel[] tLayers = (UILevel[])layers;
                ArrayUtility.Add(ref tLayers, uiLevel);
                ReflectionUtility.TrySetFieldValue(hierarchy, "m_Levels", tLayers);
            }
            else
            {
                ReflectionUtility.TrySetFieldValue(hierarchy, "m_Levels", new UILevel[1] { uiLevel });
            }

            Undo.RegisterCreatedObjectUndo(hierarchy, "Add " + levelGo.name);

            return uiLevel;
        }

        public static GameObject CreateUIEmptyImage()
        {
            GameObject go = CreateUIElement("Empty Image", ImageElementSize);
            go.AddComponent<UIEmptyImage>();
            return go;
        }

        public static GameObject CreateUIAtlasImage()
        {
            GameObject go = CreateUIElement("Atlas Image", ImageElementSize);
            go.AddComponent<UIAtlasImage>();
            return go;
        }

        public static GameObject CreateUIWebImage()
        {
            GameObject go = CreateUIElement("Web Image", ImageElementSize);
            go.AddComponent<UIWebImage>();
            return go;
        }

        public static GameObject CreateUIDynamicAtlasImage()
        {
            GameObject go = CreateUIElement("Dynamic Atlas Image", ImageElementSize);
            go.AddComponent<UIDynamicAtlasImage>();
            return go;
        }

        public static GameObject CreateUIAtlasImageAnimation()
        {
            GameObject go = CreateUIElement("Atlas Image Animation", ImageElementSize);
            go.AddComponent<UIAtlasImageAnimation>();
            return go;
        }

        public static GameObject CreateUITransparentButton()
        {
            GameObject go = CreateUIElement("Transparent Button", ImageElementSize);

            var graphic = go.AddComponent<UIEmptyGraphic>();
            var button = go.AddComponent<Button>();

            button.targetGraphic = graphic;
            button.transition = Selectable.Transition.None;

            return go;
        }

        public static GameObject CreateUIObject(string name, GameObject parent, params Type[] componentTypes)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go, parent);

            if (componentTypes != null && componentTypes.Length > 0)
            {
                foreach (Type componentType in componentTypes)
                {
                    go.AddComponent(componentType);
                }
            }

            return go;
        }

        public static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            child.SetLayer(parent.layer, true);
        }

        public static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            if (parent == null || parent.GetComponentInParent<Canvas>() == null)
            {
                parent = GetOrCreateCanvasGameObject();
            }

            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
            Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element, parent);
            if (parent != menuCommand.context) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            Selection.activeGameObject = element;
        }

        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        private static GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            UILevel layer = FindUILevel(true);
            canvas = layer.GetComponent<Canvas>();

            return canvas.gameObject;
        }

        public static GameObject CreateUIElement(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        public static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = TextColor;

            System.Type textType = lbl.GetType();
            MethodInfo mi = textType.GetMethod("AssignDefaultFont", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(lbl, new System.Object[] { });
        }

        public static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }
    }
}
