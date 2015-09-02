#version 120

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
);

//Unpack function unpacks values from 0 to 1 to -1 to 1
vec3 unpack(vec3 toUnpack);

#ifdef VIRTUAL_TEXTURE

float texMipLevel(vec2 coord, vec2 texSize);

vec2 vtexCoord(vec2 address, sampler2D indirectionTex, vec2 physicalSizeRecip, vec2 mipBiasSize, vec2 pagePaddingScale, vec2 pagePaddingOffset, float pageSizeLog2);

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
	#ifdef SEPARATE_OPACITY
		uniform sampler2D opacityTexture;	//The Opacity map, uses r channel for opacity
	#endif //SEPARATE_OPACITY
	uniform vec4 specularColor;			//The specular color of the surface
#endif //NORMAL_DIFFUSE_SPECULAR_MAPS

#ifdef NORMAL_DIFFUSE_SPECULAR_OPACITY_MAPS
	uniform sampler2D normalTexture;	//The normal map
	uniform sampler2D colorTexture;		//The color map
	uniform sampler2D specularTexture;	//The specular color map
	#ifdef SEPARATE_OPACITY
		uniform sampler2D opacityTexture;	//The Opacity map, uses r channel for opacity
	#endif //SEPARATE_OPACITY
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
	vec4 normalRead = texture2D(normalTexture, derivedCoords);
	//Unpack the normal map.
	vec3 normal;
	#ifdef RG_NORMALS
		normal.rg = 2.0f * (normalRead.rg - 0.5f);
	#else
		normal.rg = 2.0f * (normalRead.ag - 0.5f);
	#endif
		normal.b = sqrt(1.0f - normal.r * normal.r - normal.g * normal.g);
#else
	vec3 normal = passNormal;
#endif //NORMAL_MAP

#ifdef OPACITY_MAP
	#ifdef SEPARATE_OPACITY
		vec2 opacityMapValue = texture2D(opacityTexture, derivedCoords).rg;
	#else
		vec2 opacityMapValue = normalRead.ba;
	#endif //SEPARATE_OPACITY
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