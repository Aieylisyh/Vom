// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Color-Transparent"
{
	Properties{
		_Color("Main Color (A=Opacity)", Color) = (1,1,1,1)
	}
		SubShader{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		LOD 100

		Pass{
		Cull Off
		Lighting Off
		ZWrite Off
		AlphaTest Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
		};

		fixed4 _Color;

		v2f vert(appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			return o;
		}

		fixed4 frag(v2f i) : COLOR
		{
			fixed4 col = _Color;
			return col;
		}
			ENDCG
		}
	}

}
