Shader "Custom/VertexColor" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		struct Input {
			float4 color : COLOR;
		};

		half _Glossiness;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = IN.color.rgb;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
}
