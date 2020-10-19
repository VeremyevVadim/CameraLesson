using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeColorItem
{
    [MenuItem("Task/Change color to yellow")]
    public static void ChangeColorToYellow()
    {
        var sceneObjects = GameObject.FindObjectsOfType<MeshRenderer>();
        foreach (var obj in sceneObjects)
        {
            obj.sharedMaterial.color = Color.yellow;
        }
    }
    
    [MenuItem("Task/Change color to red")]
    public static void ChangeColorToRed()
    {
        var sceneObjects = GameObject.FindObjectsOfType<MeshRenderer>();
        foreach (var obj in sceneObjects)
        {
            obj.sharedMaterial.color = Color.red;
        }
    }
}
