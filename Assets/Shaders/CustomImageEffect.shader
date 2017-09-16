Shader "Custom/CustomImageEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1, 1, 1, 1)
		//_Cutoff("Cutoff", Range(0, 1.05)) = 0.0
		//_CurrentTime("Current Time", float) = 0
		//_Enabled("Enabled", float) = 0
		_A("A", Range(0, 1)) = 0.5
		_B("B", Range(0, 1)) = 0.5
		_R("R", Range(0, 1)) = 0.5
	}

		SubShader
		{
			Tags
			{		"Queue" = "Transparent"
					"IgnoreProjector" = "true"
					"RenderType" = "Transparent"
					"CanUseSpriteAtlas" = "true"
					"PreviewType" = "Plane"
			}
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend DstColor OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _Mask;
				float4 _MainTex_ST;
				fixed4 _TintColor;
				fixed _A, _B, _R;
				//float _Cutoff, _CurrentTime, _Enabled;

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					//fixed4 col = tex2D(_MainTex, i.uv0);
					//return col * _TintColor;
					fixed4 source = tex2D(_MainTex, i.uv);
					fixed4 mask = tex2D(_Mask, i.uv);



					//circle
					/*
					float2 dist = i.uv - 0.5;
					if (1 - (dist.x*dist.x + dist.y*dist.y) < 0.75) {
						source = _TintColor;
					}
					*/

					//elipse
					/*
					fixed _C = 0.5;
					fixed _B = sqrt(_A*_A -  _C* _C);
					float2 dist = i.uv - _C;
					if (1 - ((dist.x*dist.x)/(_A*_A) + (dist.y*dist.y)/(_B*_B)) < 1 - _R) {
						source.rgb = _TintColor.rgb;
					}
					//source.a = source.a * _TintColor.a;
					*/

					return source;
				}
				ENDCG
			}
		}
}
