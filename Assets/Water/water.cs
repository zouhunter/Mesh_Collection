using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{

    Mesh mesh;

    public int tier = 10;           //长度分段  
    private float length = 10;      //长  
    private int width = 3;          //宽  
    private int hight = 10;         //高  

    private Vector3[] vs;           //顶点坐标  
    private int[] ts;               //顶点序列  
    private Vector2[] newUVs;       //UV贴图  
    private Vector3[] newNormals;   //法线  

    void Update()
    {

        int temp = ((tier + 1) * 8 + 4) * 3;    //确定顶点数量  

        vs = new Vector3[temp];
        ts = new int[temp];
        newUVs = new Vector2[temp];
        newNormals = new Vector3[temp];

        float dis = 2 * Mathf.PI / tier;        //两段之差的横坐标  

        int count = 0;
        for (int i = 0; i < tier; i++)
        {

            float pos1 = i * length / tier - length / 2;
            float pos2 = (i + 1) * length / tier - length / 2;
            //顶面顶点坐标  
            vs[count] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), width);
            vs[count + 1] = new Vector3(pos2, Mathf.Sin(Time.time + (i + 1) * dis), -width);
            vs[count + 2] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), -width);

            vs[count + 3] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), width);
            vs[count + 4] = new Vector3(pos2, Mathf.Sin(Time.time + (i + 1) * dis), width);
            vs[count + 5] = new Vector3(pos2, Mathf.Sin(Time.time + (i + 1) * dis), -width);
            //顶面法线  
            newNormals[count] = Vector3.Normalize(new Vector3(1, Mathf.Cos(Time.time + i * dis), 0));
            newNormals[count + 1] = Vector3.Normalize(new Vector3(1, Mathf.Cos(Time.time + (i + 1) * dis), 0));
            newNormals[count + 2] = Vector3.Normalize(new Vector3(1, Mathf.Cos(Time.time + i * dis), 0));

            newNormals[count + 3] = Vector3.Normalize(new Vector3(1, Mathf.Cos(Time.time + i * dis), 0));
            newNormals[count + 4] = Vector3.Normalize(new Vector3(1, Mathf.Cos(Time.time + (i + 1) * dis), 0));
            newNormals[count + 5] = Vector3.Normalize(new Vector3(1, Mathf.Cos(Time.time + (i + 1) * dis), 0));

            //前面顶点坐标  
            vs[count + 6] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), -width);
            vs[count + 7] = new Vector3(pos2, -hight, -width);
            vs[count + 8] = new Vector3(pos1, -hight, -width);

            vs[count + 9] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), -width);
            vs[count + 10] = new Vector3(pos2, Mathf.Sin(Time.time + (i + 1) * dis), -width);
            vs[count + 11] = new Vector3(pos2, -hight, -width);
            //前面法线  
            for (int j = 0; j < 6; j++)
            {
                newNormals[count + 6 + j] = Vector3.back;
            }
            //后面顶点坐标  
            vs[count + 12] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), width);
            vs[count + 13] = new Vector3(pos1, -hight, width);
            vs[count + 14] = new Vector3(pos2, -hight, width);

            vs[count + 15] = new Vector3(pos1, Mathf.Sin(Time.time + i * dis), width);
            vs[count + 16] = new Vector3(pos2, -hight, width);
            vs[count + 17] = new Vector3(pos2, Mathf.Sin(Time.time + (i + 1) * dis), width);
            //后面法线  
            for (int j = 0; j < 6; j++)
            {
                newNormals[count + 12 + j] = Vector3.forward;
            }
            //下面顶点坐标  
            vs[count + 18] = new Vector3(pos1, -hight, width);
            vs[count + 19] = new Vector3(pos1, -hight, -width);
            vs[count + 20] = new Vector3(pos2, -hight, -width);

            vs[count + 21] = new Vector3(pos1, -hight, width);
            vs[count + 22] = new Vector3(pos2, -hight, -width);
            vs[count + 23] = new Vector3(pos2, -hight, width);
            //下面法线  
            for (int j = 0; j < 6; j++)
            {
                newNormals[count + 18 + j] = Vector3.down;
            }

            count += 24;
        }

        //两侧顶点坐标及法线  
        vs[vs.Length - 12] = new Vector3(-length / 2, Mathf.Sin(Time.time), width);
        vs[vs.Length - 11] = new Vector3(-length / 2, -hight, -width);
        vs[vs.Length - 10] = new Vector3(-length / 2, -hight, width);

        vs[vs.Length - 9] = new Vector3(-length / 2, Mathf.Sin(Time.time), width);
        vs[vs.Length - 8] = new Vector3(-length / 2, Mathf.Sin(Time.time), -width);
        vs[vs.Length - 7] = new Vector3(-length / 2, -hight, -width);

        for (int j = 0; j < 6; j++)
        {
            newNormals[vs.Length - 12 + j] = Vector3.left;
        }

        vs[vs.Length - 6] = new Vector3(length / 2, Mathf.Sin(Time.time + tier * dis), width);
        vs[vs.Length - 5] = new Vector3(length / 2, -hight, width);
        vs[vs.Length - 4] = new Vector3(length / 2, -hight, -width);

        vs[vs.Length - 3] = new Vector3(length / 2, Mathf.Sin(Time.time + tier * dis), width);
        vs[vs.Length - 2] = new Vector3(length / 2, -hight, -width);
        vs[vs.Length - 1] = new Vector3(length / 2, Mathf.Sin(Time.time + tier * dis), -width);

        for (int j = 0; j < 6; j++)
        {
            newNormals[vs.Length - 6 + j] = Vector3.right;
        }

        for (int i = 0; i < ts.Length; i++)
        {    //顶点序列赋值  
            ts[i] = i;
        }

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vs;
        mesh.uv = newUVs;
        mesh.triangles = ts;
        mesh.normals = newNormals;
    }
}