#if ENABLE_LUA
using DotEngine.Lua;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Lua Button", 100)]
    public class UILuaButton : Button
    {
        public LuaBehaviour behaviour = null;
        public string funcName = string.Empty;
        public LuaParamValue[] funcValues = new LuaParamValue[0];

        protected override void Awake()
        {
            base.Awake();

            onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            if (behaviour == null)
            {
                Debug.LogError("LuaButton:OnClicked->the behaviour is null");
                return;
            }

            if (string.IsNullOrEmpty(funcName))
            {
                Debug.LogError("LuaButton:OnClicked->the funcName is empty");
                return;
            }

            if (funcValues == null || funcValues.Length == 0)
            {
                behaviour.CallAction(funcName);
            }
            else
            {
                behaviour.CallActionWithParams(funcName, funcValues);
            }
        }
    }
}
#endif