Shader "Custom/ProgressBarReplaceColor"
{
	Properties
	{
		[PerRendererData]
		_MainTex ("Texture", 2D) = "white" {}
		_Progress ("Progress", Range(0,1)) = 0
		_ProgressColor ("ProgressColor", Color ) = (1,0,0,1)
		_ProgressStart( "ProgressStart", Range(0,1)) = 0
		_ProgressEnd("_ProgressEnd", Range(0.0, 1.0)) = 1.0
		_ColorToReplace("ColorToReplace", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent"  }
		Cull Off ZWrite Off ZTest Always

		Blend SrcAlpha OneMinusSrcAlpha

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed _Progress;
			fixed _ProgressStart;
			fixed _ProgressEnd;
			fixed4 _ProgressColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed progress = _ProgressStart + _Progress * (1 - _ProgressStart);

				fixed progressStartVisibility = max(0, sign(i.uv.x - _ProgressStart));
				fixed progressEndVisibility = max(0, sign(_ProgressEnd - i.uv.x));
				fixed fillVisibility = max(0, sign(progress - i.uv.x));
				fixed4 col = tex2D(_MainTex, i.uv);
				
				col = lerp(col, _ProgressColor, fillVisibility * progressStartVisibility * progressEndVisibility);
				
				return col;
			}
			ENDCG
		}
	}
}
