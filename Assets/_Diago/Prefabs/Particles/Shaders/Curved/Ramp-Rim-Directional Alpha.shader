Shader "Curved/Ramp-Rim-Directional-With-Alpha" {
    Properties{
		_Alpha("Alpha", Range(0, 1)) = 1
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Ramp("Light Ramp (RGB)", 2D) = "gray" {}
        _RimRamp("Rim Ramp (RGB)", 2D) = "gray" {}
        _RimF("Rim intensity",  Range(0, .5)) = 0.5
    }

    SubShader{
        Tags{ "RenderType" = "CurvedWorld_Opaque" }
        LOD 200

        Pass
        {
            Tags {"LightMode"="ShadowCaster"}


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
            #include "Assets/VacuumShaders/Curved World/Shaders/cginc/CurvedWorld_Base.cginc"

            struct v2f { 
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                V_CW_TransformPoint(v.vertex);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        CGPROGRAM

        //CurvedWorld shader API 
        #include "Assets/VacuumShaders/Curved World/Shaders/cginc/CurvedWorld_Base.cginc"

        sampler2D _Ramp;
        half	  _RimF;
		half	  _Alpha;

        // custom lighting function that uses a texture ramp based
        // on angle between light direction and normal
        #pragma surface surf ToonRamp vertex:vert lighting ToonRamp alpha


        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float3 viewDir;
        };

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            //CurvedWorld vertex transform
            V_CW_TransformPointAndNormal(v.vertex, v.normal, v.tangent);
        }

        inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
        {
            #ifndef USING_DIRECTIONAL_LIGHT
            lightDir = normalize(lightDir);
            #endif

            half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
            half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
            c.a = _Alpha;
            return c;
        }


        sampler2D _MainTex;
        sampler2D _RimRamp;

        

        void surf(Input IN, inout SurfaceOutput o) {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a * _Alpha;
            half rim = 1 - dot(IN.viewDir, WorldNormalVector(IN, o.Normal));
            rim *= dot(o.Normal, float3(0, 1, 0)) * _RimF + (1 - _RimF);
            o.Emission = tex2D(_RimRamp, float2(rim, rim)).rgb;
        }
        ENDCG
    }

    Fallback "Hidden/VacuumShaders/Curved World/VertexLit/Diffuse"
}
