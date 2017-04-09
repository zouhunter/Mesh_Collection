Shader "Custom/Grid" {
	Properties{ 
		_BackgroundColor("BackgroundColor", Color) = (1, 1, 1, 1) 
		_BackgroundColor2("BackgroundColor2", Color) = (0, 0, 0, 1) 
		_Space("Space", Range(0, 1)) = 0.2
		_XOffset("XOffset", Range(-1, 1)) = 0.15
		_YOffset("YOffset", Range(-1, 1)) = 0.05 
	}
	SubShader{ 
		Pass{ 
		CGPROGRAM 
#pragma vertex vert 
#pragma fragment frag 
#include "UnityCG.cginc" 
		struct appdata { 
		float4 vertex : POSITION; 
		float2 uv : TEXCOORD0;
	}; 
	struct v2f
	{ 
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0; 
	}; //格子背景 
	fixed4 _BackgroundColor;
	fixed4 _BackgroundColor2; 
	fixed _Space;
	fixed _XOffset;
	fixed _YOffset; 
	v2f vert (appdata v) {
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.uv;
		return o; 
	} 
	fixed4 frag (v2f i) : SV_Target 
	{ 
		//fmod(x, y)：x/y的余数，和x有同样的符号 
		//step(a, x)：如果x=a，返回1																																																																																																																											//得到一个小于_Space的余数，即a的范围为[0, _Space)
fixed a = fmod(i.uv.x + _XOffset, _Space);
//有1/2概率返回0，有1/2概率返回1，从而形成间隔效果
a = step(0.5 * _Space, a);

fixed b = fmod(i.uv.y + _YOffset, _Space);
b = step(0.5 * _Space, b);

return _BackgroundColor * a * b + _BackgroundColor2 * (1 - a * b);
}
ENDCG
}
}
}