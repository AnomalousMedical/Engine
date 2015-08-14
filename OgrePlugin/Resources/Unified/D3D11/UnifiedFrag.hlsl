//Vertex program to fragment program struct
struct v2f
{
	//Output position
	float4 position : SV_POSITION;

#ifdef HAS_TEXTURES
	//Output texture coords
	float2 texCoords : TEXCOORD0;
#else
	//Normal
	float3 normal : TEXCOORD0;
#endif

	//Light vector in tangent space
	float3 lightVector : TEXCOORD1;

	//Eye vector in tangent space
	float3 halfVector : TEXCOORD2;

	//Attenuation per vertex
	float4 attenuation : TEXCOORD3;
};

//Unpack function unpacks values from 0 to 1 to -1 to 1
float3 unpack(float3 input)
{
	return 2.0f * (input - 0.5);
}

//-------------------------------------------------------------------------------------------------

//----------------------------------
//Lighting function
//----------------------------------
float4 doLighting
(
		//Lighting and eye
	    float3 lightVector,
		float3 halfVector,
		float4 lightDiffuseColor,
		float4 attenuation,

		//Diffuse color
		float4 diffuseColor,

		//Specular color
		float4 specularColor,
		float specularAmount,
		float glossyness,

		//Emissive color
		float4 emissive,
		
		//Normal
		float3 normal
)
{
	//Nvidia cg shader book equation
	float3 N = normalize(normal);
	
	//Diffuse term
	float3 L = normalize(lightVector);
	float diffuseLight = saturate(dot(N, L));
	float3 diffuse = diffuseColor.rgb * lightDiffuseColor.rgb * diffuseLight;
	
	//Specular term
	float3 H = normalize(halfVector);
	float specularLight = pow(saturate(dot(N, H)), glossyness);
	float3 specular = specularAmount * (specularColor.rgb * lightDiffuseColor.rgb * specularLight);
	
	float4 finalColor;
	finalColor.rgb = diffuse + specular + emissive.rgb;
	finalColor.a = 1.0;
	
	return finalColor;
}

//-------------------------------------------------------------------------------------------------

//----------------------------------
//Virtual Texture Coords
//----------------------------------

float texMipLevel(float2 coord, float2 texSize) //This is a duplicate of the function in FeedbackBuffer.hlsl
{
	float2 dxScaled, dyScaled;
	float2 coord_scaled = coord * texSize;

	dxScaled = ddx(coord_scaled);
	dyScaled = ddy(coord_scaled);

	float2 dtex = dxScaled*dxScaled + dyScaled*dyScaled;
	float minDelta = max(dtex.x, dtex.y);
	float miplevel = max(0.5 * log2(minDelta), 0.0);

	return miplevel;
}

float2 vtexCoord(float2 address, Texture2D indirectionTex, SamplerState indirectionTexSampler, float2 physicalSizeRecip, float2 mipBiasSize, float2 pagePaddingScale, float2 pagePaddingOffset)
{
	float mipLevel = texMipLevel(address, mipBiasSize);

	//Need to add bias for mip levels, the bias adjusts the size of the indirection texture to the size of the physical texture
	float4 redirectInfo = indirectionTex.SampleLevel(indirectionTexSampler, address.xy, mipLevel);

	float mip2 = floor(exp2(redirectInfo.b * 255.0) + 0.5); //Figure out how far to shift the original address, based on the mip level, highest mip level (1x1 indirection texture) is 0 counting up from there
	float2 coordLow = frac(address * mip2); //Get fractional part of page location, this is shifted left by the mip level

	float2 page = floor(redirectInfo.rg * 255.0 + 0.5); //Get the page number on the physical texture

	float2 finalCoord = page + coordLow * pagePaddingScale + pagePaddingOffset; //Add these together to get the coords in page space 64.0 / 66.0  1.0 / 66.0
	finalCoord = finalCoord * physicalSizeRecip; //Multiple by the physical texture page size to get back to uv space, that is our final coord
	
	return finalCoord;
}

//-------------------------------------------------------------------------------------------------

float4 UnifiedFragmentShader
(
	#ifdef NORMAL_DIFFUSE_MAPS
		uniform Texture2D normalTexture : register(t0),	//The normal map
		uniform SamplerState normalTextureSampler : register(s0),	//The normal map
		uniform Texture2D colorTexture : register(t1),  //The color map
		uniform SamplerState colorTextureSampler : register(s1),  //The color map
		uniform float4 specularColor,				//The specular color of the surface

		#ifdef VIRTUAL_TEXTURE
			uniform Texture2D indirectionTex : register(t2),
			uniform SamplerState indirectionTexSampler : register(s2),
		#endif //VIRTUAL_TEXTURE
	#endif //NORMAL_DIFFUSE_MAPS

	#ifdef GLOSS_MAP
		uniform float glossyStart,
		uniform float glossyRange,
	#else //!GLOSS_MAP
		uniform float glossyness,					//The glossyness of the surface
	#endif //GLOSS_MAP

	#ifdef HIGHLIGHT
		uniform float4 highlightColor,				//A color to multiply the final color by to create a highlight effect
	#endif //HIGHLIGHT

	#ifdef ALPHA
		uniform float4 alpha,
	#endif

	#ifdef VIRTUAL_TEXTURE
			uniform float2 physicalSizeRecip,
			uniform float2 mipBiasSize,
			uniform float2 pagePaddingScale, 
			uniform float2 pagePaddingOffset,
	#endif //VIRTUAL_TEXTURE

	//Universal
	uniform float4 emissiveColor,			    //The emissive color of the surface
	uniform float4 lightDiffuseColor,			//The diffuse color of the light source

	in v2f input //Keep this guy last for easy commas
) : SV_TARGET
{
#ifdef HAS_TEXTURES
	float2 texCoords = input.texCoords;
#endif

#ifdef VIRTUAL_TEXTURE
	texCoords = vtexCoord(texCoords, indirectionTex, indirectionTexSampler, physicalSizeRecip, mipBiasSize, pagePaddingScale, pagePaddingOffset);
#endif //VIRTUAL_TEXTURE

#ifdef DIFFUSE_MAP
	//Get diffuse map value
	float4 colorMap = colorTexture.Sample(colorTextureSampler, texCoords.xy);
#endif //DIFFUSE_MAP

#ifdef NORMAL_MAP
	//Unpack the normal map.
	float3 normal;
	#ifdef RG_NORMALS
		normal.rg = 2.0f * (normalTexture.Sample(normalTextureSampler, texCoords).rg - 0.5f);
	#else
		normal.rg = 2.0f * (normalTexture.Sample(normalTextureSampler, texCoords).ag - 0.5f);
	#endif
		normal.b = sqrt(1 - normal.r * normal.r - normal.g * normal.g);
#endif //NORMAL_MAP

#ifdef GLOSS_MAP
	float glossyness = glossyStart + glossyRange * 1;// colorMap.a;, need to determine correct texture
#endif //GLOSS_MAP

	float4 color = doLighting(unpack(input.lightVector), unpack(input.halfVector), lightDiffuseColor, input.attenuation, colorMap, specularColor, colorMap.a, glossyness, emissiveColor, normal);

#ifdef HIGHLIGHT
	color *= highlightColor;
#endif //HIGHLIGHT

#ifdef ALPHA
	color.a = alpha.a;
#endif //ALPHA

	return color;
}