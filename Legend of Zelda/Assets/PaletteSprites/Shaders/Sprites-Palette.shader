Shader "Palette/Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_PaletteTex("Palette Texture", 2D) = "white" {}
		[HideInInspector] _PaletteSize("Palette Size", Float) = 0
		_Palette("Palette Index", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			//#pragma enable_d3d11_debug_symbols

			#include "UnityCG.cginc"

			#include "PaletteSprite.cginc"

			#ifdef UNITY_INSTANCING_ENABLED

				UNITY_INSTANCING_CBUFFER_START(PerDrawSprite)
					
					fixed4 unity_SpriteRendererColorArray[UNITY_INSTANCED_ARRAY_SIZE];
					// this could be smaller but that's how bit each entry is regardless of type
					float4 unity_SpriteFlipArray[UNITY_INSTANCED_ARRAY_SIZE];
					
					//float unity_PaletteArray[UNITY_INSTANCED_ARRAY_SIZE];

					// TODO: This might not need to be instanced
					//float unity_PaletteSizeArray[UNITY_INSTANCED_ARRAY_SIZE];

					//UNITY_DEFINE_INSTANCED_PROP(float4, _Flip);
					UNITY_DEFINE_INSTANCED_PROP(float, _Palette)
					UNITY_DEFINE_INSTANCED_PROP(float, _PaletteSize)
				UNITY_INSTANCING_CBUFFER_END
				
				#define _RendererColor unity_SpriteRendererColorArray[unity_InstanceID]

				#define _Flip unity_SpriteFlipArray[unity_InstanceID]

				//#define _Palette unity_PaletteArray[unity_InstanceID]

				//#define _PaletteSize unity_PaletteArray[unity_InstanceID]
			#endif // instancing

			CBUFFER_START(UnityPerDrawSprite)
#ifndef UNITY_INSTANCING_ENABLED
					fixed4 _RendererColor;
					float4 _Flip;
					float _Palette;
					float _PaletteSize;
#endif
					float _EnableExternalAlpha;
			CBUFFER_END

			#pragma vertex SpriteVert
			#pragma fragment SpriteFrag
			//#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

//#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
//#endif

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _RendererColor;

#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _PaletteTex;

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);

				float4 c = tex2D(_MainTex, IN.texcoord);
				
				float palette = UNITY_ACCESS_INSTANCED_PROP(_Palette);

				float size = UNITY_ACCESS_INSTANCED_PROP(_PaletteSize);

				float4 pc = getPaletteColor(c.r, palette, size, _PaletteTex) * IN.color;

				pc.rgb *= c.a;
				pc.a = c.a;

				return pc;
			}
			ENDCG
		}
	}
}
