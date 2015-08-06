#version 120

uniform vec2 virtTexSize;
uniform float mipSampleBias;
uniform float spaceId;

varying vec2 texCoords;

//External Functions
float texMipLevel(vec2 coord, vec2 texSize);

void main()
{
	gl_FragColor.rg = texCoords.xy;

	float mipLevel = texMipLevel(texCoords.xy, virtTexSize) + mipSampleBias;
	mipLevel = clamp(mipLevel, 0.0, 15.0); //Could clamp this smaller, remember a higher mip index is a smaller actual texture (full size is 0)
	gl_FragColor.b = mipLevel / 255.0;

	gl_FragColor.a = spaceId / 255.0;
}