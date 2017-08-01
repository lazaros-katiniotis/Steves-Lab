Shader "Custom/SpriteShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1, 1, 1, 1)
		_Cutoff("Cutoff", Range(0, 1.05)) = 0.0
		_CurrentTime("Current Time", float) = 0
		_Enabled("Enabled", float) = 0
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

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _TintColor;
				float _Cutoff, _CurrentTime, _Enabled;

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv0 = TRANSFORM_TEX(v.uv0, _MainTex);
					o.uv1 = v.uv0;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv0);
					return col * _TintColor;
				}
				ENDCG
			}
		}
}
