using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneObject : MonoBehaviour
{
    public Color BaseColor;

    public void SetBaseColor()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (!(renderer is null))
            renderer.sharedMaterial.color = BaseColor;
    }
}
