// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/GradientMap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_GradientMap("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
		#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
		#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _GradientMap;

			fixed4 frag(v2f IN) : COLOR
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, IN.texcoord);
				float lum = dot(col.rgb, fixed3(.222, .707, .071));
				fixed3 mapped = tex2D(_GradientMap, fixed2(lum, lum)).rgb;
				return fixed4(mapped.rgb, col.a) * IN.color;
			}
			ENDCG
		}
	}

	Fallback "Sprites/Default"
}
