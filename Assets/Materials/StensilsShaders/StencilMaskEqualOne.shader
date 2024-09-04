Shader "Learning/Stencil/StencilMaskEqualOne"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_StencilRef("Stencil Ref", float) = 1
		[MaterialToggle] _UseUIColor ("Use UI Color", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+1"}
		LOD 100
		
		Blend SrcAlpha OneMinusSrcAlpha
		Stencil
		{
			Ref 1
			Comp always
			Pass Replace
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _UseUIColor;

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
				fixed4 color = tex2D(_MainTex, i.uv) * i.color;
				if (color.a < 0.1) discard; 
				return color * _UseUIColor;
			}
			ENDCG
		}
	}
}
