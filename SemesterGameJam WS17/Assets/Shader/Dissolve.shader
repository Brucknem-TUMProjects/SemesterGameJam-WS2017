Shader "RGP/Dissolve/DissolveAndEmit"
{
	Properties
	{
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		_EmissionMap("Emission Map", 2D) = "white" {}
		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveSize("Size of the effect", Float) = 0
		_StartingZ("Starting point of the effect", Float) = 0
		_Speed("Effect speed", Float) = 2
		[HDR] _EmissionColor("Emission Color", Color) = (0,0,0,1)
		_Frequency("Pulse Frequency", Float) = 1.0
	}
			SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaToMask On
			Cull Off

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct vertexInput
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct pixelInput
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 worldPos : TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _DissolveTex;
				float _DissolveSize;
				float _StartingZ;
				float _Speed;
				float4 _EmissionColor;
				sampler2D _EmissionMap;
				half _Frequency;

				pixelInput vert(vertexInput v)
				{
					pixelInput o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					return o;
				}

				fixed4 frag(pixelInput i) : SV_Target
				{
					float transition = _Speed - i.worldPos.z; //(_Time.y - _StartingTime) * 
					clip( _StartingZ + (transition + (tex2D(_DissolveTex, i.uv))*_DissolveSize));
					//float transition = _DissolveY - i.worldPos.y;
					//clip(_StartingY + (transition + (tex2D(_DissolveTex, i.uv)) * _DissolveSize));

					// calculate Emission

					half4 emission = tex2D(_EmissionMap, i.uv) * _EmissionColor ;
					fixed4 col = tex2D(_MainTex, i.uv);
					col.a = _EmissionColor.a;
					// Pulsing color, lerp between white and black
					int triple = (_Time.y * _Frequency)  / 3;

					triple %= 6;
					float offset = ((_Time.y * _Frequency) % 3) * (1.0 / 3.0);
					switch (triple)
					{	
					case 0:
						col.rgb = fixed3(1 - offset, 1, 0) * emission.rgb;
						break;
					case 1:
						col.rgb = fixed3(0, 1, offset) * emission.rgb;
						break;
					case 2:
						col.rgb = fixed3(0, 1 - offset, 1) * emission.rgb;
						break;
					case 3:
						col.rgb = fixed3(offset, 0, 1) * emission.rgb;
						break;
					case 4:
						col.rgb = fixed3(1, offset, 1) * emission.rgb;
						break;
					case 5:
						col.rgb = fixed3(1, 1, 1 - offset) * emission.rgb;
						break;
					}
					return col;
				}
			ENDCG
		}
		}
}
