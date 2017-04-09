using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SteelHead {
    public float length;
    public float height;
    public float wigth;
    public float emptyLength;//空间隔
    public float distence;
    float left;
    float right;
    float front;
    float back;
    float up;
    float down;
    float iright;
    float ileft;

    /// <summary>
    /// 计算点信息
    /// </summary>
    /// <param name="span"></param>
    /// <returns></returns>
    public PointData GetPoints(float span)
    {
        InitKeyPoints();

        PointData data = new PointData();
        AddLeftBox(data,span);
        AddRightBox(data, span);
        AddDownLine(data, span);
        AddInnerRects(data,span);
        return data;
    }
    void InitKeyPoints()
    {
        left = -length * 0.5f;
        right = length * 0.5f;
        front = wigth * 0.5f;
        back = -wigth * 0.5f;
        up = height;
        down = 0;
        iright = -emptyLength * 0.5f;
        ileft = emptyLength * 0.5f;
    }
    void AddLeftBox(PointData data,float span)
    {
        PointData.Box leftBox = new PointData.Box();
        leftBox.ldb = new Vector3(left, down, back);
        leftBox.lub = new Vector3(left, up, back);
        leftBox.rub = new Vector3(iright, up, back);
        leftBox.rdb = new Vector3(iright, down, back);
        leftBox.ldf = new Vector3(left, down, front);
        leftBox.luf = new Vector3(left, up, front);
        leftBox.ruf = new Vector3(iright, up, front);
        leftBox.rdf = new Vector3(iright, down, front);

        data.AddPointData(leftBox, span);
    }
    void AddRightBox(PointData data, float span)
    {
        PointData.Box rightBox = new PointData.Box();

        rightBox.ldb = new Vector3(ileft, down, back);
        rightBox.lub = new Vector3(ileft, up, back);
        rightBox.rub = new Vector3(right, up, back);
        rightBox.rdb = new Vector3(right, down, back);
        rightBox.ldf = new Vector3(ileft, down, front);
        rightBox.luf = new Vector3(ileft, up, front);
        rightBox.ruf = new Vector3(right, up, front);
        rightBox.rdf = new Vector3(right, down, front);

        data.AddPointData(rightBox, span);
    }
    void AddDownLine(PointData data, float span)
    {
        Vector3 bl = new Vector3(iright, down, back);
        Vector3 br = new Vector3(ileft, down, back);
        Vector3 fl = new Vector3(iright, down, front);
        Vector3 fr = new Vector3(ileft, down, front);

        data.AddPointData(bl,br, span);
        data.AddPointData(fl, fr, span);
    }
    void AddInnerRects(PointData data, float span)
    {
        for (float i = left + distence; i < iright; i+= distence)
        {
            PointData.Rect rect = new PointData.Rect();
            rect.db = new Vector3(i, down,back);
            rect.ub = new Vector3(i,up,back);
            rect.uf = new Vector3(i,up,front);
            rect.df = new Vector3(i,down,front);
            data.AddPointData(rect, span,true);
        }

        for (float i = right - distence; i > ileft; i -= distence)
        {
            PointData.Rect rect = new PointData.Rect();
            rect.db = new Vector3(i,down,back);
            rect.ub = new Vector3(i,up,back);
            rect.uf = new Vector3(i,up,front);
            rect.df = new Vector3(i,down,front);
            data.AddPointData(rect, span, true);
        }
    }
}
