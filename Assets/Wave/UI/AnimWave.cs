using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public class AnimWave : MaskableGraphic
{
    [Range(0.01f, 1)]
    public float updatetime = 1f;
    private float updatetimer;
    public int scaler = 30;
    public float height = 5;
    public float defultHight = 10;
    public bool toleft;
    [Range(0.01f, 10f)]
    public float rangewave = 0.01f;
    public AnimationCurve waveCurve;
    private float scale = 0f;
    private Vector2 size;
    private float rangewaveTimer;
    private bool rising;
    private float maxAnim;
    VertexHelper VH { get; set; }
    private List<Vector2[]> posList = new List<Vector2[]>();
    private float _index;
    protected override void Awake()
    {
        base.Awake();
        VH = null;
        maxAnim = waveCurve.keys[waveCurve.length - 1].time;
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        size = GetComponent<RectTransform>().rect.size;
        scale = size.x / scaler;
        vh.Clear();

        if (VH == null)
        {
            if (toleft)
            {
                for (int i = 0; i <= scaler; i++)
                {
                    _index += scale;

                    Vector2 pos1 = new Vector2(_index, 0);
                    Vector2 pos2 = pos1 + Vector2.up * (GetCurveHeight(_index));
                    var node = new Vector2[] { pos1, pos2 };
                    posList.Add(node);
                }
            }
            else
            {
                for (int i = 0; i <= scaler; i++)
                {
                    _index = i * scale;

                    Vector2 pos1 = new Vector2(_index, 0);
                    Vector2 pos2 = pos1 + Vector2.up * (GetCurveHeight((scaler - i) * scale));
                    var node = new Vector2[] { pos1, pos2 };
                    posList.Add(node);
                }
            }

        }
        else
        {
            if (toleft)
            {
                for (int i = 0; i <= scaler; i++)
                {
                    if (i != scaler)
                    {
                        posList[i][1].y = posList[i + 1][1].y;
                    }
                    else
                    {
                        _index += scale;
                        posList[i][1].y = GetCurveHeight(_index);
                    }
                }
            }

            else
            {
                for (int i = scaler; i >= 0; i--)
                {
                    if (i != 0)
                    {
                        posList[i][1].y = posList[i - 1][1].y;
                    }
                    else
                    {
                        _index += scale;
                        posList[0][1].y = GetCurveHeight(_index);
                    }
                }
            }

        }

        for (int i = 0; i < scaler; i++)
        {
            vh.AddUIVertexQuad(GetQuad(posList[i][0], posList[i][1], posList[i + 1][1], posList[i + 1][0]));
        }
        VH = vh;
    }
    private void Update()
    {
        updatetimer += Time.deltaTime;

        if (VH != null && updatetimer > updatetime)
        {
            updatetimer = 0;
            this.SetAllDirty();
        }

    }
    private float GetCurveHeight(float normalized_x)
    {
        if (rangewaveTimer > rangewave)
        {
            rising = false;
        }
        if (rangewaveTimer < -rangewave)
        {
            rising = true;
        }
        if (rising)
        {
            rangewaveTimer += Time.deltaTime;
        }
        else
        {
            rangewaveTimer -= Time.deltaTime;
        }
        return waveCurve.Evaluate(normalized_x) * height + defultHight + rangewaveTimer;
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
