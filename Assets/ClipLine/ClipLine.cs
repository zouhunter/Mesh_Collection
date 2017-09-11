using System;
using UnityEngine;
using System.Collections;

public class ClipLine : MonoBehaviour
{
    public string matName;
    public float speed = 0.2f;
    public bool once = true;
    public float span = 0.01f;

    private Material mat;
    private float lastdistence;
    private float distenceAll;
    private float distence;
    private Vector4[] posList;
    private float[] distenceList;
    private int count;

    void Start()
    {
        Renderer render = GetComponent<Renderer>();
        if (string.IsNullOrEmpty(matName))
        {
            mat = render.material;
        }
        else
        {
            mat = Array.Find<Material>(render.materials, x => x.name == matName);
        }

        if (!mat || transform.childCount < 2)
        {
            this.enabled = false;
        }
        else
        {
            distenceList = new float[transform.childCount - 1];
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i > 0)
                {
                    distenceList[i - 1] = Vector3.Distance(transform.GetChild(i - 1).position, transform.GetChild(i).position);
                    distenceAll += distenceList[i - 1];
                }
            }
        }
    }

    void Update()
    {
        distence += Time.deltaTime * speed;
        if (distence > distenceAll)
        {
            distence = 0;
            if (once)
            {
                mat.SetInt("_Count", 0);
                enabled = false;
                return;
            }
        }

        if (Mathf.Abs(distence - lastdistence) > 0.01f)
        {
            CalcutPoistionList();
            OnPositionChanged();
            lastdistence = distence;
        }

    }

    void CalcutPoistionList()
    {
        float distenceTemp = 0f;
        count = 1;
        for (int i = 0; i < distenceList.Length; i++)
        {
            count++;

            distenceTemp += distenceList[i];

            if (distenceTemp > distence)
            {
                distenceTemp -= distenceList[i];
                    break;
            }
        }
        posList = new Vector4[20];
        int index = 0;
        for (; index < count - 1; index++)
        {
            posList[index] = transform.GetChild(index).localPosition;
            posList[index].w = 1;
        }
        posList[index] = Vector4.Lerp(transform.GetChild(index - 1).localPosition, transform.GetChild(index).localPosition,
            (distence - distenceTemp) / distenceList[index - 1]);
    }
    void OnPositionChanged()
    {
        mat.SetInt("_Count", count);
#if UNITY_5_6_OR_NEWER
        mat.SetVectorArray("_Pos", posList);
#elif UNITY_5_3_OR_NEWER
        for (int i = 0; i < posList.Length; i++)
        {
            mat.SetVector("_Pos" + i, posList[i]);
        }
#endif


    }
}
