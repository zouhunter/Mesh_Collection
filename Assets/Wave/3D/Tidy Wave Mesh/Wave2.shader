Shader "JQM/#Name#"
{
	//属性快
	Properties
	{
		_Color("Base Color",Color) = (1,1,1,1)
	}


		SubShader
	{

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct VertexOutput
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;

	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	float4 _Color;

	//顶点着色器
	VertexOutput vert(appdata_full v)
	{
		VertexOutput o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	//像素着色器
	fixed4 frag(VertexOutput i) : COLOR
	{

		float2 uv = i.uv;
		uv = uv*2.0 - 1.0;
		float wave_width;

		float3 color1 = float3(0,0,0);

		for (float i = 0.0; i<10.00; i++)
		{
			uv.y += (0.07 * sin(uv.x + i / 7.0 + _Time.y));
			wave_width = abs(1.0 / (150.0 * uv.y));
			color1.xyz += fixed3(wave_width, wave_width, wave_width);
		}


		return float4(color1.xyz,1)*_Color;
	}
		ENDCG
	}
	}
}