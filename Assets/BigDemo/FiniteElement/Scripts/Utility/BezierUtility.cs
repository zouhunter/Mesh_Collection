using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public static class BezierUtility {

    /// <summary>
    /// 通用bezier曲线计算公式
    /// </summary>
    /// <param name="t"></param>
    /// <param name="points"></param>
    /// <returns></returns>
    public static Vector3 CalculateBezierPoint(float t, params Vector3[] points)
    {
        Vector3 point = Vector3.zero;
        int n = points.Length - 1;
        for (int i = 0; i <= n; i++)
        {
            point += C(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * points[i];
        }
        return point;
    }

    /// <summary>   
    /// 排列循环方法   
    /// </summary>   
    /// <param name="N"></param>   
    /// <param name="R"></param>   
    /// <returns></returns>   
    static long P1(int N, int R)
    {
        if (R == 0) return 1;
        if (R > N || R <= 0 || N <= 0) Debug.LogError("N:" + N + "\nR:" + R);
        long t = 1;
        int i = N;

        while (i != N - R)
        {
            try
            {
                checked
                {
                    t *= i;
                }
            }
            catch
            {
                Debug.LogError("overflow happens!");
            }
            --i;
        }
        return t;
    }

    /// <summary>   
    /// 排列堆栈方法   
    /// </summary>   
    /// <param name="N"></param>   
    /// <param name="R"></param>   
    /// <returns></returns>   
    static long P2(int N, int R)
    {
        if (R > N || R <= 0 || N <= 0) Debug.LogError("arguments invalid!");
        Stack<int> s = new Stack<int>();
        long iRlt = 1;
        int t;
        s.Push(N);
        while ((t = s.Peek()) != N - R)
        {
            try
            {
                checked
                {
                    iRlt *= t;
                }
            }
            catch
            {
                Debug.LogError("overflow happens!");
            }
            s.Pop();
            s.Push(t - 1);
        }
        return iRlt;
    }

    /// <summary>   
    /// 组合   
    /// </summary>   
    /// <param name="N"></param>   
    /// <param name="R"></param>   
    /// <returns></returns>   
    static long C(int N, int R)
    {
        return P1(N, R) / P1(R, R);
    }
}
