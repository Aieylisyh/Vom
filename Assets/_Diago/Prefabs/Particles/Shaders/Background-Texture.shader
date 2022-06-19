// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Background-Texture" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue"="Geometry+1" 
			"IgnoreProjector" = "True"
			"RenderType" = "Opaque"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite ON
			AlphaTest Off
			Offset -1, -1
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}

			half4 frag (v2f IN) : COLOR
			{
				return tex2D(_MainTex, IN.texcoord);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
