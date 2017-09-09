using System;
using UnityEngine;
using System.Collections;

public class ClipLine : MonoBehaviour
{
    public string matName;
    public float speed;
    private Material mat;
    private float timer;
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

        if (mat)
        {
            //mat = new Material(mat.shader);
            mat.SetInt("_Count", transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                var pos = transform.GetChild(i).localPosition;
                mat.SetVector("_Pos" + i, pos);
            }
        }


    }

    void Update()
    {
        timer += Time.deltaTime * speed;
        if (timer > 1){
            timer = 0;
        }
        mat.SetFloat("_Slider", timer);
}

}
