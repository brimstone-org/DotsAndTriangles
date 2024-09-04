Shader "Learning/Stencil/StencilUnlitNotEqualOne"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_StencilRef("Stencil Ref", float) = 1
	}
	SubShader
	{
		Tags {  "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
		

		Stencil
		{
			Ref [_StencilRef]
			Comp notequal
			Pass keep
		}

		Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;

			};

			struct v2f
			{				
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col * i.color;
			}
			ENDCG
		}
	}
}
