Shader "Particles/Alpha Blended Premultiply Separate Channel" {
Properties {
	_MainTex ("Particle Texture (RGB)", 2D) = "white" {}
	_AlphaTex ("Alpha Texture (A)", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend One OneMinusSrcAlpha 
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Mode Off }
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}

	SubShader {
		Pass {
			SetTexture [_AlphaTex] {
				combine primary * primary alpha
			}
			SetTexture [_MainTex] {
				combine previous * texture, previous
			}
			SetTexture [_AlphaTex] {
				combine previous, previous * texture alpha
			}
		}
	}
}
}
