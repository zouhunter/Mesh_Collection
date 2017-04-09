using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class BeamSystem : MonoBehaviour, ICurveModle
{
    public Color32 lineColor;
    public BeamInfo beamInfo;
    public BeamInfo gridBeamInfo;
    public HandleData handleData;
    public List<Texture> yinbianTextures;
    public List<Texture> weiyiTextures;
    public GameObject pointPfb;
    public int lineWidth;
    //private bool _isweiyi;//weiyi
    private bool lineCrated;//创建线与否
    private MeshStruct _meshStructStatic;//未变弯的mesh
    private MeshStruct _meshStruct;//实时的mesh
    private MeshStruct _gridmeshStructStatic;//
    private MeshStruct _gridmeshStruct;//
    private MeshRenderer meshRender;
    private MeshFilter meshFilter;
    private Material lineMaterial;
    private Material yinbianMaterial;
    private Material weiyiMaterial;
    private GameObject[] points;
    private PointLines pointLine = new PointLines();
    private Mesh mesh;
    void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh = new Mesh();
        //_isweiyi = false;
    }
    public void InitMaterial(Texture weiyitexture,Texture yinbiantexture)
    {
        lineMaterial = MaterialUtility.GetLineMaterial(Color.white);
        weiyiMaterial = MaterialUtility.GetStandedMaterial(weiyitexture);
        meshRender.material = yinbianMaterial = MaterialUtility.GetStandedMaterial(yinbiantexture);
    }
    /// <summary>
    /// 初始化梁外框
    /// </summary>
    public void InitBeamView()
    {
        ResetBeamView(false);
        ClearOldPoint();
    }

    /// <summary>
    /// 利用梁的基本信息绘制梁
    /// </summary>
    /// <param name="dir"></param>
    private void ResetBeamView(bool dir)
    {
        _meshStructStatic = MeshUtility.CreateBeamMesh(beamInfo);
        _meshStruct = new MeshStruct(_meshStructStatic);

        _gridmeshStructStatic = MeshUtility.CreateBeamMesh(gridBeamInfo);
        _gridmeshStruct = new MeshStruct(_gridmeshStructStatic);

        SetMeshStructToMesh();
    }
    /// <summary>
    /// 用网格数据生成网格
    /// </summary>
    private void SetMeshStructToMesh()
    {
        mesh = new Mesh();
        mesh.vertices = _meshStruct.vertices;               // 网格的顶点
        mesh.triangles = _meshStruct.triangles;             // 网格的三角形
        mesh.uv = _meshStruct.uvs;                          // 网格的UV坐标
        mesh.normals = _meshStruct.normals;                 // 网格的法线
        mesh.tangents = _meshStruct.tangents;               // 网格的切线
        //mesh.colors32 = gridInfo.Colors32;
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;                          // 添加网格
    }
    /// <summary>
    /// 清除创建的点
    /// </summary>
    private void ClearOldPoint()
    {
        if (points != null) PointUtility.SavePoints(points);
    }

    /// <summary>
    /// 创建网格点
    /// </summary>
    /// <param name="g_size"></param>
    /// <returns></returns>
    public bool CreatePoints(int g_size)
    {
        if (gridBeamInfo.SetGridSize(g_size))
        {
            ResetBeamView(true);
            ClearOldPoint();
            lineCrated = false;
            points = PointUtility.CreatePointsByMeshStruct(_gridmeshStructStatic, pointPfb);
            foreach (var item in points)
            {
                item.transform.SetParent(transform, false);
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 创建网格线
    /// </summary>
    public void CreateLines(bool clearOld)
    {
        ClearOldPoint();
        if (clearOld) pointLine.data.Clear();
        PointUtility.UpdatePointLines(_gridmeshStruct, ref pointLine);
        lineCrated = true;
    }
    /// <summary>
    /// 设置表面可见与否
    /// </summary>
    /// <param name="isweiyi"></param>
    public void SetWeiyiTexture(bool isweiyi)
    {
        //this._isweiyi = isweiyi;
        if (meshRender) meshRender.material = isweiyi ? weiyiMaterial : yinbianMaterial;
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

        // Draw lines 
        GL.Begin(GL.LINES);
        DrawGrid();
        GL.End();
        GL.PopMatrix();
    }
    /// <summary>
    /// 绘制网格
    /// </summary>
    public void DrawGrid()
    {
        if (lineCrated)//绘制线
        {
            PointUtility.DrawGridLines(pointLine, lineColor);
        }
    }

    /// <summary>
    /// 改变弯曲程度(0,1)
    /// </summary>
    public void ChangeCurve(float progress)
    {
        progress = Mathf.Clamp01(progress);
        handleData.RefleshHandPoint(progress);
        MeshUtility.CurveMeshStruct(handleData, _meshStructStatic, ref _meshStruct);
        MeshUtility.CurveMeshStruct(handleData, _gridmeshStructStatic, ref _gridmeshStruct);
        SetMeshStructToMesh();
        ChangeTexture(Mathf.FloorToInt(yinbianTextures.Count * progress));
        CreateLines(false);
    }
    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="progress"></param>
    private void ChangeTexture(int progress)
    {
        //3~180 //[0.03~0.1]=>8 [0.1~1]=>5  [1~x]=>3
        //比例不正常加载不正确
        float dir = gridBeamInfo.gridSize / beamInfo.gridSize;
        if (dir >= 10 && dir < 100)
        {
            progress = Mathf.FloorToInt(progress * 5f / 8f);
        }
        else if (dir >= 100)
        {
            progress = Mathf.FloorToInt(progress * 3f / 8f);
        }

        Debug.Log(progress);

        if (yinbianTextures.Count > progress && progress >= 0)
        {
            yinbianMaterial.mainTexture = yinbianTextures[progress];
            weiyiMaterial.mainTexture = weiyiTextures[progress];
        }
    }

}
