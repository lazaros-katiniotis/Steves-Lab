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
					"Glowable" = "False"
			}

			ZWrite Off
			Cull Off
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
					float2 uv : TEXCOORD0;
					half4 color : COLOR;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 screenuv : TEXCOORD1;
					half4 color : COLOR;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					//o.uv = v.uv;
					o.screenuv = ((o.vertex.xy/o.vertex.w)+1)*0.5;
					o.screenuv.y = 1 - o.screenuv.y;
					o.color = v.color;
					return o;
				}

				uniform sampler2D _GlobalTestTex;
				sampler2D _GlowPrePassTex;

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					//fixed4 col = tex2D(_GlobalTestTex, i.uv);
					//fixed4 col = tex2D(_GlowPrePassTex, i.uv);
					return col;
				}
				ENDCG
			}
		}
}
