using DotEngine.UI;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        var uiMgr = UIManager.CreateInstance();
        uiMgr.inputEnable = false;
    }

}
