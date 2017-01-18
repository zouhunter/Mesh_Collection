using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 脚本功能：扩展Unity编辑器，在GameObject目录下添加“麦比乌斯带”目录以向导方式生成麦比乌斯带3D模型对象
/// 添加对象：无（在Editor文件夹中）
/// 创建时间：2015年6月27日14:16:16
/// 创建人员：杜子兮
/// 版权声明：2015 www.duzixi.com
/// 技术要点：
/// 1. 通过生成，父子化和旋转空对象确认各个顶点位置
/// 2. 根据几何顶点位置生成网格顶点位置
/// </summary>
public class CreateMesh : ScriptableWizard
{
    public int n = 50;       // 切割数
    public float r = 5;      // 半径
    public float h = 0.2f;   // 半高
    public GameObject point; // 顶点的预设体（选择适当的图标可显示顶点号）

    [MenuItem("GameObject/麦比乌斯带")]
    static void Create()
    {
        DisplayWizard<CreateMesh>("创建麦比乌斯带", "创建");
    }

    void OnWizardCreate()
    {
        // Step 1: 确定顶点位置
        List<Vector3> vList = new List<Vector3>();

        GameObject obj = new GameObject();  // 实例化要生成的目标对象
        obj.name = "麦比乌斯带";

        float angle = 2 * Mathf.PI / n;     // 计算单位弧度

        for (int i = 0; i < n; i++)
        {
            // 分别计算带子上中下三个顶点位置并保存到数组中
            Vector3 center = new Vector3(Mathf.Cos(angle * i), 0, Mathf.Sin(angle * i));
            Vector3[] vs = new Vector3[3] { center, center + h * Vector3.down, center + h * Vector3.up };

            // 计算中心点的角度，使之朝向坐标原点
            Quaternion rot = Quaternion.Euler(new Vector3(0, -90 - 360 / n * i, 0));

            // 生成顶点对象
            GameObject p0 = Instantiate(point, vs[0], rot) as GameObject;
            GameObject p1 = Instantiate(point, vs[1], Quaternion.identity) as GameObject;
            GameObject p2 = Instantiate(point, vs[2], Quaternion.identity) as GameObject;

            // 设置顶点名字（用于调试）
            p0.name = "c" + i;
            p1.name = 2 * i + "";
            p2.name = 2 * i + 1 + "";

            // 让上下边缘的点认中心点为父对象
            p0.transform.parent = obj.transform;
            p1.transform.parent = p0.transform;
            p2.transform.parent = p0.transform;

            // 通过旋转中心点，改变上下边缘顶点的位置
            p0.transform.Rotate(Vector3.right, 360 / n * i);

            // 将上下边缘顶点添加到顶点列表中
            vList.Add(p1.transform.position);
            vList.Add(p2.transform.position);

        }

        // Step 2: 生成顶点数组
        Vector3[] Vs = new Vector3[6 * n];
        int index = 0;

        for (int i = 0; i < 2 * n - 2; i += 2)
        {
            Vs[index++] = vList[i];
            Vs[index++] = vList[i + 1];
            Vs[index++] = vList[i + 2];
            Vs[index++] = vList[i + 2];
            Vs[index++] = vList[i + 1];
            Vs[index++] = vList[i + 3];
        }

        // 结合处顶点
        Vs[index++] = vList[2 * n - 2];
        Vs[index++] = vList[2 * n - 1];
        Vs[index++] = vList[0];
        Vs[index++] = vList[1];
        Vs[index++] = vList[0];
        Vs[index++] = vList[2 * n - 1];

        // Step 3: 缔造三角形索引
        int[] Ts = new int[Vs.Length];
        for (int i = 0; i < Vs.Length; i++)
        {
            Ts[i] = i;
        }

        // Step 4: 给目标对象添加网格组件
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        mesh.vertices = Vs;
        mesh.triangles = Ts;
        obj.GetComponent<MeshFilter>().mesh = mesh;

        // Step 5: 载入材质资源，添加给目标对象
        Material m = Resources.Load<Material>("M");
        obj.GetComponent<Renderer>().sharedMaterial = m;
    }

}
