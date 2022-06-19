// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Unlit-Transparent-DepthPass"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
		LOD 200

		Cull Off
		Lighting Off
		Offset -1, -1
		ZWrite Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			ZWrite On
			ColorMask 0

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return half4 (0, 0, 0, 0);
			}
			ENDCG
		}

		Pass
		{			

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				return tex2D(_MainTex, i.texcoord);
			}
		ENDCG
	}
	}
}
