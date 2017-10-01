﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GlowComposite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
	    Tags
        {
            "Queue"="Geometry"
            "RenderType"="Transparent"
        }

		//Cull Off 
		ZWrite Off
		ZTest Always
		//Blend SrcAlpha OneMinusSrcAlpha 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			float2 _MainTex_TexelSize;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0 = v.uv;
				o.uv1 = v.uv;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv1.y = 1 - o.uv1.y;
				#endif

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _GlowPrePassTex;
			sampler2D _GlowBlurTex;
			sampler2D _PlayerTex;
			float _Intensity;
			//float _Cutoff;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv0);
				fixed4 player = tex2D(_PlayerTex, i.uv1);
				fixed4 prePass = tex2D(_GlowPrePassTex, i.uv1);
				fixed4 glowBlur = tex2D(_GlowBlurTex, i.uv1);
				prePass = prePass - player * prePass;
				glowBlur = glowBlur + player * glowBlur;
				fixed4 glow = max(0, (prePass - glowBlur));
				return col + glow * _Intensity;
			}
			ENDCG
		}
	}
}
