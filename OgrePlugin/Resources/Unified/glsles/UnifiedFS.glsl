precision highp float;

//External function prototypes
//----------------------------------
//Lighting function
//----------------------------------
vec4 doLighting
(
		//Lighting and eye
	    vec3 lightVector,
		vec3 halfVector,
		vec4 lightDiffuseColor,
		vec4 attenuation,

		//Diffuse color
		vec4 diffuseColor,

		//Specular color
		vec4 specularColor,
		float specularAmount,
		float glossyness,

		//Emissive color
		vec4 emissive,
		
		//Normal
		vec3 normal
)
{
	//Nvidia cg shader book equation
	vec3 N = normalize(normal);
	
	//Diffuse term
	vec3 L = normalize(lightVector);
	float diffuseLight = clamp(dot(N, L), 0.0, 1.0);
	vec3 diffuse = diffuseColor.rgb * lightDiffuseColor.rgb * diffuseLight;
	
	//Specular term
	vec3 H = normalize(halfVector);
	float specularLight = pow(clamp(dot(N, H), 0.0, 1.0), glossyness);
	vec3 specular = specularAmount * (specularColor.rgb * lightDiffuseColor.rgb * specularLight);
	
	vec4 finalColor;
	finalColor.rgb = diffuse + specular + emissive.rgb;
	finalColor.a = 1.0;
	
	return finalColor;
}

//Unpack function unpacks values from 0 to 1 to -1 to 1
vec3 unpack(vec3 packValue)
{
	return 2.0 * (packValue - 0.5);
}

#ifdef VIRTUAL_TEXTURE

#extension GL_OES_standard_derivatives : require

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

vec2 vtexCoord(vec2 address, sampler2D indirectionTex, vec2 physicalSizeRecip, vec2 mipBiasSize, vec2 pagePaddingScale, vec2 pagePaddingOffset, float pageSizeLog2)
{
	float mipLevel = texMipLevel(address, mipBiasSize);

	//Need to add bias for mip levels, the bias adjusts the size of the indirection texture to the size of the physical texture
	vec4 redirectInfo = texture2D(indirectionTex, address.xy, pageSizeLog2);

	float mip2 = floor(exp2(redirectInfo.b * 255.0) + 0.5); //Figure out how far to shift the original address, based on the mip level, highest mip level (1x1 indirection texture) is 0 counting up from there
	vec2 coordLow = fract(address * mip2); //Get fractional part of page location, this is shifted left by the mip level

	vec2 page = floor(redirectInfo.rg * 255.0 + 0.5); //Get the page number on the physical texture

	vec2 finalCoord = page + coordLow * pagePaddingScale + pagePaddingOffset; //Add these together to get the coords in page space 64.0 / 66.0  1.0 / 66.0
	finalCoord = finalCoord * physicalSizeRecip; //Multiple by the physical texture page size to get back to uv space, that is our final coord
	
	return finalCoord;
}

#endif

//Input
#ifdef NO_MAPS
	uniform vec4 diffuseColor;			//The diffuse color of the object
	uniform vec4 specularColor;			//The specular color of the surface
#endif

#ifdef NORMAL_MAPS
	uniform sampler2D normalTexture;	//The normal map
	uniform vec4 diffuseColor;			//The diffuse color of the object
	uniform vec4 specularColor;			//The specular color of the surface
#endif //NORMAL_MAPS

#ifdef NORMAL_DIFFUSE_MAPS
	uniform sampler2D normalTexture;	//The normal map
	uniform sampler2D colorTexture;		//The color map
	uniform vec4 specularColor;	 		//The specular color of the surface
#endif //NORMAL_DIFFUSE_MAPS

#ifdef NORMAL_DIFFUSE_SPECULAR_MAPS
	uniform sampler2D normalTexture;	//The normal map
	uniform sampler2D colorTexture;		//The color map
	uniform sampler2D specularTexture;  //The specular color map
#endif //NORMAL_DIFFUSE_SPECULAR_MAPS

#ifdef NORMAL_DIFFUSE_OPACITY_MAPS
	uniform sampler2D normalTexture;	//The normal map
	uniform sampler2D colorTexture;		//The color map
	uniform sampler2D opacityTexture;	//The Opacity map, uses r channel for opacity
	uniform vec4 specularColor;			//The specular color of the surface
#endif //NORMAL_DIFFUSE_SPECULAR_MAPS

#ifdef NORMAL_DIFFUSE_SPECULAR_OPACITY_MAPS
	uniform sampler2D normalTexture;	//The normal map
	uniform sampler2D colorTexture;		//The color map
	uniform sampler2D specularTexture;	//The specular color map
	uniform sampler2D opacityTexture;	//The Opacity map, uses r channel for opacity
#endif //NORMAL_DIFFUSE_SPECULAR_OPACITY_MAPS

#ifdef GLOSS_MAP
	uniform float glossyStart;
	uniform float glossyRange;
#else //!GLOSS_MAP
	uniform float glossyness;			//The glossyness of the surface
#endif //GLOSS_MAP

#ifdef HIGHLIGHT
	uniform vec4 highlightColor;		//A color to multiply the final color by to create a highlight effect
#endif //HIGHLIGHT

#ifdef OPACITY
	uniform float opacity;				//The opacity for an entire object if this is not defined by an opacity map
#endif //OPACITY

#ifdef ALPHA
	uniform vec4 alpha;
#endif

#ifdef VIRTUAL_TEXTURE
	uniform sampler2D indirectionTex;
	uniform vec2 physicalSizeRecip;
	uniform vec2 mipBiasSize;
	uniform vec2 pagePaddingScale;
	uniform vec2 pagePaddingOffset;
	uniform float pageSizeLog2;
#endif //VIRTUAL_TEXTURE

//Universal
uniform vec4 emissiveColor;			    //The emissive color of the surface
uniform vec4 lightDiffuseColor;			//The diffuse color of the light source

//Vertex shader output
#ifdef NO_MAPS
	varying vec3 passNormal;
#else
	varying vec2 texCoords;
#endif
varying vec3 lightVector;	//Light vector in tangent space
varying vec3 halfVector;	//Eye vector in tangent space
varying vec4 attenuation;	//Attenuation per vertex

void main()
{
#ifndef NO_MAPS
	vec2 derivedCoords = texCoords;
	#ifdef VIRTUAL_TEXTURE
		derivedCoords = vtexCoord(texCoords, indirectionTex, physicalSizeRecip, mipBiasSize, pagePaddingScale, pagePaddingOffset, pageSizeLog2);
	#endif //VIRTUAL_TEXTURE
#endif //NO_MAPS

#ifdef DIFFUSE_MAP
	//Get diffuse map value
	vec4 diffuseColor = texture2D(colorTexture, derivedCoords.xy);
#endif //DIFFUSE_MAP

#ifdef SPECULAR_MAP
	vec4 specularColor = texture2D(specularTexture, derivedCoords.xy);
#endif

//Determine specular amount depending on which textures are active
#ifdef SPECULAR_MAP
	float specularAmount = specularColor.a;
#else
	float specularAmount = diffuseColor.a;
#endif

#ifdef NORMAL_MAP
	//Unpack the normal map.
	vec3 normal;
	#ifdef RG_NORMALS
		normal.rg = 2.0 * (texture2D(normalTexture, derivedCoords).rg - 0.5);
	#else
		normal.rg = 2.0 * (texture2D(normalTexture, derivedCoords).ag - 0.5);
	#endif
		normal.b = sqrt(1.0 - normal.r * normal.r - normal.g * normal.g);
#else
	vec3 normal = passNormal;
#endif //NORMAL_MAP

#ifdef OPACITY_MAP
	vec2 opacityMapValue = texture2D(opacityTexture, derivedCoords).rg;
#endif

#ifdef GLOSS_MAP
	#ifdef GLOSS_CHANNEL_OPACITY_GREEN
		float glossyness = glossyStart + glossyRange * opacityMapValue.g;
	#else
		float glossyness = glossyStart + glossyRange * diffuseColor.a;
	#endif
#endif //GLOSS_MAP

	gl_FragColor = doLighting(
		//Lighting and eye
		unpack(lightVector), unpack(halfVector), lightDiffuseColor, attenuation, 
		//Diffuse color
		diffuseColor, 
		//Specular color
		specularColor, specularAmount, glossyness, 
		//emisssive
		emissiveColor, 
		//Normal
		normal);

#ifdef HIGHLIGHT
	gl_FragColor *= highlightColor;
#endif //HIGHLIGHT

#if defined(OPACITY_MAP) || defined(OPACITY)
	#ifdef OPACITY_MAP
		#ifdef ALPHA
			gl_FragColor.a = opacityMapValue.r - (1.0f - alpha.a);
		#else
			gl_FragColor.a = opacityMapValue.r;
		#endif
	#endif
	#ifdef OPACITY
		#ifdef ALPHA
			gl_FragColor.a = opacity - (1.0f - alpha.a);
		#else
			gl_FragColor.a = opacity;
		#endif
	#endif
#else //OPACITY_MAP
	#ifdef ALPHA
		gl_FragColor.a = alpha.a;
	#endif //ALPHA
#endif //OPACITY_MAP
}