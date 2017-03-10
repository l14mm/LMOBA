Shader "Custom/FogOfWar" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BlurPower("BlurPower", float) = 0.002
		_Lambda("Lambda", int) = 1
	}
		SubShader{
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting off
		LOD 200

		CGPROGRAM
#pragma target 3.0
#pragma surface surf Lambert alpha

		fixed4 _Color;
		sampler2D _MainTex;
		float _BlurPower;
		int _Lambda;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		//o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
		/*
		half4 baseColor1 = tex2D(_MainTex, IN.uv_MainTex + float2(-_BlurPower, 0));
		half4 baseColor2 = tex2D(_MainTex, IN.uv_MainTex + float2(0, -_BlurPower));
		half4 baseColor3 = tex2D(_MainTex, IN.uv_MainTex + float2(-_BlurPower, 0));
		half4 baseColor4 = tex2D(_MainTex, IN.uv_MainTex + float2(0, -_BlurPower));
		half4 baseColor = 0.25 * (baseColor1 + baseColor2 + baseColor3 + baseColor4);

		//o.Alpha = 0.9f - baseColor.g;
		o.Alpha = 0.9f - tex2D(_MainTex, IN.uv_MainTex).g;
		*/
		float kernel1[81] =
		{
			0,	0,			0,			0,			0,			0,			0,			0,			0,
			0,	0.005084,	0.009377,	0.013539,	0.015302,	0.013539,	0.009377,	0.005084,	0,
			0,	0.009377,	0.017296,	0.024972,	0.028224,	0.024972,	0.017296,	0.009377,	0,
			0,	0.013539,	0.024972,	0.036054,	0.040749,	0.036054,	0.024972,	0.013539,	0,
			0,	0.015302,	0.028224,	0.040749,	0.046056,	0.040749,	0.028224,	0.015302,	0,
			0,	0.013539,	0.024972,	0.036054,	0.040749,	0.036054,	0.024972,	0.013539,	0,
			0,	0.009377,	0.017296,	0.024972,	0.028224,	0.024972,	0.017296,	0.009377,	0,
			0,	0.005084,	0.009377,	0.013539,	0.015302,	0.013539,	0.009377,	0.005084,	0,
			0,	0,			0,			0,			0,			0,			0,			0		,	0

		};
		float kernel2[81] = 
		{
			0		,	0.000001,	0.000014,	0.000055,	0.000088,	0.000055,	0.000014,	0.000001,	0		,
			0.000001,	0.000036,	0.000362,	0.001445,	0.002289,	0.001445,	0.000362,	0.000036,	0.000001,
			0.000014,	0.000362,	0.003672,	0.014648,	0.023205,	0.014648,	0.003672,	0.000362,	0.000014,
			0.000055,	0.001445,	0.014648,	0.058434,	0.092566,	0.058434,	0.014648,	0.001445,	0.000055,
			0.000088,	0.002289,	0.023205,	0.092566,	0.146634,	0.092566,	0.023205,	0.002289,	0.000088,
			0.000055,	0.001445,	0.014648,	0.058434,	0.092566,	0.058434,	0.014648,	0.001445,	0.000055,
			0.000014,	0.000362,	0.003672,	0.014648,	0.023205,	0.014648,	0.003672,	0.000362,	0.000014,
			0.000001,	0.000036,	0.000362,	0.001445,	0.002289,	0.001445,	0.000362,	0.000036,	0.000001,
			0		,	0.000001,	0.000014,	0.000055,	0.000088,	0.000055,	0.000014,	0.000001,	0
		};
		float kernel[81];
		if (_Lambda == 1)
			kernel = kernel1;
		else if (_Lambda == 2)
			kernel = kernel2;


		// Gaussian blur
		///*
		o.Alpha = 0.9f -
			(

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower *-4)) * kernel[0]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-4)) * kernel[1]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-4)) * kernel[2]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-4)) *	kernel[3]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-4)) * kernel[4]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-4)) * kernel[5]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-4)) * kernel[6]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-4)) * kernel[7]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower *-4)) * kernel[8]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower *-3)) * kernel[9]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-3)) * kernel[10]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-3)) * kernel[11]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-3)) *	kernel[12]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-3)) * kernel[13]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-3)) * kernel[14]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-3)) * kernel[15]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-3)) * kernel[16]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower *-3)) * kernel[17]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower *-2)) * kernel[18]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-2)) * kernel[19]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-2)) * kernel[20]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-2)) *	kernel[21]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-2)) * kernel[22]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-2)) * kernel[23]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-2)) * kernel[24]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-2)) * kernel[25]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower *-2)) * kernel[26]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower *-1)) * kernel[27]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-1)) * kernel[28]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-1)) * kernel[29]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-1)) *	kernel[30]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-1)) * kernel[31]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-1)) * kernel[32]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-1)) * kernel[33]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-1)) * kernel[34]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower *-1)) * kernel[35]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower * 0)) *	kernel[36]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 0)) *	kernel[37]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 0)) *	kernel[38]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 0)) *	kernel[39]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 0)) * kernel[40]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 0)) * kernel[41]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 0)) * kernel[42]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 0)) * kernel[43]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower * 0)) * kernel[44]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower * 1)) * kernel[45]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 1)) * kernel[46]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 1)) * kernel[47]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 1)) *	kernel[48]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 1)) * kernel[49]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 1)) * kernel[50]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 1)) * kernel[51]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 1)) * kernel[52]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower * 1)) * kernel[53]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower * 2)) * kernel[54]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 2)) * kernel[55]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 2)) * kernel[56]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 2)) *	kernel[57]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 2)) * kernel[58]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 2)) *	kernel[59]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 2)) *	kernel[60]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 2)) * kernel[61]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower * 2)) * kernel[62]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower * 3)) * kernel[63]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 3)) * kernel[64]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 3)) * kernel[65]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 3)) *	kernel[66]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 3)) * kernel[67]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 3)) * kernel[68]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 3)) * kernel[69]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 3)) *	kernel[70]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower * 3)) *	kernel[71]

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-4, _BlurPower * 4)) * kernel[72]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 4)) * kernel[73]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 4)) * kernel[74]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 4)) *	kernel[75]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 4)) * kernel[76]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 4)) * kernel[77]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 4)) * kernel[78]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 4)) *	kernel[79]
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 4, _BlurPower * 4)) *	kernel[80]
				/*
				+tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-3)) * 0.000036
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-3)) * 0.000363
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-3)) *	0.001446
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-3)) * 0.002291
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-3)) * 0.001446
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-3)) * 0.000363
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-3)) * 0.000036

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-2)) * 0.000363
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-2)) * 0.003676
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-2)) *	0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-2)) * 0.023226
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-2)) * 0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-2)) * 0.003676
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-2)) * 0.000363

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-1)) * 0.001446
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower *-1)) * 0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower *-1)) *	0.058488
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower *-1)) * 0.092651
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower *-1)) * 0.058488
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower *-1)) * 0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower *-1)) * 0.001446

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 0)) *	0.002291
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 0)) *	0.023226
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 0)) *	0.092651
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 0)) * 0.146768
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 0)) * 0.092651
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 0)) * 0.023226
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 0)) * 0.002291

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 1)) * 0.001446
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 1)) * 0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 1)) *	0.058488
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 1)) * 0.092651
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 1)) * 0.058488
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 1)) * 0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 1)) * 0.001446

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 2)) * 0.000363
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 2)) * 0.003676
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 2)) *	0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 2)) * 0.023226
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 2)) *	0.014662
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 2)) *	0.003676
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 2)) * 0.000363

				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower * 3)) * 0.000036
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-2, _BlurPower * 3)) * 0.000363
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-1, _BlurPower * 3)) *	0.001446
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 0, _BlurPower * 3)) * 0.002291
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 1, _BlurPower * 3)) * 0.001446
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 2, _BlurPower * 3)) * 0.000363
				+ tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower * 3, _BlurPower * 3)) *	0.000036
				*/
				).g;

		//o.Alpha = 0.9f - baseColor.g;
		//o.Alpha = 0.9f - tex2D(_MainTex, IN.uv_MainTex).g;
		//*/
		/*
		o.Alpha = 0.9f -
			(

				+tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower *-3, _BlurPower *-3)) * 0.000036
				(2.0f*PI*sqr(sigma)) * exp(-((sqr(x) + sqr(y)) / (2.0f*sqr(sigma))))

				).g;
				*/
	}

	ENDCG
	}

		Fallback "Diffuse"
}
