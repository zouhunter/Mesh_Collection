using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public struct MeshStruct  {
    public Vector3[] vertices;    // 网格的顶点
    public int[] triangles;       // 网格的三角形
    public Vector2[] uvs;         // 网格的UV坐标
    public Vector3[] normals;     // 网格的法线
    public Vector4[] tangents;    // 网格的切线
    public Color32[] Colors32;
    public MeshStruct(Vector3[] vertices,int[] triangles,Vector2[] uvs,Vector3[] normals,Vector4[] tangents)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
        this.normals = normals;
        this.tangents = tangents;
        this.Colors32 = new Color32[vertices.Length];
    }
    public MeshStruct(MeshStruct temp)
    {
        this.vertices = new Vector3[temp.vertices.Length]; System.Array.Copy(temp.vertices, this.vertices, this.vertices.Length);
        this.triangles = new int[temp.triangles.Length]; System.Array.Copy(temp.triangles, this.triangles, this.triangles.Length);
        this.uvs = new Vector2[temp.uvs.Length];System.Array.Copy(temp.uvs, this.uvs, this.uvs.Length);
        this.normals = new Vector3[temp.normals.Length];System.Array.Copy(temp.normals, this.normals, this.normals.Length);
        this.tangents = new Vector4[temp.tangents.Length];System.Array.Copy(temp.tangents, this.tangents, this.tangents.Length);
        this.Colors32 = new Color32[temp.Colors32.Length];System.Array.Copy(temp.Colors32, this.Colors32, this.Colors32.Length);
    }
    public MeshStruct(Mesh meshData)
    {
        this.vertices = meshData.vertices;
        this.triangles = meshData.triangles;
        this.uvs = meshData.uv;
        this.normals = meshData.normals;
        this.tangents = meshData.tangents;
        this.Colors32 = meshData.colors32;
    }

    //public void SetColorByTexutre(Texture texture)
    //{
    //    Texture2D t2d = (Texture2D)texture;
    //    for (int i = 0; i < uvs.Length - 2; i+=3)
    //    {
    //        Vector2 item1 = uvs[i];
    //        Vector2 item2 = uvs[i + 1];
    //        Vector2 item3 = uvs[i + 2];

    //        int x = (int)(t2d.width * (item1.x + item2.x + item3.x) /3);
    //        int y = (int)(t2d.height * (item1.y + item2.y + item3.y) / 3);

    //        Color color = t2d.GetPixel(x, y);

    //        Colors32[i] = Colors32[i+1] = Colors32[i+2] = new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
    //    }
    //    //, Color32[] Colors32
    //}
}
