Shader "Custom/FogOfWar" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BlurPower("BlurPower", float) = 0.002
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting off
		LOD 200

		CGPROGRAM
#pragma target 3.0
#pragma surface surf Lambert alpha

		fixed4 _Color;
		sampler2D _MainTex;
		float _BlurPower;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		//o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
		///*
		half4 baseColor1 = tex2D(_MainTex, IN.uv_MainTex + float2(-_BlurPower, 0));
		half4 baseColor2 = tex2D(_MainTex, IN.uv_MainTex + float2(0, -_BlurPower));
		half4 baseColor3 = tex2D(_MainTex, IN.uv_MainTex + float2(-_BlurPower, 0));
		half4 baseColor4 = tex2D(_MainTex, IN.uv_MainTex + float2(0, -_BlurPower));
		half4 baseColor = 0.25 * (baseColor1 + baseColor2 + baseColor3 + baseColor4);

		o.Alpha = 0.9f - baseColor.g;
		//*/



		//o.Alpha = 0.9f - tex2D(_MainTex, IN.uv_MainTex).g;
	}

	ENDCG
	}

		Fallback "Diffuse"
}
