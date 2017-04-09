using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class SteelSystem : MonoBehaviour, ICurveModle
{
    public HandleData handleData;
    [Range(1, 5)]
    public int lineWidth;
    public Texture defultTexture;
    public List<Texture> textures;
    private Material m_gridMaterial;
    private GameObject[] m_points;
    private MeshStruct _meshStruct;
    private MeshStruct _meshStructStatic;
    private MeshRenderer m_meshRender;
    private MeshFilter m_meshFilter;
    [SerializeField]
    private SteelHead _steelData;
    [SerializeField]
    private GameObject m_pointPfb;
    private int _size;
    private void Awake()
    {
        m_gridMaterial = MaterialUtility.GetGridViewMaterial(defultTexture, lineWidth);
        m_meshRender = GetComponent<MeshRenderer>();
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRender.material = m_gridMaterial;
    }
    private void Start()
    {
        _meshStruct = new MeshStruct(m_meshFilter.mesh);
        for (int i = 0; i < _meshStruct.vertices.Length; i++)
        {
            _meshStruct.vertices[i] = new Vector3(_meshStruct.vertices[i].x, _meshStruct.vertices[i].z, _meshStruct.vertices[i].y);
        }
        transform.localRotation = Quaternion.identity;
        SetMeshStructToMesh();
        _meshStructStatic = new MeshStruct(_meshStruct);
    }
    /// <summary>
    /// 压弯弧度变化 
    /// </summary>
    /// <param name="progress"></param>
    public void ChangeCurve(float progress)
    {
        progress = Mathf.Clamp01(progress);
        handleData.RefleshHandPoint(progress);
        MeshUtility.CurveMeshStruct(handleData, _meshStructStatic, ref _meshStruct);
        SetMeshStructToMesh();
        ChangeTexture(Mathf.FloorToInt(textures.Count * progress));
    }

    /// <summary>
    /// 用网格数据生成网格
    /// </summary>
    private void SetMeshStructToMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _meshStruct.vertices;               // 网格的顶点
        mesh.triangles = _meshStruct.triangles;             // 网格的三角形
        mesh.uv = _meshStruct.uvs;                          // 网格的UV坐标
        mesh.normals = _meshStruct.normals;                 // 网格的法线
        mesh.tangents = _meshStruct.tangents;               // 网格的切线
        //mesh.colors32 = gridInfo.Colors32;
        mesh.RecalculateBounds();
        m_meshFilter.mesh = mesh;                          // 添加网格
    }

    private void ChangeTexture(int progress)
    {
        Debug.Log(_size);
        if (_size >= 10 && _size < 100)
        {
            progress = Mathf.FloorToInt(progress * 5f / 8f);
        }
        else if (_size >= 100)
        {
            progress = Mathf.FloorToInt(progress * 3f / 8f);
        }

        progress = Mathf.Clamp(progress, 0, textures.Count - 1);
        Texture txe = textures[progress];
        m_gridMaterial.mainTexture = txe;
    }

    public bool CreatePoints(int size)
    {
        this._size = size;
        if (m_points != null)
            PointUtility.SavePoints(m_points);
        m_points = PointUtility.CratePointsByPoints(_steelData.GetPoints(_size * 0.01f), m_pointPfb);
        foreach (var item in m_points)
        {
            item.transform.SetParent(transform, false);
            //item.transform.position += transform.position;
        }
        return true;
    }

    public void CreateLines(bool clearOld)
    {
        if (m_points != null)
            PointUtility.SavePoints(m_points);
        ChangeTexture(0);
    }
}
