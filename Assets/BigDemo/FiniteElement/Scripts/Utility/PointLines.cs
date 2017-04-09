using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class PointLines
{
    public List<PointLine> data = new List<PointLine>();
}

public class PointLine
{
    public int id;
    public int i;
    public int j;
    public Vector3 startPoint;
    public Vector3 endPoint;
    //public Color32 centercolor;
    public PointLine(/*Color32 centercolor,*/Vector3 startPoint, Vector3 endPoint,int id,int i,int j)
    {
        //this.centercolor = centercolor;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.id = id;
        this.i = i;
        this.j = j;
    }

    /// <summary>
    /// 按旧记录更新坐标
    /// </summary>
    /// <param name="verticals"></param>
    public void ReFleshPos(/*Color32[] colors,*/ Vector3[] verticals)
    {
        //centercolor = colors[i];
        startPoint = verticals[i];
        endPoint = verticals[j];
    }
    /// <summary>
    /// 利用3个点得到两个pointLine
    /// </summary>
    /// <param name="verticals"></param>
    /// <returns></returns>
    public static void GetPointLines(PointLines pls,int id,/*Color32[] colors,*/ Vector3[] verticals)
    {
        List<PointLine> olds = pls.data.FindAll(x => x.id == id);
        if (olds != null && olds.Count > 0)
        {
            foreach (var item in olds){
                item.ReFleshPos(/*colors,*/verticals);
            }
        }
        else//第一次加载时，可预测
        {
            for (int i = 0; i < verticals.Length; ++i)
                for (int j = i + 1; j < verticals.Length; ++j)
                {
                    Vector3 nor = Vector3.Normalize(verticals[i] - verticals[j]);

                    if (ApproximateRights(nor,true)|| ApproximateRights(nor,false))//轴向
                    {
                        //去除重复的线
                        if (pls.data.Find(x=> { return (x.startPoint == verticals[i] && x.endPoint == verticals[j]) || (x.startPoint == verticals[j] && x.endPoint == verticals[i]); }) == null)
                        {
                            PointLine pl = new PointLine(/*colors[i], */verticals[i], verticals[j], id, i, j);
                            pls.data.Add(pl);
                        }
                    }
                }
        }
    }

    static bool ApproximateRights(Vector3 arraw,bool revers)
    {
        bool appr = false;
        appr |= revers ? arraw == Vector3.right: arraw == Vector3.left;
        appr |= revers ? arraw == Vector3.forward: arraw == Vector3.back;
        appr |= revers ? arraw == Vector3.up : arraw == Vector3.down;
        return appr;
    }
}
