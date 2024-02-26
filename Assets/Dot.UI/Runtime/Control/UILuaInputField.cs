#if ENABLE_LUA
using DotEngine.Lua;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Lua Input Field", 101)]
    public class UILuaInputField : InputField
    {
        public LuaBehaviour behaviour = null;

        public string changedFuncName = string.Empty;
        public LuaParamValue[] changedFuncValues = new LuaParamValue[0];

        public string submitedFuncName = string.Empty;
        public LuaParamValue[] submitedFuncValues = new LuaParamValue[0];

        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(OnValueChanged);
            onEndEdit.AddListener(OnValueSubmited);
        }

        private void OnValueChanged(string value)
        {
            if (behaviour != null && !string.IsNullOrEmpty(changedFuncName))
            {
                if (changedFuncValues == null || changedFuncValues.Length == 0)
                {
                    behaviour.CallAction(changedFuncName, value);
                }
                else
                {
                    behaviour.CallActionWithParams(changedFuncName, value, changedFuncValues);
                }
            }
        }

        private void OnValueSubmited(string value)
        {
            if (behaviour != null && !string.IsNullOrEmpty(submitedFuncName))
            {
                if (submitedFuncValues == null || submitedFuncValues.Length == 0)
                {
                    behaviour.CallAction(submitedFuncName, value);
                }
                else
                {
                    behaviour.CallActionWithParams(submitedFuncName, value, submitedFuncValues);
                }
            }
        }
    }
}
#endif