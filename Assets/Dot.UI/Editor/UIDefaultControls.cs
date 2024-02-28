using DotEngine.Core.Extensions;
using DotEngine.UI;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private const int kUIScreenWith = 1920;
        private const int kUIScreenHeight = 1080;

        public const float kWidth = 160f;
        public const float kThickHeight = 30f;
        public const float kThinHeight = 20f;
        public static Vector2 ThickElementSize = new Vector2(kWidth, kThickHeight);
        public static Vector2 ThinElementSize = new Vector2(kWidth, kThinHeight);
        public static Vector2 ImageElementSize = new Vector2(100f, 100f);
        public static Color DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        public static Color PanelColor = new Color(1f, 1f, 1f, 0.392f);
        public static Color TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        public static UIRoot CreateUIRoot(string name, int layer)
        {
            var root = UnityObject.FindObjectOfType<UIRoot>();
            if (root != null)
            {
                return root;
            }

            var rootGo = new GameObject(name);
            rootGo.layer = layer;
            root = rootGo.AddComponent<UIRoot>();
            Undo.RegisterCreatedObjectUndo(rootGo, "Create " + rootGo.name);

            var eventSystem = CreateEventSystem(rootGo);
            root.eventSystem = eventSystem;

            return root;
        }

        public static UIRoot CreateUIRoot()
        {
            return CreateUIRoot(kUIRootName, LayerMask.NameToLayer(kLayerName));
        }

        public static UIHierarchy CreateUIHierarchy(string name, bool isDefault = false)
        {
            var root = CreateUIRoot();

            var hierarchyGo = CreateUIObject(name, root.gameObject, typeof(UIHierarchy));
            var hierarchy = hierarchyGo.GetComponent<UIHierarchy>();

            var hierarchyCanvas = hierarchyGo.GetComponent<Canvas>();
            hierarchyCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            hierarchy.canvas = hierarchyCanvas;

            var canvasScaler = hierarchyGo.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referenceResolution = new Vector2(kUIScreenWith, kUIScreenHeight);

            var uiCamera = CreateUICamera(hierarchyGo);
            hierarchy.uiCamera = uiCamera;
            hierarchyCanvas.worldCamera = uiCamera;

            var data = new UIHierarchyData()
            {
                name = name,
                isDefault = isDefault,
                hierarchy = hierarchy,
            };

            root.hierarchies ??= new UIHierarchyData[0];
            ArrayUtility.Add(ref root.hierarchies, data);

            Undo.RegisterCreatedObjectUndo(hierarchyGo, "Create " + hierarchyGo.name);

            return hierarchy;
        }

        public static UIHierarchy CreateUIHierarchy(bool isDefault = false)
        {
            return CreateUIHierarchy(kUIHierarchyName, isDefault);
        }

        public static UIHierarchy FindUIHierarchy(bool createIfNot = true)
        {
            UIRoot root = CreateUIRoot();

            UIHierarchy hierarchy = root.transform.FindComponentInChild<UIHierarchy>(false) ?? CreateUIHierarchy();

            return hierarchy;
        }

        public static UILayer CreateUILayer(string name)
        {
            UIRoot root = CreateUIRoot();

            UIHierarchy hierarchy = root.transform.FindComponentInChild<UIHierarchy>(false) ?? CreateUIHierarchy();
            var hierarchyGo = hierarchy.gameObject;
            var layerGo = CreateUIObject(name, hierarchyGo, typeof(UILayer));
            var layer = layerGo.GetComponent<UILayer>();
            var layerTransform = layerGo.transform as RectTransform;
            layerTransform.SetStretchAnchorAll();
            layer.cachedGameObject = layerGo;
            layer.cachedTransform = layerTransform;

            var data = new UILayerData()
            {
                name = name,
                layer = layer,
            };

            hierarchy.layers ??= new UILayerData[0];
            ArrayUtility.Add(ref hierarchy.layers, data);

            Undo.RegisterCreatedObjectUndo(layerGo, "Create " + layerGo.name);

            return layer;
        }

        public static UILayer CreateUILayer()
        {
            return CreateUILayer(kUILayerName);
        }

        public static UILayer FindUILayer(bool createIfNot = true)
        {
            UIHierarchy hierarchy = FindUIHierarchy(true);
            UILayer layer = hierarchy.transform.FindComponentInChild<UILayer>(false) ?? CreateUILayer();
            return layer;
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

        private static EventSystem CreateEventSystem(GameObject parent = null)
        {
            var evtSystem = UnityObject.FindObjectOfType<EventSystem>();
            if (evtSystem == null)
            {
                var evtSystemGo = new GameObject(kEventSystemName);
                evtSystem = evtSystemGo.AddComponent<EventSystem>();
                evtSystemGo.AddComponent<StandaloneInputModule>();

                if (parent != null)
                {
                    evtSystemGo.transform.SetParent(parent.transform, false);
                    evtSystemGo.layer = parent.layer;
                }

                Undo.RegisterCreatedObjectUndo(evtSystemGo, "Create " + evtSystemGo.name);
            }

            return evtSystem;
        }

        private static Camera CreateUICamera(GameObject parent)
        {
            var uiCamera = parent.transform.FindComponentInChild<Camera>(false);
            if (uiCamera == null)
            {
                var uiCameraGO = new GameObject(kUICameraName);
                uiCameraGO.transform.SetParent(parent.transform, false);
                uiCameraGO.layer = parent.layer;

                uiCamera = uiCameraGO.AddComponent<Camera>();
                uiCamera.clearFlags = CameraClearFlags.Depth;
                uiCamera.cullingMask = 1 << parent.layer;
                uiCamera.orthographic = true;
                uiCamera.depth = 100;
                uiCamera.nearClipPlane = -100;
                uiCamera.farClipPlane = 100;
            }

            return uiCamera;
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

            UILayer layer = FindUILayer(true);
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
