Shader "Transparent/Diffuse-Wrapped" {
    Properties {
        _Alpha ("Alpha", Float) = 1
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Desaturate ("Desaturate", Float) = 0
    }

    SubShader {
        Tags {"Queue"="Transparent+5" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Cull back
        Zwrite On
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass {
                ZWrite On
                ColorMask 0
         }
        
        LOD 200

    CGPROGRAM	
    #pragma surface surf SimpleLambert alpha noforwardadd 
    
    sampler2D _MainTex;
    float _Lightness;
    float _Alpha;
    float _Desaturate;
  
    half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
        fixed diff = max (0, dot (s.Normal, lightDir));
        diff = diff * 0.66 + 0.33;
        fixed4 c;
        c.rgb = saturate(s.Albedo * _LightColor0.rgb * diff * atten  * (1 - _Desaturate));
        c.a = s.Alpha;
        return c;
    }	

    struct Input {
        float2 uv_MainTex;
    };

    void surf (Input IN, inout SurfaceOutput o) {
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
        o.Alpha = c.a * _Alpha;
        if (_Desaturate > 0){
            c.rgb = dot(c.rgb, fixed3(.222, .707, .071));
        }
        
        o.Albedo = c;
    }
    ENDCG
    }

    Fallback "Transparent/VertexLit"
}
