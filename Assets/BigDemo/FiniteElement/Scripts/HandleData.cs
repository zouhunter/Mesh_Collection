using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
/// <summary>
/// 弯曲梁信息
/// </summary>
/// 
[System.Serializable]
public class HandleData
{
    public float maxLow;
    public Vector3 LeftCenterPoint;
    public Vector3 RightCenterPoint;
    public Vector3 HandPoint1;
    public Vector3 HandPoint2;
    public void ResetPoints(Vector3 LeftCenterPoint, Vector3 RightCenterPoint,Vector3 HandPoint1, Vector3 HandPoint2)
    {
        this.LeftCenterPoint = LeftCenterPoint;
        this.RightCenterPoint = RightCenterPoint;
        this.HandPoint1 = HandPoint1;
        this.HandPoint2 = HandPoint2;
    }
    /// <summary>
    /// 更新控制点的坐标
    /// </summary>
    /// <param name="progress"></param>
    public void RefleshHandPoint(float progress)
    {
        float y = -maxLow * progress;
        HandPoint2.y = HandPoint1.y = y;
    }
}
