using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GLLines : MonoBehaviour {
    // When added to an object, draws colored rays from the 
    // transform position. 
    Material lineMaterial;

    void Start()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing 
            // simple colored things. 
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader); lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending 
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off 
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes 
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
    // Will be called after all regular rendering is done 
    public void OnRenderObject()
    {
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to 
        // match our transform 
        GL.MultMatrix(transform.localToWorldMatrix);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Draw lines 
        GL.Begin(GL.LINES);
        for (int i = 0; i < mesh.vertexCount; ++i)
        {
            GL.Color(Color.green);
            // One vertex at transform position 
            GL.Vertex3(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z);
            // Another vertex at edge of circle 
            GL.Vertex3(-0.5f,-0.5f,-0.5f);
        }
        GL.End();
        GL.PopMatrix();
    }
}
