#if ENABLE_LUA
using DotEngine.Lua;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Lua Button", 102)]
    public class UILuaToggle : Toggle
    {
        public LuaBehaviour binderBehaviour = null;

        public string changedFuncName = string.Empty;
        public LuaParamValue[] changedParamValues = new LuaParamValue[0];

        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            if (binderBehaviour != null && !string.IsNullOrEmpty(changedFuncName))
            {
                if (changedParamValues == null || changedParamValues.Length == 0)
                {
                    binderBehaviour.CallAction(changedFuncName, value);
                }
                else
                {
                    binderBehaviour.CallActionWithParams(changedFuncName, value, changedParamValues);
                }
            }
        }
    }
}
#endif