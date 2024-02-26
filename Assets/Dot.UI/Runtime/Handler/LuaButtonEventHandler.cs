#if ENABLE_LUA
using DotEngine.Lua;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Button))]
    public class LuaButtonEventHandler : MonoBehaviour
    {
        public Button button;

        public LuaEventHandler handler = new LuaEventHandler();

        private void Awake()
        {
            if (button != null)
            {
                button = GetComponent<Button>();
            }

            button?.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            button?.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            handler.Invoke();
        }
    }
}
#endif