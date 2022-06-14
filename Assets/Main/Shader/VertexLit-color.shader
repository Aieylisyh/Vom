Shader "Mobile-Ex/VertexLit color"  {
	Properties{
		_Color("Main Color", COLOR) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
	}
		SubShader{
			Tags { "LightMode" = "Vertex" }
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