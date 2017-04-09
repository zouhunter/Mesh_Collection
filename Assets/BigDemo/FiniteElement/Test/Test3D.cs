using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Test3D : MonoBehaviour
{
    // 注意：该类继承EditorWindow，只能包含静态成员

    static Mesh mesh;            // 网格
    static Vector3[] Vs;         // 模型顶点坐标数组
    static Vector2[] UVs;        // UV贴图坐标
    static Vector3[] normals;    // 法线
    static Vector4[] tangents;   // 切线
    static int[] Ts;             // 三角形的点序列

    // 添加菜单项，并放置最顶端
    void Start()
    {
        // 可自定义修改顶点和面
        SetVsAndTs();
        // 生成网格生成需要的数据
        GenerateCore();
        // 调用创建对象函数
        CreateObjectByMesh();
    }
    //设置顶点和三角面
    static void SetVsAndTs()
    {
        Vs = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f,0.5f, -0.5f),
            new Vector3(-0.5f,0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f)
        };


        Ts = new int[]
        {
            0,2,1,
            0,3,2,
            3,4,2,
            4,5,2,
            4,7,5,
            7,6,5,
            7,0,1,
            6,7,1,
            4,3,0,
            4,0,7,
            2,5,6,
            2,6,1
        };
    }

    //生成网格的核心代码
    static void GenerateCore()
    {
        //根据面的顺序，重新创建新的顶点数组，用于计算顶点法线
        Vector3[] newVs = new Vector3[Ts.Length];
        for (int i = 0; i < newVs.Length; i++)
        {
            newVs[i] = Vs[Ts[i]];
        }
        Vs = newVs;
        UVs = new Vector2[Vs.Length];
        normals = new Vector3[Vs.Length];
        tangents = new Vector4[Vs.Length];

        // 根据新的点，设置三角面的顶点ID并计算点法线
        for (int i = 0; i < Ts.Length - 2; i += 3)
        {
            Vector3 normal = Vector3.Normalize(Vector3.Cross(Vs[i + 1] - Vs[i], Vs[i + 2] - Vs[i]));  // 计算点的法线
            for (int j = 0; j < 3; j++)
            {
                Ts[i + j] = i + j;        // 重新设置面的顶点ID
                normals[i + j] = normal;  // 点的法线赋值
            }
        }

        // 设置每个点的切线和UV
        for (int i = 0; i < Vs.Length; i++)
        {
            tangents[i] = new Vector4(-1, 0, 0, -1);    // 切线
            if (normals[i] == Vector3.back || normals[i] == Vector3.forward)
            {
                UVs[i] = new Vector2((Vs[i].x + 0.5f) * 0.928f , (Vs[i].y + 0.5f) * 0.42f + 0.292f);     // UV坐标
            }
            else if(normals[i] == Vector3.up)
            {
                UVs[i] = new Vector2((Vs[i].x + 0.5f) * 0.928f, (Vs[i].z + 0.5f) * 0.292f + 0.708f);     // UV坐标
            }
            else if (normals[i] == Vector3.down)
            {
                UVs[i] = new Vector2((Vs[i].x + 0.5f) * 0.928f, (Vs[i].z + 0.5f) * 0.292f);     // UV坐标
            }
            else if (normals[i] == Vector3.left || normals[i] == Vector3.right)
            {
                UVs[i] = new Vector2((Vs[i].z + 0.5f) * 0.072f + 0.928f, (Vs[i].y + 0.5f) * 0.42f + 0.292f);     // UV坐标
            }
        }

    }
    // 创建对象函数（这个功能提出来，方便以后扩展）
    void CreateObjectByMesh()
    {
        mesh = new Mesh();                      // 创建网格
        mesh.vertices = Vs;                     // 网格的顶点
        mesh.triangles = Ts;                    // 网格的三角形
        mesh.uv = UVs;                          // 网格的UV坐标
        mesh.normals = normals;                 // 网格的法线
        mesh.tangents = tangents;               // 网格的切线
        gameObject.GetComponent<MeshFilter>().mesh = mesh; // 添加网格
    }
}
