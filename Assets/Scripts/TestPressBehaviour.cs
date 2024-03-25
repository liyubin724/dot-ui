using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPressBehaviour : MonoBehaviour
{
    public void PrintLog(string message)
    {
        Debug.Log($"{name}=>{message}");
    }
}
