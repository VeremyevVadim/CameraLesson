using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseSceneObject))]
public class BaseColorInspector : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Base color"))
        {
            var obj = target as BaseSceneObject;

            if (!(obj is null))
                obj.SetBaseColor();
        }
    }
}
