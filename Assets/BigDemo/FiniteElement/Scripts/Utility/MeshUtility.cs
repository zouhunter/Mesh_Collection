using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public struct Vector3Int
{
    public int x;
    public int y;
    public int z;
    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3Int(Vector3 value)
    {
        x = (int)value.x;
        y = (int)value.y;
        z = (int)value.z;
    }
    public static implicit operator Vector3Int(Vector3 value)
    {
        return new Vector3Int((int)value.x, (int)value.y, (int)value.z);
    }
    public override string ToString()
    {
        return "{" + x + ", " + y + ", " + z + "}";
    }
}

public static class MeshUtility
{
    static int vIndex, tIndex;

    /// <summary>
    /// 按要求创建网格体信息
    /// </summary>
    /// <param name="binfo"></param>
    /// <param name="size"></param>
    /// <returns></returns>
	public static MeshStruct CreateBeamMesh(BeamInfo binfo)
    {
        Vector3Int _grid = new Vector3Int();
        // 根据【单元格大小】计算【格子个数】  
        _grid.x = Mathf.CeilToInt((float)binfo.length / binfo.gridSize) + 1;
        _grid.y = Mathf.CeilToInt((float)binfo.height / binfo.gridSize) + 1;
        _grid.z = Mathf.CeilToInt((float)binfo.wigth / binfo.gridSize) + 1;
        MeshStruct _meshStruct = AllocateMeshArrays(_grid);
        CreateMeshGrid(_grid, binfo, ref _meshStruct);
        return _meshStruct;
    }

    /// <summary>
    /// 按要求将网格，得到新的网格
    /// </summary>
    /// <param name="meshData"></param>
    /// <param name=""></param>
    /// <returns></returns>
    public static void CurveMeshStruct(HandleData handle, MeshStruct meshDataTemp,ref MeshStruct meshData)
    {
        for (int i = 0; i < meshData.vertices.Length; i++)
        {
            Vector3 item = meshData.vertices[i];
            //区域内部
            if (item.x > handle.LeftCenterPoint.x && item.x < handle.RightCenterPoint.x)
            {
                Vector3 newCenterPos = BezierUtility.CalculateBezierPoint((item.x - handle.LeftCenterPoint.x) / (handle.RightCenterPoint.x - handle.LeftCenterPoint.x), handle.LeftCenterPoint, handle.HandPoint1, handle.HandPoint2, handle.RightCenterPoint);
                meshData.vertices[i].y = meshDataTemp.vertices[i].y + newCenterPos.y;
            }
        }
    }

    static MeshStruct AllocateMeshArrays(Vector3Int _grid)
    {
        int numVertices = 2 * (
             _grid.x * _grid.z +  //上下
             _grid.y * _grid.z +  //左右
             _grid.x * _grid.y);  // 前后
        int NumTriangle = 2 * 2 * 3 * (
            (_grid.x - 1) * (_grid.z - 1) +        //上下
             (_grid.y - 1) * (_grid.z - 1) +       //左右
              (_grid.x - 1) * (_grid.y - 1));      // 前后
        MeshStruct _meshStruct = new MeshStruct();
        _meshStruct.vertices = new Vector3[numVertices];
        _meshStruct.normals = new Vector3[numVertices];
        _meshStruct.tangents = new Vector4[numVertices];
        _meshStruct.Colors32 = new Color32[numVertices];
        _meshStruct.uvs = new Vector2[numVertices];
        _meshStruct.triangles = new int[NumTriangle];
        return _meshStruct;
    }

    static void CreateMeshGrid(Vector3Int _grid, BeamInfo binfo, ref MeshStruct _meshStruct)
    {
        vIndex = tIndex = 0;
        float xMin = -binfo.length * 0.5f;
        float xMax = binfo.length * 0.5f;
        float yMin = -binfo.height * 0.5f;
        float yMax = binfo.height * 0.5f;
        float zMin = -binfo.wigth * 0.5f;
        float zMax = binfo.wigth * 0.5f;
        CreateMeshPlane(_grid.x, _grid.z,//上
            (i, j) => { return Mathf.Clamp(i * binfo.gridSize + xMin, xMin, xMax); },
            (i, j) => { return yMax; },
            (i, j) => { return Mathf.Clamp(-j * binfo.gridSize + zMax, zMin, zMax); },
            /*new Rect(0, 0.7083f, 0.9278f, 0.2917f)*/
            (x, y, z) => { return 0 + ((x + xMax) / binfo.length) * 0.9278f; },
            (x, y, z) => { return 0.7083f + ((z + zMax) / binfo.wigth) * 0.2917f; },
            Vector3.up, ref _meshStruct, true);
        CreateMeshPlane(_grid.x, _grid.z,//下
            (i, j) => { return Mathf.Clamp(i * binfo.gridSize + xMin, xMin, xMax); },
            (i, j) => { return yMin; },
            (i, j) => { return Mathf.Clamp(-j * binfo.gridSize + zMax, zMin, zMax); },
            //new Rect(0, 0, 0.9278f, 0.2917f),
            (x, y, z) => { return 0 + ((x + xMax) / binfo.length) * 0.9278f; },
            (x, y, z) => { return 0 + ((z + zMax) / binfo.wigth) * 0.2917f; },
            Vector3.down, ref _meshStruct);
        CreateMeshPlane(_grid.z, _grid.y,//左
           (i, j) => { return xMin; },
           (i, j) => { return Mathf.Clamp(j * binfo.gridSize + yMin, yMin, yMax); },
           (i, j) => { return Mathf.Clamp(-i * binfo.gridSize + zMax, zMin, zMax); },
            //new Rect(0.9278f, 0.2917f, 0.0722f, 0.4167f), 
            (x, y, z) => { return 0.9278f + ((z + zMax) / binfo.wigth) * 0.0722f; },
            (x, y, z) => { return 0.2917f + ((y + yMax) / binfo.height) * 0.4167f; },
           Vector3.left, ref _meshStruct);
        CreateMeshPlane(_grid.z, _grid.y,//右
          (i, j) => { return xMax; },
          (i, j) => { return Mathf.Clamp(j * binfo.gridSize + yMin, yMin, yMax); },
          (i, j) => { return Mathf.Clamp(-i * binfo.gridSize + zMax, zMin, zMax); },
           //new Rect(0.9278f, 0.2917f, 0.0722f, 0.4167f),
           (x, y, z) => { return 0.9278f + ((z + zMax) / binfo.wigth) * 0.0722f; },
            (x, y, z) => { return 0.2917f + ((y + yMax) / binfo.height) * 0.4167f; },
          Vector3.right, ref _meshStruct, true);
        CreateMeshPlane(_grid.x, _grid.y,//前
           (i, j) => { return Mathf.Clamp(i * binfo.gridSize + xMin, xMin, xMax); },
           (i, j) => { return Mathf.Clamp(j * binfo.gridSize + yMin, yMin, yMax); },
           (i, j) => { return zMin; },
            //new Rect(0f, 0.2917f, 0.9278f, 0.4167f),
            (x, y, z) => { return 0 + ((x + xMax) / binfo.length) * 0.9278f; },
            (x, y, z) => { return 0.2917f + ((y + yMax) / binfo.height) * 0.4167f; },
           Vector3.back, ref _meshStruct);
        CreateMeshPlane(_grid.x, _grid.y,//后
          (i, j) => { return Mathf.Clamp(i * binfo.gridSize + xMin, xMin, xMax); },
          (i, j) => { return Mathf.Clamp(j * binfo.gridSize + yMin, yMin, yMax); },
          (i, j) => { return zMax; },
          //new Rect(0f, 0.2917f, 0.9278f, 0.4167f),
          (x, y, z) => { return 0 + ((x + xMax) / binfo.length) * 0.9278f; },
            (x, y, z) => { return 0.2917f + ((y + yMax) / binfo.height) * 0.4167f; },
          Vector3.forward, ref _meshStruct, true);
    }

    /// <summary>
    /// 创建出一个面的mesh数据
    /// </summary>
    /// <param name="axisX"></param>
    /// <param name="axisY"></param>
    /// <param name="vx"></param>
    /// <param name="vy"></param>
    /// <param name="vz"></param>
    /// <param name="uvstapX"></param>
    /// <param name="uvstapY"></param>
    /// <param name="normal"></param>
    static void CreateMeshPlane(int axisX, int axisY,
        System.Func<int, int, float> vx,
        System.Func<int, int, float> vy,
        System.Func<int, int, float> vz,
        System.Func<float, float, float, float> vuvx,
        System.Func<float, float, float, float> vuvy,
        Vector3 normal, ref MeshStruct _meshStruct, bool revers = false)
    {
        int start = vIndex;
        for (int j = 0; j < axisY; j++)
        {
            for (int i = 0; i < axisX; i++)
            {
                // Set vertices  
                _meshStruct.vertices[vIndex].x = vx(i, j);
                _meshStruct.vertices[vIndex].y = vy(i, j);
                _meshStruct.vertices[vIndex].z = vz(i, j);

                // Set triangles  
                if (j < axisY - 1 && i < axisX - 1)
                {
                    _meshStruct.triangles[tIndex + 0] = revers ? (j * axisX) + i + 1 + start : (j * axisX) + i + start;
                    _meshStruct.triangles[tIndex + 1] = ((j + 1) * axisX) + i + start;
                    _meshStruct.triangles[tIndex + 2] = revers ? (j * axisX) + i + start : (j * axisX) + i + 1 + start;

                    _meshStruct.triangles[tIndex + 3] = revers ? (j * axisX) + i + 1 + start : ((j + 1) * axisX) + i + start;
                    _meshStruct.triangles[tIndex + 4] = ((j + 1) * axisX) + i + 1 + start;
                    _meshStruct.triangles[tIndex + 5] = revers ? ((j + 1) * axisX) + i + start : (j * axisX) + i + 1 + start;

                    tIndex += 6;
                }
                float uvx = vuvx(_meshStruct.vertices[vIndex].x, _meshStruct.vertices[vIndex].y, _meshStruct.vertices[vIndex].z);//uvRect.x + i * uvstapX * uvRect.width;
                float uvy = vuvy(_meshStruct.vertices[vIndex].x, _meshStruct.vertices[vIndex].y, _meshStruct.vertices[vIndex].z);//uvRect.y + j * uvstapY * uvRect.height;
                SetOther(uvx, uvy, normal, ref _meshStruct);
                vIndex++;
            }
        }
    }

    static void SetOther(float uvx, float uvy, Vector3 normal, ref MeshStruct _meshStruct)
    {
        //Color32 colorOne = new Color32(255, 255, 255, 255);
        bool setTangents = true;
        Vector4 tangent = new Vector4(1f, 0f, 0f, 1f);
        // Set UV  
        _meshStruct.uvs[vIndex].x = uvx;
        _meshStruct.uvs[vIndex].y = uvy;

        //// Set colors  
        //_meshStruct.Colors32[vIndex] = colorOne;

        // Set normals  
        _meshStruct.normals[vIndex] = normal;

        if (setTangents)
        {
            // set tangents  
            _meshStruct.tangents[vIndex] = tangent;
        }

    }
    /// <summary>
    /// 默认网格体绘制
    /// </summary>
    /// <param name="binfo"></param>
   // static MeshStruct GenerateSimpleBox(BeamInfo binfo)
   // {
   //     //Color32 colorOne = new Color32(255, 255, 255, 255);

   //     Vector3[] Vs = new Vector3[]
   //    {
   //         new Vector3(-0.5f * binfo.length, -0.5f * binfo.height, -0.5f * binfo.wigth),
   //         new Vector3(0.5f * binfo.length,  -0.5f* binfo.height, -0.5f* binfo.wigth),
   //         new Vector3(0.5f  * binfo.length,  0.5f* binfo.height, -0.5f* binfo.wigth),
   //         new Vector3(-0.5f * binfo.length,  0.5f* binfo.height, -0.5f * binfo.wigth),
   //         new Vector3(-0.5f * binfo.length,  0.5f* binfo.height, 0.5f* binfo.wigth),
   //         new Vector3(0.5f  * binfo.length,  0.5f* binfo.height, 0.5f* binfo.wigth),
   //         new Vector3(0.5f  * binfo.length, -0.5f* binfo.height, 0.5f * binfo.wigth),
   //         new Vector3(-0.5f * binfo.length, -0.5f * binfo.height, 0.5f* binfo.wigth)
   //    };
   //     int[] Ts = new int[]
   //{
   //         0,2,1,
   //         0,3,2,
   //         3,4,2,
   //         4,5,2,
   //         4,7,5,
   //         7,6,5,
   //         7,0,1,
   //         6,7,1,
   //         4,3,0,
   //         4,0,7,
   //         2,5,6,
   //         2,6,1
   //};

   //     //根据面的顺序，重新创建新的顶点数组，用于计算顶点法线
   //     Vector3[] newVs = new Vector3[Ts.Length];
   //     for (int i = 0; i < newVs.Length; i++)
   //     {
   //         newVs[i] = Vs[Ts[i]];
   //     }
   //     Vs = newVs;
   //     Vector2[] UVs = new Vector2[Vs.Length];
   //     Vector3[] normals = new Vector3[Vs.Length];
   //     Vector4[] tangents = new Vector4[Vs.Length];
   //     // 根据新的点，设置三角面的顶点ID并计算点法线
   //     for (int i = 0; i < Ts.Length - 2; i += 3)
   //     {
   //         Vector3 normal = Vector3.Normalize(Vector3.Cross(Vs[i + 1] - Vs[i], Vs[i + 2] - Vs[i]));  // 计算点的法线
   //         for (int j = 0; j < 3; j++)
   //         {
   //             Ts[i + j] = i + j;        // 重新设置面的顶点ID
   //             normals[i + j] = normal;  // 点的法线赋值
   //         }
   //     }

   //     // 设置每个点的切线和UV
   //     for (int i = 0; i < Vs.Length; i++)
   //     {
   //         tangents[i] = new Vector4(-1, 0, 0, -1);    // 切线
   //         if (normals[i] == Vector3.back || normals[i] == Vector3.forward)
   //         {
   //             UVs[i] = new Vector2((Vs[i].x + 0.5f * binfo.length) * 0.928f / binfo.length, (Vs[i].y + 0.5f * binfo.height) * 0.42f / binfo.height + 0.292f);     // UV坐标
   //         }
   //         else if (normals[i] == Vector3.up)
   //         {
   //             UVs[i] = new Vector2((Vs[i].x + 0.5f * binfo.length) * 0.928f / binfo.length, (Vs[i].z + 0.5f * binfo.wigth) * 0.292f / binfo.wigth + 0.708f);     // UV坐标
   //         }
   //         else if (normals[i] == Vector3.down)
   //         {
   //             UVs[i] = new Vector2((Vs[i].x + 0.5f * binfo.length) * 0.928f / binfo.length, (Vs[i].z + 0.5f * binfo.wigth) / binfo.wigth * 0.292f);     // UV坐标
   //         }
   //         else if (normals[i] == Vector3.left || normals[i] == Vector3.right)
   //         {
   //             UVs[i] = new Vector2((Vs[i].z + 0.5f * binfo.wigth) * 0.072f / binfo.wigth + 0.928f, (Vs[i].y + 0.5f * binfo.height) * 0.42f / binfo.height + 0.292f);     // UV坐标
   //         }
   //     }
   //     MeshStruct bMesh = new MeshStruct(Vs, Ts, UVs, normals, tangents);
   //     return bMesh;
   // }

}
