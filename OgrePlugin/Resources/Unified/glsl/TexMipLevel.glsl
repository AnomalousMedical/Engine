#version 120

float texMipLevel(vec2 coord, vec2 texSize)
{
	vec2 dxScaled, dyScaled;
	vec2 coord_scaled = coord * texSize;

	dxScaled = dFdx(coord_scaled);
	dyScaled = dFdy(coord_scaled);

	vec2 dtex = dxScaled*dxScaled + dyScaled*dyScaled;
	float minDelta = max(dtex.x, dtex.y);
	float miplevel = max(0.5 * log2(minDelta), 0.0);

	return miplevel;
}