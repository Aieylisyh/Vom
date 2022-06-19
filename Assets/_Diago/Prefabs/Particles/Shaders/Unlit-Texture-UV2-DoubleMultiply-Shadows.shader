// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Texture-UV2-Double-Multiply-Shadows" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_SecondTex("Detail (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

		Pass{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase
#pragma fragmentoption ARB_fog_exp2
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"
#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex    : POSITION;
				float2 texcoord  : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : POSITION;
				float2 uv  : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				LIGHTING_COORDS(2, 3)
			};

			float4 _MainTex_ST;
			float4 _SecondTex_ST;

			v2f vert(appdata v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
				o.uv2 = TRANSFORM_TEX(v.texcoord1, _SecondTex).xy;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _SecondTex;

			fixed4 frag(v2f i) : COLOR
			{
				fixed atten = SHADOW_ATTENUATION(i);
				return tex2D(_MainTex, i.uv) * tex2D(_SecondTex, i.uv2) * 2 * atten;
			}
				ENDCG
		}		
	}
	FallBack "VertexLit"
}
