// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Curved/Unlit/With Shadows"
{
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
    }
        SubShader{
        Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

        Pass{
            Tags{ "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #pragma fragmentoption ARB_fog_exp2
            #pragma fragmentoption ARB_precision_hint_fastest

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Assets/VacuumShaders/Curved World/Shaders/cginc/CurvedWorld_Base.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                LIGHTING_COORDS(1, 2)
                UNITY_FOG_COORDS(3)
            };

            float4 _MainTex_ST;
            fixed4 _ShadowColor;

            v2f vert(appdata_tan v)
            {
                v2f o;
                //CurvedWorld vertex transform
                V_CW_TransformPoint(v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f i) : COLOR
            {
                fixed atten = LIGHT_ATTENUATION(i); // Light attenuation + shadows.
                fixed4 c = lerp(_ShadowColor, (1, 1, 1, 1), atten);
                //fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.
                c *= tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }

        Pass{
            Tags{ "LightMode" = "ForwardAdd" }
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdadd_fullshadows
            #pragma fragmentoption ARB_fog_exp2
            #pragma fragmentoption ARB_precision_hint_fastest

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Assets/VacuumShaders/Curved World/Shaders/cginc/CurvedWorld_Base.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                LIGHTING_COORDS(1, 2)
                UNITY_FOG_COORDS(3)
            };

            float4 _MainTex_ST;

            v2f vert(appdata_tan v)
            {
                v2f o;
                //CurvedWorld vertex transform
                V_CW_TransformPoint(v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f i) : COLOR
            {
                fixed atten = LIGHT_ATTENUATION(i); // Light attenuation + shadows.
                                                    //fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.
                fixed4 c = tex2D(_MainTex, i.uv) * atten;
                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }
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
    }
    FallBack "Hidden/VacuumShaders/Curved World/VertexLit/Diffuse"
}
