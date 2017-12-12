// PaletteSprite source. Copyright (c) 2017 Julius Häger. CC0

#include "UnityCG.cginc"

inline float4 getPaletteColor(in float shade, in float palette, in float paletteSize, in sampler2D paletteTex) {
	return tex2D(paletteTex, float2(shade + 0.001, palette / paletteSize));
}