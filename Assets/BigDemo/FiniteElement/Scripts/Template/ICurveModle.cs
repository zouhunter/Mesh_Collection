using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可压弯模型
/// </summary>
public interface ICurveModle {
    //改变弯曲程度
    void ChangeCurve(float progress);
    bool CreatePoints(int size);
    void CreateLines(bool clearOld);
}
