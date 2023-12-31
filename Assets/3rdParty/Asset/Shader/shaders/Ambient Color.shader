﻿Shader "Surface/Lambert Ambient"
{
	Properties
	{
		_Color ("Color" , Color) = (0,0,0,0)
		_Ambient("Ambient intensity",Range(0,2)) = 1
		_Directional("Directional Light",Range(0,2)) = 0 
	}

	Subshader
	{

		Pass
		{
			Tags{"Lightmode" = "ForwardBase"}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			

			// user defined
			uniform float4 _Color;
			uniform float _Ambient;
			uniform float _Directional;

			// unity defined
			uniform float4 _LightColor0;

			// structs

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
		
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};

			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;

				float3 normalDirection = normalize(mul(float4(v.normal,0), unity_WorldToObject).xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz)*_Directional;
				float3 difuseReflection = _LightColor0 * max(0,dot(normalDirection, lightDirection));
				float3 lightFinal = difuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz * _Ambient;
				
				o.col = float4(lightFinal * _Color.rgb,1);

				o.pos = UnityObjectToClipPos(v.vertex);
	
				return o;
			}

			float4 frag(vertexOutput i) : COLOR
			{
				return i.col;
			}
	
			ENDCG
		}

		// GÖLGE
		Pass
		{
			Tags {"LightMode" = "ShadowCaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}

	}
}