#extension GL_OES_standard_derivatives : require

precision highp float;

uniform vec2 virtTexSize;
uniform float mipSampleBias;
uniform float spaceId;

varying vec2 texCoords;

//External Functions
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
	gl_FragColor.rg = texCoords.xy;

	float mipLevel = texMipLevel(texCoords.xy, virtTexSize) + mipSampleBias;
	mipLevel = clamp(mipLevel, 0.0, 15.0); //Could clamp this smaller, remember a higher mip index is a smaller actual texture (full size is 0)
	gl_FragColor.b = mipLevel / 255.0;

	gl_FragColor.a = spaceId / 255.0;
}