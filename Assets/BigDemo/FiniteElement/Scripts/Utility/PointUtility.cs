using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public static class PointUtility {
    /// <summary>
    /// 保存不用的points
    /// </summary>
    /// <param name="point"></param>
    public static void SavePoints(GameObject[] pointss)
    {
        foreach (var item in pointss)
        {
            if (item != null)
            {
                Object.Destroy(item);
            }
        }
    }

    /// <summary>
    /// 利用网格信息绘制网格点
    /// </summary>
    /// <param name="meshData"></param>
    public static GameObject[] CreatePointsByMeshStruct(MeshStruct meshData, GameObject sprefab, bool boraderOnly = true)
    {
        List<Vector3> pointsTemp = new List<Vector3>();
        List<GameObject> sprites = new List<GameObject>();

        List<Vector3> newPoints = new List<Vector3>();
        float maxx = 0;
        float maxy = 0;
        float maxz = 0;//只创建边线上
        foreach (var item in meshData.vertices)
        {
            if (!pointsTemp.Contains(item))
            {
                newPoints.Add(item);
                maxx = maxx > item.x ? maxx : item.x;
                maxy = maxx > item.y ? maxy : item.y;
                maxz = maxx > item.z ? maxz : item.z;
                pointsTemp.Add(item);//防止重复记录
            }
        }

        foreach (var item in newPoints)
        {
            if (!boraderOnly || (Mathf.Abs(item.x) == maxx && Mathf.Abs(item.y) == maxy) || (Mathf.Abs(item.y) == maxy && Mathf.Abs(item.z) == maxz || ((Mathf.Abs(item.z) == maxz && Mathf.Abs(item.x) == maxx))))
            {
                GameObject sprite = GameObject.Instantiate(sprefab);
                sprite.SetActive(true);
                sprite.transform.position = item;
                sprites.Add(sprite);
            }
        }
        return sprites.ToArray();
    }
    
    /// <summary>
    /// 按给定的线条和间距创建网格点
    /// </summary>
    /// <returns></returns>
    public static GameObject[] CratePointsByPoints(PointData data, GameObject sprefab)
    {
        List<GameObject> sprites = new List<GameObject>();
        List<Vector3> pointsTemp = new List<Vector3>();
        pointsTemp.AddRange(data.HeadPoints);
        pointsTemp.AddRange(data.InnerPoints);
        for (int i = 0; i < pointsTemp.Count; i++)
        {
            GameObject sprite = GameObject.Instantiate(sprefab);
            sprite.SetActive(true);
            sprite.transform.position = pointsTemp[i];
            sprites.Add(sprite);
        }
        return sprites.ToArray();
    }
    /// <summary>
    /// 利用网格信息得到线集合
    /// </summary>
    /// <param name="meshData"></param>
    /// <returns></returns>
    public static void UpdatePointLines(MeshStruct meshData,ref PointLines pointLine)
    {
        for (int i = 0; i < meshData.triangles.Length - 2; i+=3)
        {
            PointLine.GetPointLines(pointLine,i,
               new Vector3[] { meshData.vertices[meshData.triangles[i]], meshData.vertices[meshData.triangles[i + 1]], meshData.vertices[meshData.triangles[i + 2]] });
        }
    }

    /// <summary>
    /// 将网格点连接成线
    /// </summary>
    /// <param name="points"></param>
    public static void DrawGridLines(PointLines pointLines,/*bool defut,*/Color32 defultColor)
    {
        foreach (var item in pointLines.data)
        {
            /*if(defut)*/ GL.Color(defultColor);
            //else GL.Color(item.centercolor);
            GL.Vertex(item.startPoint);
            GL.Vertex(item.endPoint);
        }
    }
}
