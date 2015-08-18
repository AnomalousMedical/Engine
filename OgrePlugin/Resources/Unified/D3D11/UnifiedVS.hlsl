//----------------------------------
// This file contains our mega shader. You can control how it works with some preprocessor defines
//
// PARITY - Define this to turn on parity for the shader.
// BONES_PER_VERTEX - Set to 1, 2, 3, or 4 depending on how many bones can effect each vertex
// POSE_COUNT - Set to 1 or 2 depending on how many poses you want to hardware skin
//
//----------------------------------

//Application to vertex program
struct a2v
{
	//Input position
	float4 position : POSITION;

	//Input normal
	float3 normal : NORMAL0;

#ifndef NO_MAPS
	//Input texture coords
	float2 texCoords : TEXCOORD0;

	//Input tangent
	#ifdef PARITY
		float4 tangent : TANGENT0;
	#else
		float3 tangent : TANGENT0;
	#endif

	//Input binormal
	float3 binormal : BINORMAL0;
#endif
};

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

//Pack function packs values from -1 to 1 to 0 and 1
float3 pack(float3 input)
{
	return 0.5 * input + 0.5;
}

//-------------------------------------------------------------------------------------------------

#ifdef NO_MAPS

void computeNoTextureOutput(float4 position,
							float3 normal, 
							float4x4 worldViewProj,
							float3 eyePosition,
							float4 lightAttenuation,
							float4 lightPosition,
							out v2f output)
{
	//Calculate the local light vector
	output.lightVector = lightPosition.xyz;
	
	//Calculate the light attenuation, ax^2 + bx + c
	float dist = length(output.lightVector);
	output.attenuation = 1 / max((lightAttenuation.y + (dist * lightAttenuation.z) + (dist * dist * lightAttenuation.w)), 1);

	//Calculate the half vector
	output.halfVector = eyePosition + output.lightVector;
	
	//Copy the texture coords
	output.normal = normal;

	// Transform the current vertex from object space to clip space
	output.position = mul(worldViewProj, position);
}

#else //NO_MAPS

void computeOutput( float3x3 TBNMatrix, 
				    float4 position,
				    float2 texCoords, 
				    float4x4 worldViewProj,
					float3 eyePosition,
					float4 lightAttenuation,
					float4 lightPosition,
					out v2f output
					)
{
	//Calculate the local light vector
	output.lightVector = lightPosition.xyz - position.xyz;
	
	//Calculate the light attenuation, ax^2 + bx + c
	float dist = length(output.lightVector);
	output.attenuation = 1 / max((lightAttenuation.y + (dist * lightAttenuation.z) + (dist * dist * lightAttenuation.w)), 1);

	//Calculate the half vector
	output.lightVector = normalize(output.lightVector);
	output.halfVector = normalize(eyePosition - position.xyz) + output.lightVector;

	//Transform the light vector from object space into tangent space
	output.lightVector = mul(TBNMatrix, output.lightVector);
	output.lightVector = pack(output.lightVector);
	
	//Transform the eye vector to tangent space
	output.halfVector = mul(TBNMatrix, output.halfVector);
	output.halfVector = pack(output.halfVector);
	
	//Copy the texture coords
	output.texCoords = texCoords;

	// Transform the current vertex from object space to clip space
	output.position = mul(worldViewProj, position);
}

#endif //NO_MAPS

//----------------------------------
//Shared Vertex Program
//----------------------------------
void mainVP
(
	uniform float4 lightAttenuation,		//The attenuation of the light
	const uniform float3 eyePosition,		//The eye position the coordinate space should match the eyePosition space (object or world)
	const uniform float4 lightPosition,		//The position of the light the coordinate space should match the eyePosition space (object or world)
	const uniform float4x4 cameraMatrix,	//The matrix for the final camera transform, either view projection (hardware skinning) or world view projection

	#if BONES_PER_VERTEX > 0
		int4 blendIdx : BLENDINDICES,
		float4 blendWgt : BLENDWEIGHT,
		const uniform float3x4 worldMatrix3x4Array[60], //This is an array of bones, the index is the maximum amount of bones supported
	#endif //BONES_PER_VERTEX

	#if POSE_COUNT > 0
		float3 pose1pos  : TEXCOORD1,
		#if POSE_COUNT > 1
			float3 pose2pos  : TEXCOORD2,
		#endif
		uniform float4 poseAnimAmount,
	#endif

	in a2v input,
	out v2f output
)
{
	//Hardware Pose Animation
	#if POSE_COUNT == 1
		input.position.xyz = input.position.xyz + poseAnimAmount.x * pose1pos;
	#elif POSE_COUNT == 2
		input.position.xyz = input.position.xyz + poseAnimAmount.x * pose1pos + poseAnimAmount.y * pose2pos;
	#endif

	//Hardware Skinning
	#if BONES_PER_VERTEX > 0
		float4 blendPos = float4(0,0,0,0);
		float3 newNormal = float3(0,0,0);
		#ifndef NO_MAPS
			float3 newTangent = float3(0,0,0);
		#endif
		//-----------Skinning Unrolled Loop--------------
			float3x4 worldMatrix;
			float weight;

			//First Bone
			#if BONES_PER_VERTEX > 0
				worldMatrix = worldMatrix3x4Array[blendIdx[0]];
				weight = blendWgt[0];

				blendPos += float4(mul(worldMatrix, input.position).xyz, 1.0) * weight;
				newNormal += mul((float3x3)worldMatrix, input.normal) * weight;
				#ifndef NO_MAPS
					newTangent += mul((float3x3)worldMatrix, input.tangent.xyz) * weight;
				#endif //NO_MAPS
			#endif //BONES_PER_VERTEX > 0

			//Second Bone
			#if BONES_PER_VERTEX > 1
				worldMatrix = worldMatrix3x4Array[blendIdx[1]];
				weight = blendWgt[1];

				blendPos += float4(mul(worldMatrix, input.position).xyz, 1.0) * weight;
				newNormal += mul((float3x3)worldMatrix, input.normal) * weight;
				#ifndef NO_MAPS
					newTangent += mul((float3x3)worldMatrix, input.tangent.xyz) * weight;
				#endif //NO_MAPS
			#endif //BONES_PER_VERTEX > 1

			//Third Bone
			#if BONES_PER_VERTEX > 2
				worldMatrix = worldMatrix3x4Array[blendIdx[2]];
				weight = blendWgt[2];

				blendPos += float4(mul(worldMatrix, input.position).xyz, 1.0) * weight;
				newNormal += mul((float3x3)worldMatrix, input.normal) * weight;
				#ifndef NO_MAPS
					newTangent += mul((float3x3)worldMatrix, input.tangent.xyz) * weight;
				#endif //NO_MAPS
			#endif //BONES_PER_VERTEX > 2

			//Fourth Bone
			#if BONES_PER_VERTEX > 3
				worldMatrix = worldMatrix3x4Array[blendIdx[3]];
				weight = blendWgt[3];

				blendPos += float4(mul(worldMatrix, input.position).xyz, 1.0) * weight;
				newNormal += mul((float3x3)worldMatrix, input.normal) * weight;
				#ifndef NO_MAPS
					newTangent += mul((float3x3)worldMatrix, input.tangent.xyz) * weight;
				#endif //NO_MAPS
			#endif //BONES_PER_VERTEX > 3
		//---------------End Skinning Unrolled Loop------------------
		input.position = blendPos;
		input.normal = normalize(newNormal);
		#ifndef NO_MAPS
			input.tangent.xyz = normalize(newTangent).xyz;
			input.binormal = cross(newTangent, newNormal);
		#endif //NO_MAPS
	#endif //BONES_PER_VERTEX > 0

	#ifdef NO_MAPS
		computeNoTextureOutput(input.position, input.normal, cameraMatrix, eyePosition, lightAttenuation, lightPosition, output);
	#else //NO_MAPS
		//Tangent space conversion matrix
		#ifdef PARITY
			float3x3 TBNMatrix = float3x3(input.tangent.xyz, input.binormal * input.tangent.w, input.normal);
		#else
			float3x3 TBNMatrix = float3x3(input.tangent, input.binormal, input.normal);
		#endif

		computeOutput(TBNMatrix, input.position, input.texCoords, cameraMatrix, eyePosition, lightAttenuation, lightPosition, output);
	#endif //NO_MAPS
}