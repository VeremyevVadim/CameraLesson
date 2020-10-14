using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GunMirror : MonoBehaviour
{
    private RenderTexture _mirrorTexture;
    [SerializeField]
    private Camera scopeCamera = null;
    void Start()
    {
        var rend = GetComponent<MeshRenderer> ();
        
        _mirrorTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        _mirrorTexture.Create();

        scopeCamera.targetTexture = _mirrorTexture;
        
        Material mirrorMaterial = new Material(Shader.Find("Unlit/Texture"));
        mirrorMaterial.mainTexture = _mirrorTexture;
        rend.material = mirrorMaterial;

    }

    
    void Update()
    {
        
    }
}
