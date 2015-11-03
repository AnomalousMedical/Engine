//Vertex program to fragment program struct
struct v2f
{
	//Output position
	float4 position : SV_POSITION;

#ifdef NO_MAPS
	//Normal
	float3 normal : TEXCOORD0;
#else
	//Output texture coords
	float2 texCoords : TEXCOORD0;
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
	#ifdef NO_MAPS
		uniform float4 diffuseColor,				//The diffuse color of the object
		uniform float4 specularColor,				//The specular color of the surface
	#endif

	#ifdef NORMAL_MAPS
		uniform Texture2D normalTexture : register(t0),	//The normal map
		uniform SamplerState normalTextureSampler : register(s0),	//The normal map
		uniform float4 diffuseColor,				//The diffuse color of the object
		uniform float4 specularColor,				//The specular color of the surface

		#ifdef VIRTUAL_TEXTURE
			uniform Texture2D indirectionTex : register(t1),
			uniform SamplerState indirectionTexSampler : register(s1),
		#endif //VIRTUAL_TEXTURE
	#endif //NORMAL_MAPS

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

	#ifdef NORMAL_DIFFUSE_SPECULAR_MAPS
		uniform Texture2D normalTexture : register(t0),	//The normal map
		uniform SamplerState normalTextureSampler : register(s0),	//The normal map
		uniform Texture2D colorTexture : register(t1),  //The color map
		uniform SamplerState colorTextureSampler : register(s1),  //The color map
		uniform Texture2D specularTexture : register(t2),  //The specular color map
		uniform SamplerState specularTextureSampler : register(s2),  //The specular color map

		#ifdef VIRTUAL_TEXTURE
			uniform Texture2D indirectionTex : register(t3),
			uniform SamplerState indirectionTexSampler : register(s3),
		#endif //VIRTUAL_TEXTURE
	#endif //NORMAL_DIFFUSE_SPECULAR_MAPS

	#ifdef NORMAL_DIFFUSE_OPACITY_MAPS
		uniform Texture2D normalTexture : register(t0),	//The normal map
		uniform SamplerState normalTextureSampler : register(s0),	//The normal map
		uniform Texture2D colorTexture : register(t1),  //The color map
		uniform SamplerState colorTextureSampler : register(s1),  //The color map

		#ifdef SEPARATE_OPACITY
			uniform Texture2D opacityTexture : register(t2), //The Opacity map, uses r channel for opacity
			uniform SamplerState opacityTextureSampler : register(s2), //The Opacity map, uses r channel for opacity

			#ifdef VIRTUAL_TEXTURE
				uniform Texture2D indirectionTex : register(t3),
				uniform SamplerState indirectionTexSampler : register(s3),
			#endif //VIRTUAL_TEXTURE
		#else
			#ifdef VIRTUAL_TEXTURE
				uniform Texture2D indirectionTex : register(t2),
				uniform SamplerState indirectionTexSampler : register(s2),
			#endif //VIRTUAL_TEXTURE
		#endif //SEPARATE_OPACITY

		uniform float4 specularColor,				//The specular color of the surface
	#endif //NORMAL_DIFFUSE_SPECULAR_MAPS

	#ifdef NORMAL_DIFFUSE_SPECULAR_OPACITY_MAPS
		uniform Texture2D normalTexture : register(t0),	//The normal map
		uniform SamplerState normalTextureSampler : register(s0),	//The normal map
		uniform Texture2D colorTexture : register(t1),  //The color map
		uniform SamplerState colorTextureSampler : register(s1),  //The color map
		uniform Texture2D specularTexture : register(t2),  //The specular color map
		uniform SamplerState specularTextureSampler : register(s2),  //The specular color map

		#ifdef SEPARATE_OPACITY
			uniform Texture2D opacityTexture : register(t3), //The Opacity map, uses r channel for opacity
			uniform SamplerState opacityTextureSampler : register(s3), //The Opacity map, uses r channel for opacity
			#ifdef VIRTUAL_TEXTURE
				uniform Texture2D indirectionTex : register(t4),
				uniform SamplerState indirectionTexSampler : register(s4),
			#endif //VIRTUAL_TEXTURE
		#else
			#ifdef VIRTUAL_TEXTURE
				uniform Texture2D indirectionTex : register(t3),
				uniform SamplerState indirectionTexSampler : register(s3),
			#endif //VIRTUAL_TEXTURE
		#endif //SEPARATE_OPACITY
	#endif //NORMAL_DIFFUSE_SPECULAR_OPACITY_MAPS

	#ifdef NORMAL_OPACITY_MAPS
		uniform Texture2D normalTexture : register(t0),	//The normal map
		uniform SamplerState normalTextureSampler : register(s0),	//The normal map

		#ifdef SEPARATE_OPACITY
			uniform Texture2D opacityTexture : register(t1), //The Opacity map, uses r channel for opacity
			uniform SamplerState opacityTextureSampler : register(s1), //The Opacity map, uses r channel for opacity

			#ifdef VIRTUAL_TEXTURE
				uniform Texture2D indirectionTex : register(t2),
				uniform SamplerState indirectionTexSampler : register(s2),
			#endif //VIRTUAL_TEXTURE
		#else
			#ifdef VIRTUAL_TEXTURE
				uniform Texture2D indirectionTex : register(t1),
				uniform SamplerState indirectionTexSampler : register(s1),
			#endif //VIRTUAL_TEXTURE
		#endif //SEPARATE_OPACITY

		uniform float4 diffuseColor,				//The diffuse color of the object
		uniform float4 specularColor,				//The specular color of the surface
	#endif //NORMAL_OPACITY_MAPS

	#ifdef GLOSS_MAP
		uniform float glossyStart,
		uniform float glossyRange,
	#else //!GLOSS_MAP
		uniform float glossyness,					//The glossyness of the surface
	#endif //GLOSS_MAP

	#ifdef HIGHLIGHT
		uniform float4 highlightColor,				//A color to multiply the final color by to create a highlight effect
	#endif //HIGHLIGHT

	#ifdef OPACITY
		uniform float opacity,						//The opacity for an entire object if this is not defined by an opacity map
	#endif //OPACITY

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
#ifndef NO_MAPS
	float2 texCoords = input.texCoords;
	#ifdef VIRTUAL_TEXTURE
		texCoords = vtexCoord(texCoords, indirectionTex, indirectionTexSampler, physicalSizeRecip, mipBiasSize, pagePaddingScale, pagePaddingOffset);
	#endif //VIRTUAL_TEXTURE
#endif //NO_MAPS

#ifdef DIFFUSE_MAP
	//Get diffuse map value
	float4 diffuseColor = colorTexture.Sample(colorTextureSampler, texCoords.xy);
#endif //DIFFUSE_MAP

#ifdef SPECULAR_MAP
	float4 specularColor = specularTexture.Sample(specularTextureSampler, texCoords.xy);
#endif

//Determine specular amount depending on which textures are active
#ifdef SPECULAR_MAP
	float specularAmount = specularColor.a;
#else
	float specularAmount = diffuseColor.a;
#endif

#ifdef NORMAL_MAP
	//Unpack the normal map.
	float4 normalRead = normalTexture.Sample(normalTextureSampler, texCoords);
	float3 normal;
	#ifdef RG_NORMALS
		normal.rg = 2.0f * (normalRead.rg - 0.5f);
	#else
		normal.rg = 2.0f * (normalRead.ag - 0.5f);
	#endif
	normal.b = sqrt(1 - normal.r * normal.r - normal.g * normal.g);
#else
	float3 normal = input.normal;
#endif //NORMAL_MAP

#if OPACITY_MAP || (GLOSS_MAP && GLOSS_CHANNEL_OPACITY_GREEN)
	#ifdef SEPARATE_OPACITY
		float2 opacityMapValue = opacityTexture.Sample(opacityTextureSampler, texCoords).rg;
	#else
		float2 opacityMapValue = normalRead.ba;
	#endif //SEPARATE_OPACITY
#endif

#ifdef GLOSS_MAP
	#ifdef GLOSS_CHANNEL_OPACITY_GREEN
		float glossyness = glossyStart + glossyRange * opacityMapValue.g;
	#else
		float glossyness = glossyStart + glossyRange * diffuseColor.a;
	#endif
#endif //GLOSS_MAP

	float4 color = doLighting(
		//Lighting and eye
		unpack(input.lightVector), unpack(input.halfVector), lightDiffuseColor, input.attenuation, 
		//Diffuse color
		diffuseColor, 
		//Specular color
		specularColor, specularAmount, glossyness, 
		//emisssive
		emissiveColor, 
		//Normal
		normal);

#ifdef HIGHLIGHT
	color *= highlightColor;
#endif //HIGHLIGHT

#if defined(OPACITY_MAP) || defined(OPACITY)
	#ifdef OPACITY_MAP
		#ifdef ALPHA
			color.a = opacityMapValue.r - (1.0f - alpha.a);
		#else
			color.a = opacityMapValue.r;
		#endif
	#endif
	#ifdef OPACITY
		#ifdef ALPHA
			color.a = opacity - (1.0f - alpha.a);
		#else
			color.a = opacity;
		#endif
	#endif
#else //OPACITY_MAP
	#ifdef ALPHA
		color.a = alpha.a;
	#endif //ALPHA
#endif //OPACITY_MAP

	return color;
}