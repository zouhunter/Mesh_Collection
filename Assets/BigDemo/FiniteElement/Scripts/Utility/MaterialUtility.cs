using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public static class MaterialUtility {

    public static Material GetLineMaterial(Color color)
    {
        // Unity has a built-in shader that is useful for drawing 
        // simple colored things. 
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        Material lineMaterial = new Material(shader); lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending 
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off 
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes 
        lineMaterial.SetInt("_ZWrite", 0);
        lineMaterial.color = color;
        return lineMaterial;
    }
    public static Material GetGridViewMaterial(Texture texture,int lineWidth = 1)
    {
        Shader shader = Shader.Find("UCLA Game Lab/Wireframe/Double-Sided Cutout");
        Material mat = GetMaterialByShader(shader, texture);
        mat.SetFloat("_Thickness", lineWidth);
        return mat;
    }

    public static Material GetParticalMaterial(Texture texture)
    {
        // Unity has a built-in shader that is useful for drawing 
        // simple colored things. 
        Shader shader = Shader.Find("Particles/Alpha Blended");
        return GetMaterialByShader(shader, texture);
    }

    public static Material GetStandedMaterial(Texture texture)
    {
        // Unity has a built-in shader that is useful for drawing 
        // simple colored things. 
        Shader shader = Shader.Find("Standard");
        return GetMaterialByShader(shader, texture);
    }

    public static Material GetMaterialByShader(Shader shader,Texture texture)
    {
        Material mat = new Material(shader);
        mat.mainTexture = texture;
        return mat;
    }
}
