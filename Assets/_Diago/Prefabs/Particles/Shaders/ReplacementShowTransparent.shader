Shader "Replacement/ReplacementShowTransparent"
{
	Properties
	{
	}

	CGINCLUDE
#include "UnityCG.cginc"
		struct Appdata
	{
		float4 vertex : POSITION;
	};

	struct V2F
	{
		float4  pos : SV_POSITION;
	};

	V2F vert(Appdata v)
	{
		V2F o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}

	fixed4 frag(V2F i) : COLOR
	{
		return fixed4(1,0,0,0.1);
	}

		ENDCG

		SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

			Lighting Off Fog{ Mode Off }
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest	
			ENDCG
		}
	}
}
