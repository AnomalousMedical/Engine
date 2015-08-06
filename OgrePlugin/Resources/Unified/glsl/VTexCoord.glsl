#version 120

vec2 vtexCoord(vec2 address, sampler2D indirectionTex, vec2 physicalSizeRecip, vec2 mipBiasSize, vec2 pagePaddingScale, vec2 pagePaddingOffset)
{
	float mipLevel = texMipLevel(address, mipBiasSize);

	//Need to add bias for mip levels, the bias adjusts the size of the indirection texture to the size of the physical texture
	vec4 redirectInfo = texture2D(indirectionTex, address.xy); //Need to do mip level here

	float mip2 = floor(exp2(redirectInfo.b * 255.0) + 0.5); //Figure out how far to shift the original address, based on the mip level, highest mip level (1x1 indirection texture) is 0 counting up from there
	vec2 coordLow = frac(address * mip2); //Get fractional part of page location, this is shifted left by the mip level

	vec2 page = floor(redirectInfo.rg * 255.0 + 0.5); //Get the page number on the physical texture

	vec2 finalCoord = page + coordLow * pagePaddingScale + pagePaddingOffset; //Add these together to get the coords in page space 64.0 / 66.0  1.0 / 66.0
	finalCoord = finalCoord * physicalSizeRecip; //Multiple by the physical texture page size to get back to uv space, that is our final coord
	
	return finalCoord;
}