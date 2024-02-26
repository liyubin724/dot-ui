#if ENABLE_LUA
using DotEngine.Lua;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Lua Input Field", 103)]
    public class UILuaSlider : Slider
    {
        public LuaBehaviour behaviour = null;

        public string changedFuncName = string.Empty;
        public LuaParamValue[] changedFuncValues = new LuaParamValue[0];

        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
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
    }
}
#endif