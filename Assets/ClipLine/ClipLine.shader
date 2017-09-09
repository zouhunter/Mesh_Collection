Shader "Unlit/ClipLine"
{
	Properties
	{
		_Slider("Slider",Range(0,1)) = 0
		_Color("Color",Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Round("Round",Float) = 0
		_Count("Count",Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
			    Cull Back
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
                void ClipNotComplete(float4 vec);
		        void UpdatePosition();
		        int GetPosRegion(float4 vec);
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
				vector _CurrPos;//当前坐标
				vector startPos;
				vector endPos;
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
				    ClipNotComplete(i.op);
					return col * _Color;
				}

				//判断是否已经过线
				void ClipNotComplete(float4 vec)
				{
					if (_Count == 0) return;
					UpdatePosition();
					int id = GetPosRegion(vec);
					if (id > _Index)
					{
						clip(-1);
					}
					else if(id == _Index) {
						float3 dir = _CurrPos - vec;
						float c = dot(dir, endPos - startPos);
						clip(c-0.01);
					}
				}

				///指定当前点的区域
				int GetPosRegion(float4 vec)
				{
					for (int i = 0; i < _Count - 1; i++)
					{
						float3 dir1 = vec.xyz - _Pos[i + 1].xyz;
						float3 dir2 = vec.xyz - _Pos[i].xyz;
						float3 dirtemp = _Pos[i].xyz - _Pos[i + 1].xyz;

						float dot1 = dot(dir1, dirtemp);
						float dot2 = dot(dir2,- dirtemp);

						if (dot1 * dot2 > 0)
						{
							float angle = acos(dot1 / (length(vec.xyz - _Pos[i + 1].xyz) * length(_Pos[i].xyz - _Pos[i + 1].xyz)));
							float dis = length(vec.xyz - _Pos[i + 1].xyz)*sin(angle);
							if (dis < _Round)
							{
								return i;
							}
						}
					}
					return _Count;
				}

				//更新当前坐标及索引
				void UpdatePosition()
				{
					if (abs(_Slider - _Slider_last) > 0.01f) {
						if (_Count > 0 && _Index < _Count - 1)
						{
							float val = _Slider * _Count;
							_Index = floor(val);
							startPos = _Pos[_Index];
							endPos = _Pos[_Index + 1];
							_CurrPos = lerp(startPos, endPos, val - _Index);
						}
						_Slider_last = _Slider;
					}
				}


				ENDCG
			}
		}
}
