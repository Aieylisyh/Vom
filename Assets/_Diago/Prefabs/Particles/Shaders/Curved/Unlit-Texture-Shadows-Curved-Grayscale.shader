// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Curved/Unlit/With Shadows and Grayscale"
{
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _GSF("Grayscale intensity",  Range(0, 1)) = 0.25
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
            half   _GSF;

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
                half4 c = tex2D(_MainTex, i.uv);
              	c.rgb =  (c.r*.33 + c.g*.33 + c.b*.33) * _GSF;
                return c;
            }
            ENDCG
        }
    }
    FallBack "Hidden/VacuumShaders/Curved World/VertexLit/Diffuse"
}
