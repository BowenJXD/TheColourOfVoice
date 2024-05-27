using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRT : MonoBehaviour
{
    public Shader crtShader;

    [Range(1.0f, 10.0f)] public float curvature = 1.0f;
    [Range(1.0f,100.0f)] public float vignetteWidth = 30.0f;

    private Material crtMat;
    
    void Start()
    {
        crtMat = new Material(crtShader);
        crtMat.hideFlags = HideFlags.HideAndDontSave;
    }

    /// <summary>
    /// OnRenderIMage的作用是在渲染图像的时候调用
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("OnRenderImage called！");
        crtMat.SetFloat("_Curvature", curvature);
        crtMat.SetFloat("_VignetteWidth", vignetteWidth);
        Graphics.Blit(source, destination, crtMat);
    }
}
