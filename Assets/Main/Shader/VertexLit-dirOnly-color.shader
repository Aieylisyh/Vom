Shader "Mobile-Ex/ForwardBase dir only color"  {
	Properties{
		_Color("Main Color", COLOR) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
	}
	SubShader{
		Tags { "LightMode" = "ForwardBase" "PassFlags" = "OnlyDirectional" }
		Pass {
			Material {
				Diffuse[_Color]
				Ambient[_Color]
				Emission[_Emission]
			}
			Lighting On
		}
	}
	FallBack "Mobile/VertexLit"
}