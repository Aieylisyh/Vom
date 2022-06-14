Shader "Mobile-Ex/diffuse color"  {
	Properties{
		_Color("Main Color", COLOR) = (1,1,1,1)
	}
		SubShader{
			Tags { "LightMode" = "Vertex" }
			Pass {
				Material {
					Diffuse[_Color]
					Ambient[_Color]
				}
				Lighting On
			}
	}
		FallBack "Mobile/Diffuse"
}