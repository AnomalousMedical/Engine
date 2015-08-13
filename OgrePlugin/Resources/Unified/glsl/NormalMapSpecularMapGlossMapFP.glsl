#version 120

//Parameters
uniform vec4 lightDiffuseColor;			//The diffuse color of the light source
uniform vec4 emissiveColor;				//The emissive color of the surface
uniform float glossyStart;
uniform float glossyRange;

//----------------------------Common----------------------------
#ifdef ALPHA
	uniform vec4 alpha;
#endif

#ifdef VIRTUAL_TEXTURE
	uniform sampler2D indirectionTex;
	uniform vec2 physicalSizeRecip;
	uniform vec2 mipBiasSize;
	uniform vec2 pagePaddingScale;
	uniform vec2 pagePaddingOffset;
#endif

//Textures
uniform sampler2D normalTexture;	//The normal map
uniform sampler2D colorTexture;  //The color map
uniform sampler2D specularTexture;  //The specular color map

//Vertex shader output
varying vec2 texCoords;
varying vec3 lightVector; //Light vector in tangent space
varying vec3 halfVector; //Eye vector in tangent space
varying vec4 attenuation; //Attenuation per vertex

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

vec2 vtexCoord(vec2 address, sampler2D indirectionTex, vec2 physicalSizeRecip, vec2 mipBiasSize, vec2 pagePaddingScale, vec2 pagePaddingOffset);

#endif

//----------------------------Main Shader----------------------------

void main()
{
	vec2 derivedCoords = texCoords;

	#ifdef VIRTUAL_TEXTURE
		derivedCoords = vtexCoord(texCoords, indirectionTex, physicalSizeRecip, mipBiasSize, pagePaddingScale, pagePaddingOffset);
	#endif

	//Get color value
	vec4 colorMap = texture2D(colorTexture, derivedCoords.xy);

	//Get the specular value
	vec4 specularMapColor = texture2D(specularTexture, derivedCoords.xy);

	//Unpack the normal map.
	vec3 normal;
#ifdef RG_NORMALS
	normal.rg = 2.0 * (texture2D(normalTexture, derivedCoords).rg - 0.5);
#else
	normal.rg = 2.0 * (texture2D(normalTexture, derivedCoords).ag - 0.5);
#endif
	normal.b = sqrt(1.0 - normal.r * normal.r - normal.g * normal.g);

	//Compute the glossyness
	float glossyness = glossyStart + glossyRange * colorMap.a;

	gl_FragColor = doLighting(unpack(lightVector), unpack(halfVector), lightDiffuseColor, attenuation, colorMap, specularMapColor, specularMapColor.a, glossyness, emissiveColor, normal);

	#ifdef ALPHA
		gl_FragColor.a = alpha.a;
	#endif
}