Shader "Custom/OxygenBarShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FillTex ("Fill Texture", 2D) = "white" {}
		_BorderTex ("Border Texture", 2D) = "white" {}
		_DetailTex ("Detail Texture", 2D) = "white" {}
		_CutoffTex("Cutoff Texture", 2D) = "white" {}
		_Cutoff("Cutoff Value", Range(0.0, 1.0)) = 0.0
		_HighlightValue("Highlight Value", Range(0.0, 1.0)) = 0.0
		_FlashValue("Flash Value", Range(-1.0, 1.0)) = 0.0
		_FlashColor("Flash Color", Color) = (1.0, 1.0, 1.0, 1.0)
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
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv_detail : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex, _FillTex, _BorderTex, _DetailTex, _CutoffTex;
			float4 _MainTex_ST, _FillTex_ST, _BorderTex_ST, _DetailTex_ST, _CutoffTex_ST;
			float _Cutoff, _HighlightValue;
			fixed _InvertCutoff;
			half _FlashValue;
			fixed4 _FlashColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv_detail = TRANSFORM_TEX(v.uv, _DetailTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				fixed4 background = tex2D(_MainTex, i.uv);
				fixed4 fill = tex2D(_FillTex, i.uv);
				fixed4 border = tex2D(_BorderTex, i.uv);
				float c = 1 - tex2D(_CutoffTex, i.uv).x; 
				fixed time = (sin(_Time.y*5)+1)/2;
				col = background;
				if (_Cutoff > c) {
					if (_FlashValue == 1.0 && fill.a != 0) {
						fill = lerp(fill + _FlashColor, fill, time);
					}
					col = fill + border;
				}

				//_DetailTex_ST.w -= _DetailTex_ST.w * _Time.y;
				//fixed4 detail =  tex2D(_DetailTex, i.uv_detail + _DetailTex_ST.zw);
				//fixed4 small_detail = tex2D(_DetailTex, i.uv_detail * 2 + _DetailTex_ST.zw);

				return col;
			}
			ENDCG
		}
	}
}
