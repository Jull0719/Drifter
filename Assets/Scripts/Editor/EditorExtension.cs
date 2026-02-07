using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorExtension : Editor
{
    [MenuItem("EditorExtensions/Open SaveDataPath")]
    static void OpenSavePath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath + "/");
    }
}
