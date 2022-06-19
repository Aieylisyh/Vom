Shader "Transparent/Toon-Lighted-Rim" {
	Properties {
		_Alpha("Alpha", Float) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Light Ramp (RGB)", 2D) = "gray" {} 
		_RimRamp("Rim Ramp (RGB)", 2D) = "gray" {}
		
	}

	SubShader {
		Tags{ "Queue" = "Transparent+5" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha 
		
CGPROGRAM

sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma surface surf ToonRamp exclude_path:prepass  alpha
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir) * 0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
	c.a = s.Alpha;
	return c;
}


sampler2D _MainTex;
sampler2D _RimRamp;
float _Alpha;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
	float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	half4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = c.a * _Alpha;
	half rim = 1 - dot(IN.viewDir, WorldNormalVector(IN, o.Normal));
	o.Emission = tex2D(_RimRamp, float2(rim, rim)).rgb;
}
ENDCG

	} 

	Fallback "Diffuse"
}
