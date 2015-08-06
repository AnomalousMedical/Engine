#version 120

uniform vec2 virtTexSize;
uniform float mipSampleBias;
uniform float spaceId;

varying vec2 texCoords;

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

void main()
{
	vec4 result;
	result.rg = texCoords.xy;

	float mipLevel = texMipLevel(texCoords.xy, virtTexSize) + mipSampleBias;
	mipLevel = clamp(mipLevel, 0.0, 15.0); //Could clamp this smaller, remember a higher mip index is a smaller actual texture (full size is 0)
	result.b = mipLevel / 255.0;

	result.a = spaceId / 255.0;

	gl_FragColor = result;
}