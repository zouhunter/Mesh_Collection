Shader "Custom/Mesh"  
{  
    Properties  
    {  
        _MainTex ("MainTex", 2D) = "white" {}  
        _MeshTex ("MeshTex", 2D) = "white" {}  

        _MinY ("MinY", Range(-20, 0)) = -20
		_MaxY ("MaxY", Range(-15, 0)) = -15

		_Speed ("Speed", Range(1, 10)) = 5  
    }  
    SubShader  
    {  
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType"="Transparent" }  
  
        Pass  
        {  
            ZWrite off  
            Blend SrcAlpha OneMinusSrcAlpha  
  
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
            #include "UnityCG.cginc"  
  
            struct appdata  
            {  
                float4 vertex : POSITION;  
                float2 uv : TEXCOORD0;  
            };  
  
            struct v2f  
            {  
                float4 vertex : SV_POSITION;  
                float2 uv : TEXCOORD0;  
            };  
  
            sampler2D _MainTex;  
            float4 _MainTex_ST;  
            sampler2D _MeshTex;  

			half _MinY;
			half _MaxY;
            half _Speed;  
  
            v2f vert (appdata v)  
            {  
                v2f o;  
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);  
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);  
  
                return o;  
            }  
              
            fixed4 frag (v2f i) : SV_Target  
            {  
                //最好选择世界空间坐标系下的顶点而非模型空间下的顶点  
                //因为后者会因为模型制作的规范不同而不同  
                half y = -i.vertex.y;  
				_MinY += _Time.y * _Speed;  
				_MaxY += _Time.y * _Speed;
				_MinY *= 50;//让_MinY更"负"
				_MaxY *= 50;//让_MaxY更"负"

				if(y > _MaxY)
				{
					return fixed4(0, 0, 0, 0); 
				}
                else if(y > _MinY)
                {  
					fixed4 col = tex2D(_MeshTex, i.uv);  
                    return col;       
                }  
				else 
				{
					fixed4 col = tex2D(_MainTex, i.uv);  
                    return col; 
				}
            }  
            ENDCG  
        }  
    }  
}  