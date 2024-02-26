using DotEngine.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DotEditor.UI
{
    static internal class UIMenuOptions
    {
        private const string kUILayerName = "UI";
        private const int kUIScreenWith = 1920;
        private const int kUIScreenHeight = 1080;
        private const int kUILayerCount = 5;

        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";

        private static readonly string[] kLayerNames = new string[]
        {
            "HUD Layer",
            "Main Layer",
            "Default Layer",
            "Popup Layer",
            "Toppest Layer"
        };
        private static readonly string kDefalutLayerName = "Default Layer";

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

        [MenuItem("GameObject/UI/Root", false, 999)]
        static public void AddUIRoot(MenuCommand menuCommand)
        {
            CreateNewUI();
        }

        [MenuItem("GameObject/UI/UI Hierarchy", false, 998)]
        static public void AddUIHierarchy()
        {
            CreateNewHierarchy("UI Hierarchy", false);
        }

        [MenuItem("GameObject/UI/UI Layer", false, 997)]
        static public void AddUILayer()
        {
            CreateNewLayer("UI Layer");
        }

        [MenuItem("GameObject/UI/Clear Image", false, 1000)]
        static public void AddClearImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateClearImage(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Atlas Image", false, 1000)]
        static public void AddAtlasImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateAtlasImage(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Web Image", false, 1000)]
        static public void AddWebImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateWebImage(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Dynamic Atlas Image", false, 1001)]
        static public void AddDynamicAtlasImage(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateDynamicAtlasImage(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Atlas Image Animation", false, 1002)]
        static public void AddAtlasImageAnimation(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateAtlasImageAnimation(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Transparent Button", false, 1003)]
        static public void AddTransparentButton(MenuCommand menuCommand)
        {
            GameObject go = UIDefaultControls.CreateTransparentButton(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }
#if ENABLE_LUA
        [MenuItem("GameObject/Lua UI/Lua Button", false, 1102)]
        static public void AddLuaButton(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaButton(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/Lua UI/Lua Input Field", false, 1103)]
        static public void AddLuaInputField(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaInputField(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/Lua UI/Lua Toggle", false, 1104)]
        static public void AddLuaToggle(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaToggle(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/Lua UI/Lua Slider", false, 1104)]
        static public void AddLuaSlider(MenuCommand menuCommand)
        {
            GameObject go = UGUIExtensionDefaultControls.CreateLuaSlider(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }
#endif

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

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
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

        static public GameObject CreateNewUI()
        {
            GameObject uiLayerGO = null;
            for (int i = 0; i < kLayerNames.Length; i++)
            {
                var layerGO = CreateNewLayer(kLayerNames[i]);
                if (kLayerNames[i] == kDefalutLayerName)
                {
                    uiLayerGO = layerGO;
                }
            }

            return uiLayerGO;
        }

        static public GameObject CreateNewRoot()
        {
            var uiRoot = Object.FindObjectOfType<UIRoot>();
            if (uiRoot == null)
            {
                var uiRootGO = new GameObject("UI Root");
                uiRootGO.layer = LayerMask.NameToLayer(kUILayerName);
                uiRootGO.transform.position = new Vector3(0, 0, 0);
                uiRoot = uiRootGO.AddComponent<UIRoot>();

                var eventSystem = CreateEventSystem(false, uiRootGO);
                uiRoot.eventSystem = eventSystem.GetComponent<EventSystem>();

                Undo.RegisterCreatedObjectUndo(uiRootGO, "Create " + uiRootGO.name);
            }

            return uiRoot.gameObject;
        }

        static public GameObject CreateNewHierarchy(string name, bool isDefault)
        {
            var uiRootGO = CreateNewRoot();
            var uiRoot = uiRootGO.GetComponent<UIRoot>();

            var uiHierarchyGO = new GameObject(name);
            uiHierarchyGO.layer = LayerMask.NameToLayer(kUILayerName);
            uiHierarchyGO.transform.SetParent(uiRootGO.transform, false);
            var uiHierarchy = uiHierarchyGO.AddComponent<UIHierarchy>();

            Canvas uiCanvas = uiHierarchyGO.GetComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            uiHierarchy.canvas = uiCanvas;

            var uiCanvasScaler = uiHierarchyGO.GetComponent<CanvasScaler>();
            uiCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            uiCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            uiCanvasScaler.referenceResolution = new Vector2(kUIScreenWith, kUIScreenHeight);

            var uiCameraGO = new GameObject("UI Camera");
            uiCameraGO.transform.SetParent(uiRootGO.transform, false);
            var uiCamera = uiCameraGO.AddComponent<Camera>();
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = 1 << LayerMask.NameToLayer(kUILayerName);
            uiCamera.orthographic = true;
            uiCamera.depth = 100;
            uiHierarchy.uiCamera = uiCamera;

            uiCanvas.worldCamera = uiCamera;

            var hierarchyData = new UIHierarchyData()
            {
                name = name,
                isDefault = isDefault,
                hierarchy = uiHierarchy,
            };

            if (uiRoot.hierarchies == null)
            {
                uiRoot.hierarchies = new UIHierarchyData[0];
            }
            ArrayUtility.Add(ref uiRoot.hierarchies, hierarchyData);

            Undo.RegisterCreatedObjectUndo(uiHierarchyGO, "Create " + uiHierarchyGO.name);

            return uiHierarchyGO;
        }

        static public GameObject CreateNewLayer(string layerName)
        {
            var uiHierarchy = Object.FindObjectOfType<UIHierarchy>();
            if (uiHierarchy == null)
            {
                uiHierarchy = CreateNewHierarchy("UI Hierarchy", false).GetComponent<UIHierarchy>();
            }

            var uiHierarchyGO = uiHierarchy.gameObject;

            var layerGO = new GameObject(layerName);
            layerGO.transform.SetParent(uiHierarchyGO.transform, false);
            layerGO.layer = LayerMask.NameToLayer(kUILayerName);

            var layer = layerGO.AddComponent<UILayer>();
            var layerTransform = layerGO.GetComponent<RectTransform>();
            layer.cachedTransform = layerTransform;
            layer.cachedGameObject = layerGO;

            layerTransform.SetStretchAnchorAll();

            var layerData = new UILayerData()
            {
                name = layerName,
                layer = layer,
            };

            if (uiHierarchy.layers == null)
            {
                uiHierarchy.layers = new UILayerData[0];
            }
            ArrayUtility.Add(ref uiHierarchy.layers, layerData);

            return layerGO;
        }

        private static GameObject CreateEventSystem(bool select, GameObject parent)
        {
            var esys = Object.FindObjectOfType<EventSystem>();
            if (esys == null)
            {
                var eventSystem = new GameObject("EventSystem");
                GameObjectUtility.SetParentAndAlign(eventSystem, parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
            }

            if (select && esys != null)
            {
                Selection.activeGameObject = esys.gameObject;
            }

            return esys.gameObject;
        }

        // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
        static public GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameobject that is the selected GO or one if its parents.
            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use just any canvas..
            canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in the scene at all? Then create a new one.
            return UIMenuOptions.CreateNewUI();
        }
    }
}
