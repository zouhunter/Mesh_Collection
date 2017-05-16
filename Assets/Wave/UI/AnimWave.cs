using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public class AnimWave : MaskableGraphic
{
    [Range(0.01f, 100)]
    public float speed = 1f;
    public int count = 30;
    public float emptyHeight = 10;
    [Range(0.01f,1f)]
    public float rangeHeight = 0.01f;
    public AnimationCurve waveCurve;
    private float scale = 0f;
    private Vector2 size;
    private Keyframe[] keyframes;
    private int timer = 1;
    private float waveRange;
    VertexHelper VH { get; set; }
    protected override void Awake()
    {
        base.Awake();
        keyframes = waveCurve.keys;
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        size = GetComponent<RectTransform>().rect.size;
        vh.Clear();
        scale = size.x / count;
        float randomheight = 0;
        for (int i = 0; i < count; i++)
        {
            var x1 = i * scale;
            var x2 = (i + 1) * scale;
            if (i % 5 == 0)
            {
                randomheight = UnityEngine.Random.Range(0, rangeHeight);
            }
            Vector2 pos1 = new Vector2(x1, 0);
            Vector2 pos2 = pos1 + Vector2.up * (GetCurveHeight(x1, Time.time * speed, i));
            Vector2 pos4 = new Vector2(x2, 0);
            Vector2 pos3 = pos4 + Vector2.up * (GetCurveHeight(x2, Time.time * speed, i));

            vh.AddUIVertexQuad(GetQuad(pos1, pos2, pos3, pos4));
        }

        VH = vh;
    }
    private void Update()
    {
        if (VH != null){
            this.SetAllDirty();
        }
        if (Time.time % timer < 0.02f)
        {
            waveRange = Random.Range(0,rangeHeight);
        }
    }
    private float GetCurveHeight(float normalized_x, float time,int index)
    {
        return waveCurve.Evaluate(normalized_x + time) + emptyHeight + waveRange * Mathf.Abs(count * 0.5f - index);
    }

    private UIVertex[] GetQuad(params Vector2[] vertPos)
    {
        UIVertex[] vs = new UIVertex[4];
        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(1, 1);
        for (int i = 0; i < 4; i++)
        {
            UIVertex v = UIVertex.simpleVert;
            v.color = color;
            v.position = vertPos[i];
            v.uv0 = uv[i];
            vs[i] = v;
        }
        return vs;
    }
}
