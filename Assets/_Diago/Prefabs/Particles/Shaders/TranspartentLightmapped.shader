Shader "Transparent/Diffuse-LM" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_TransparencyLM ("Transmissive Colour", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha

#pragma lighting LightCorrection exclude_path:prepass
inline half4 LightingLightCorrection (SurfaceOutput s, half3 lightDir, half atten)
{
	half NdotL = dot (s.Normal, lightDir);
    half4 c;
    c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
    c.a = s.Alpha;
    return c;
}


sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}