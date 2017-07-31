Shader "Custom/InGameHpbarShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_HealthTex ("Health Texture", 2D) = "white" {}
		_CutoffTex("Cutoff Texture", 2D) = "white" {}
		_Cutoff("Cutoff Value", Range(0.0, 1.0)) = 0.0
		_InvertedCutoff("Inverted Cutoff Texture", Range(0.0, 1.0)) = 0.0
		_HitPercentage("Hit Value", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		Tags
		{		"Queue" = "Transparent"

		}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex, _HealthTex, _CutoffTex;
			float4 _MainTex_ST, _HealthTex_ST, _CutoffTex_ST;
			float _Cutoff, _HitPercentage;
			fixed _InvertCutoff;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float c = 1 - tex2D(_CutoffTex, i.uv).x; 
				if (_HitPercentage > c) {
					col = tex2D(_HealthTex, i.uv)*2;
				}
				if (_Cutoff > c) {
					col = tex2D(_HealthTex, i.uv);
				}
				return col;
			}
			ENDCG
		}
	}
}
