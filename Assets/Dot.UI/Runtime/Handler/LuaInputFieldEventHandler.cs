#if ENABLE_LUA
using DotEngine.Lua;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class LuaInputFieldEventHandler : MonoBehaviour
    {
        public TMP_InputField inputField;

        public LuaEventHandler changedHandler = new LuaEventHandler();
        public LuaEventHandler summitHandler = new LuaEventHandler();

        private void Awake()
        {
            if (inputField == null)
            {
                inputField = GetComponent<TMP_InputField>();
            }

            inputField?.onValueChanged.AddListener(OnValueChanged);
            inputField?.onSubmit.AddListener(OnValueSubmited);
        }

        private void OnDestroy()
        {
            inputField?.onValueChanged.RemoveListener(OnValueChanged);
            inputField?.onSubmit.RemoveListener(OnValueSubmited);
        }

        private void OnValueChanged(string text)
        {
            changedHandler.Invoke(text);
        }

        private void OnValueSubmited(string text)
        {
            summitHandler.Invoke(text);
        }
    }
}
#endif