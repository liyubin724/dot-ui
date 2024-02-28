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
        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        public const string kLayerName = "UI";
        public const string kUIRootName = "UI Root";
        public const string kEventSystemName = "EventSystem";
        public const string kUICameraName = "UI Camera";
        public const string kUIHierarchyName = "UI Hierarchy";
        public const string kUILayerName = "UI Layer";

        private const int kUIScreenWith = 1920;
        private const int kUIScreenHeight = 1080;

        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        private static Vector2 s_ThinElementSize = new Vector2(kWidth, kThinHeight);
        private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
        private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

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

        public static GameObject CreateUIAtlasImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("Atlas Image", s_ImageElementSize);
            go.AddComponent<UIAtlasImage>();
            return go;
        }

        public static GameObject CreateClearImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("Empty Image", s_ImageElementSize);
            go.AddComponent<UIEmptyImage>();
            return go;
        }

        public static GameObject CreateWebImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("Web Image", s_ImageElementSize);
            go.AddComponent<UIWebImage>();
            return go;
        }

        public static GameObject CreateDynamicAtlasImage(Resources resources)
        {
            GameObject go = CreateUIElementRoot("Dynamic Atlas Image", s_ImageElementSize);
            go.AddComponent<UIDynamicAtlasImage>();
            return go;
        }

        public static GameObject CreateAtlasImageAnimation(Resources resources)
        {
            GameObject go = CreateUIElementRoot("Atlas Image Animation", s_ImageElementSize);
            go.AddComponent<UIAtlasImageAnimation>();
            return go;
        }

        public static GameObject CreateTransparentButton(Resources resources)
        {
            GameObject go = CreateUIElementRoot("Transparent Button", s_ImageElementSize);

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

        private static void SetParentAndAlign(GameObject child, GameObject parent)
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

        // Helper methods at top

        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = s_TextColor;

            System.Type textType = lbl.GetType();
            MethodInfo mi = textType.GetMethod("AssignDefaultFont", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(lbl, new System.Object[] { });
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }


#if ENABLE_LUA
        public static GameObject CreateLuaButton(Resources resources)
        {
            GameObject btnGO = CreateUIElementRoot("LuaButton", s_ThickElementSize);

            GameObject childText = new GameObject("Text");
            childText.AddComponent<RectTransform>();
            SetParentAndAlign(childText, btnGO);

            Image image = btnGO.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            UILuaButton bt = btnGO.AddComponent<UILuaButton>();
            SetDefaultColorTransitionValues(bt);

            Text text = childText.AddComponent<Text>();
            text.text = "Lua Button";
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            return btnGO;
        }

        public static GameObject CreateLuaInputField(Resources resources)
        {
            GameObject root = CreateUIElementRoot("LuaInputField", s_ThickElementSize);

            GameObject childPlaceholder = CreateUIObject("Placeholder", root);
            GameObject childText = CreateUIObject("Text", root);

            Image image = root.AddComponent<Image>();
            image.sprite = resources.inputField;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            UILuaInputField inputField = root.AddComponent<UILuaInputField>();
            SetDefaultColorTransitionValues(inputField);

            Text text = childText.AddComponent<Text>();
            text.text = "";
            text.supportRichText = false;
            SetDefaultTextValues(text);

            Text placeholder = childPlaceholder.AddComponent<Text>();
            placeholder.text = "Enter text...";
            placeholder.fontStyle = FontStyle.Italic;
            // Make placeholder color half as opaque as normal text color.
            Color placeholderColor = text.color;
            placeholderColor.a *= 0.5f;
            placeholder.color = placeholderColor;

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;
            textRectTransform.offsetMin = new Vector2(10, 6);
            textRectTransform.offsetMax = new Vector2(-10, -7);

            RectTransform placeholderRectTransform = childPlaceholder.GetComponent<RectTransform>();
            placeholderRectTransform.anchorMin = Vector2.zero;
            placeholderRectTransform.anchorMax = Vector2.one;
            placeholderRectTransform.sizeDelta = Vector2.zero;
            placeholderRectTransform.offsetMin = new Vector2(10, 6);
            placeholderRectTransform.offsetMax = new Vector2(-10, -7);

            inputField.textComponent = text;
            inputField.placeholder = placeholder;

            return root;
        }

        public static GameObject CreateLuaToggle(Resources resources)
        {
            // Set up hierarchy
            GameObject toggleRoot = CreateUIElementRoot("LuaToggle", s_ThinElementSize);

            GameObject background = CreateUIObject("Background", toggleRoot);
            GameObject checkmark = CreateUIObject("Checkmark", background);
            GameObject childLabel = CreateUIObject("Label", toggleRoot);

            // Set up components
            UILuaToggle toggle = toggleRoot.AddComponent<UILuaToggle>();
            toggle.isOn = true;

            Image bgImage = background.AddComponent<Image>();
            bgImage.sprite = resources.standard;
            bgImage.type = Image.Type.Sliced;
            bgImage.color = s_DefaultSelectableColor;

            Image checkmarkImage = checkmark.AddComponent<Image>();
            checkmarkImage.sprite = resources.checkmark;

            Text label = childLabel.AddComponent<Text>();
            label.text = "Lua Toggle";
            SetDefaultTextValues(label);

            toggle.graphic = checkmarkImage;
            toggle.targetGraphic = bgImage;
            SetDefaultColorTransitionValues(toggle);

            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0f, 1f);
            bgRect.anchorMax = new Vector2(0f, 1f);
            bgRect.anchoredPosition = new Vector2(10f, -10f);
            bgRect.sizeDelta = new Vector2(kThinHeight, kThinHeight);

            RectTransform checkmarkRect = checkmark.GetComponent<RectTransform>();
            checkmarkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchoredPosition = Vector2.zero;
            checkmarkRect.sizeDelta = new Vector2(20f, 20f);

            RectTransform labelRect = childLabel.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.offsetMin = new Vector2(23f, 1f);
            labelRect.offsetMax = new Vector2(-5f, -2f);

            return toggleRoot;
        }

        public static GameObject CreateLuaSlider(Resources resources)
        {
            GameObject root = CreateUIElementRoot("LuaSlider", s_ThinElementSize);

            GameObject background = CreateUIObject("Background", root);
            GameObject fillArea = CreateUIObject("Fill Area", root);
            GameObject fill = CreateUIObject("Fill", fillArea);
            GameObject handleArea = CreateUIObject("Handle Slide Area", root);
            GameObject handle = CreateUIObject("Handle", handleArea);

            // Background
            Image backgroundImage = background.AddComponent<Image>();
            backgroundImage.sprite = resources.background;
            backgroundImage.type = Image.Type.Sliced;
            backgroundImage.color = s_DefaultSelectableColor;
            RectTransform backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.25f);
            backgroundRect.anchorMax = new Vector2(1, 0.75f);
            backgroundRect.sizeDelta = new Vector2(0, 0);

            // Fill Area
            RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1, 0.75f);
            fillAreaRect.anchoredPosition = new Vector2(-5, 0);
            fillAreaRect.sizeDelta = new Vector2(-20, 0);

            // Fill
            Image fillImage = fill.AddComponent<Image>();
            fillImage.sprite = resources.standard;
            fillImage.type = Image.Type.Sliced;
            fillImage.color = s_DefaultSelectableColor;

            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.sizeDelta = new Vector2(10, 0);

            // Handle Area
            RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.sizeDelta = new Vector2(-20, 0);
            handleAreaRect.anchorMin = new Vector2(0, 0);
            handleAreaRect.anchorMax = new Vector2(1, 1);

            // Handle
            Image handleImage = handle.AddComponent<Image>();
            handleImage.sprite = resources.knob;
            handleImage.color = s_DefaultSelectableColor;

            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 0);

            // Setup slider component
            UILuaSlider slider = root.AddComponent<UILuaSlider>();
            slider.fillRect = fill.GetComponent<RectTransform>();
            slider.handleRect = handle.GetComponent<RectTransform>();
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;
            SetDefaultColorTransitionValues(slider);

            return root;
        }
#endif
    }
}
