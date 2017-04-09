using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointData {

    private List<Vector3> _ipoints = new List<Vector3>();
    private List<Vector3> _hPoints = new List<Vector3>();
    public List<Vector3> InnerPoints { get { return _ipoints; } }
    public List<Vector3> HeadPoints { get { return _hPoints; } }
    public class Box
    {
       public Vector3 ldb;
       public Vector3 lub;
       public Vector3 rub;
       public Vector3 rdb;
       public Vector3 ldf;
       public Vector3 luf;
       public Vector3 ruf;
       public Vector3 rdf;
    }
    public class Rect
    {
        public Vector3 db;
        public Vector3 ub;
        public Vector3 uf;
        public Vector3 df;
    }


    public void ClearPoints()
    {
        _ipoints.Clear();
        _hPoints.Clear();
    }
    /// <summary>
    /// 直线
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="span"></param>
    public void AddPointData(Vector3 start,Vector3 end,float span)
    {
        RegisterPointsNoRepet(_hPoints,start,end);
        RegisterInnerPoint(start, end,span);
    }

    /// <summary>
    /// 矩形
    /// </summary>
    /// <param name="ldb">左后下</param>
    /// <param name="lub">左后上</param>
    /// <param name="rub">右后上</param>
    /// <param name="rdb">右后下</param>
    /// <param name="ldf">左前下</param>
    /// <param name="luf">左前上</param>
    /// <param name="ruf">右前上</param>
    /// <param name="rdf">右前下</param>
    /// <param name="span"></param>
    public void AddPointData(Vector3 ldb,Vector3 lub, Vector3 rub, Vector3 rdb, Vector3 ldf, Vector3 luf, Vector3 ruf, Vector3 rdf, float span)
    {
        RegisterPointsNoRepet(_hPoints,ldb,lub,rub,rdb,ldf,luf,ruf,rdf);

        RegisterInnerPoint(ldb,lub, span);//左后下
        RegisterInnerPoint(ldb,ldf, span);
        RegisterInnerPoint(ldb,rdb, span);

        RegisterInnerPoint(luf,ldf, span);//左前上
        RegisterInnerPoint(luf,ruf, span);
        RegisterInnerPoint(luf,lub, span);

        RegisterInnerPoint(rub, rdb, span);//右后上
        RegisterInnerPoint(rub, lub, span);
        RegisterInnerPoint(rub, ruf, span);

        RegisterInnerPoint(rdf, ruf, span);//右前下
        RegisterInnerPoint(rdf, ldf, span);
        RegisterInnerPoint(rdf, rdb, span);
    }
    public void AddPointData(Box box,float span)
    {
        AddPointData(box.ldb, box.lub, box.rub, box.rdb, box.ldf, box.luf, box.ruf, box.rdf, span);
    }
    public void AddPointData(Vector3 db, Vector3 ub,Vector3 uf,Vector3 df,float span,bool noHead = false)
    {
        if(!noHead) RegisterPointsNoRepet(_hPoints, db, ub, uf, df);
        RegisterInnerPoint(db, ub,span);
        RegisterInnerPoint(ub, uf, span);
        RegisterInnerPoint(uf, df, span);
        RegisterInnerPoint(df, db, span);
    }
    public void AddPointData(Rect rect,float span,bool noHead = false)
    {
        AddPointData(rect.db, rect.ub, rect.uf, rect.df, span,noHead);
    }
 
    private void RegisterInnerPoint(Vector3 start, Vector3 end,float span)
    {
        float distence = Vector3.Distance(start, end);
        if (span < distence)
        {
            for (float i = span; i < distence; i += span)
            {
                Vector3 pos = Vector3.Lerp(start, end, i / distence);
                RegisterPointsNoRepet(_ipoints, pos);
            }
        }
    }

    private void RegisterPointsNoRepet(List<Vector3> target,params Vector3[] poss)
    {
        foreach (var item in poss)
        {
            if (!target.Contains(item))
            {
                target.Add(item);
            }
        }
    }
}
