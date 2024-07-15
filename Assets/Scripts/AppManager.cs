using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public void ExitApp()
    {
# if UNITY_EDITOR 
        if (Application.isEditor) EditorApplication.isPlaying = false;
# endif

        Application.Quit();
    }
}
