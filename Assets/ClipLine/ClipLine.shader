﻿Shader "Unlit/ClipLine"
{
	Properties
	{
		_Color("Color",Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	    _Round("Round",Float) = 0
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
	{
		Cull Back
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		bool IsInsidePosList(float4 vec);
#include "UnityCG.cginc"
	sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _Color;
	float _Round;
	float _Slider;
	float _Slider_last;
	int _Count;

	uniform vector _Pos[20];

	int _Index;//区域
	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float4 op :TEXCOORD1;
	};


	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.op = v.vertex;// mul(_Object2World, v.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv);
    	if (!IsInsidePosList(i.op)){
			clip(-1);
		}
	    return col * _Color;
	}

	///在指定的区域内
	bool IsInsidePosList(float4 vec)
	{
		if (_Count == 0) return true;
		for (int i = 0; i < _Count -1; i++)
		{
			float3 dir1 = vec.xyz - _Pos[i + 1].xyz;
			float3 dir2 = vec.xyz - _Pos[i].xyz;
			float3 dirtemp = _Pos[i].xyz - _Pos[i + 1].xyz;

			float dot1 = dot(dir1, dirtemp);
			float dot2 = dot(dir2,-dirtemp);

			if (dot1 * dot2 > 0)
			{
				float angle = acos(dot1 / (length(vec.xyz - _Pos[i + 1].xyz) * length(_Pos[i].xyz - _Pos[i + 1].xyz)));
				float dis = length(vec.xyz - _Pos[i + 1].xyz)*sin(angle);
				if (dis < _Round)
				{
					return true;
				}
			}
		}
		return false;
	}

	ENDCG
	}
	}
}
