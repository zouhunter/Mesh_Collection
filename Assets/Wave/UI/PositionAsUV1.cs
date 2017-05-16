using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class PositionAsUV1 : BaseMeshEffect
{
    protected PositionAsUV1() { }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        var verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);

        for (int i = 0; i < verts.Count; i++)
        {
            var vert = verts[i];
            vert.uv1 = new Vector2(vert.position.x, vert.position.y);
            verts[i] = vert;
        }
        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}