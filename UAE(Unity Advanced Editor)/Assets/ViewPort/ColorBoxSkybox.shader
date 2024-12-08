// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Digicrafts/ColorBoxSkybox"
{
	Properties
	{
		[KeywordEnum(Solid, Gradient, Gradient3)] _BG_COLOR ("Color Type", Float) = 1
		_BgColor (" ",Color) = (0.957, 1, 0.686,1)
		_BgColor1 ("Start Color",Color) = (0.667,0.851,0.937,1)
		_BgColor2 ("Middle Color",Color) = (0.29, 0.8, 0.2,1)
		_BgColor3 ("End Color",Color) = (0.29, 0.8, 0.2,1)
		[GradientPositionSliderDrawer]
		_BgColorPosition ("Gradient Position",Vector) = (0,1,0)
		_BgColorRotation ("Gradient Rotation",Range(0,2)) = 0
		_BgColorPosition3 ("Middle Size",Range(0,1)) = 0
		_BgColorIntensity ("Intensity",Range(0,4)) = 1
		[Toggle] _SCREENSPACE ("Use Screen Space", Float) = 1

	}
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _BG_COLOR_SOLID _BG_COLOR_GRADIENT _BG_COLOR_GRADIENT3
			#pragma shader_feature _SCREENSPACE_ON

			#include "UnityCG.cginc"
			#include "ColorBoxHelper.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{				
				float4 pos : SV_POSITION;

				#if _SCREENSPACE_ON
				float4 screenPos : TEXCOORD4;
				#else
				float3 worldPos : TEXCOORD2;
				float3 worldViewDir : TEXCOORD3;
				#endif
			};

			uniform fixed3 _BgColor;
			uniform fixed3 _BgColor1;
			uniform fixed3 _BgColor2;
			uniform fixed3 _BgColor3;
			uniform float _BgColorRotation;
			uniform float2 _BgColorPosition;
			uniform float _BgColorPosition3;
			uniform float _BgColorIntensity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				#if _SCREENSPACE_ON
					o.screenPos = ComputeScreenPos(o.pos);
				#else
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					o.worldViewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
				#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				#if _SCREENSPACE_ON
					float2 uv =  - (i.screenPos.xy / i.screenPos.w - 0.5)*2;
				#else
					float3 uv = i.worldViewDir;
				#endif

				fixed3 c;						
				#if _BG_COLOR_SOLID
					c = _BgColor.rgb;
				#elif _BG_COLOR_GRADIENT
					c = lerp(_BgColor1,_BgColor3,clampValue(rotateUV(uv.xy,_BgColorRotation*PI).y,_BgColorPosition));    					    				
				#elif _BG_COLOR_GRADIENT3
					c = lerp3(_BgColor1,_BgColor2,_BgColor3,clampValue(rotateUV(uv.xy,_BgColorRotation*PI).y,_BgColorPosition),_BgColorPosition3);
				#else
					c = _BgColor.rgb;
				#endif	        				

				return fixed4(c*_BgColorIntensity,1);
			}
			ENDCG
		}
	}
	CustomEditor "Digicrafts.Editor.ColorBoxSkyboxEditor"
}
