#version 120

uniform vec3 worldEyePosition;		//The eye position
uniform vec4 worldLightPosition;		//The position of the light in object space
uniform vec4 lightAttenuation;		//The attenuation of the light, Should this be uniform?

uniform mat4x3 worldMatrix3x4Array[60]; //This is an array of bones, the index is the maximum amount of bones supported
uniform mat4 viewProjectionMatrix;

attribute vec4 blendIndices;
attribute vec4 blendWeights;

//Input
attribute vec4 vertex; //Vertex
attribute vec3 normal; //Normal

//Output
varying vec3 passNormal;
varying vec3 lightVector; //Light vector in tangent space
varying vec3 halfVector; //Eye vector in tangent space
varying vec4 attenuation; //Attenuation per vertex

//External Functions
vec3 pack(vec3 toPack);

//----------------------------------
//Shared Vertex Program
//----------------------------------
void main(void)
{
    //Hardware Skinning
	vec4 blendPos = vec4(0,0,0,0);
    vec3 newNormal = vec3(0,0,0);
	//-----------Skinning Unrolled Loop--------------
	//Remove blocks for bones that are not needed
        int idx;
        mat4x3 worldMatrix;
        float weight;
        mat3 worldMatrixRot; //Rotation only matrix, prevents translation from screwing up normals

		//First Bone
		#if BONES_PER_VERTEX > 0
			idx = int(blendIndices[0]);
			worldMatrix = worldMatrix3x4Array[idx];
			weight = blendWeights[0];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((worldMatrix * vertex).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
		#endif

		//Second Bone
		#if BONES_PER_VERTEX > 1
			idx = int(blendIndices[1]);
			worldMatrix = worldMatrix3x4Array[idx];
			weight = blendWeights[1];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((worldMatrix * vertex).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
		#endif

		//Third Bone
		#if BONES_PER_VERTEX > 2
			idx = int(blendIndices[2]);
			worldMatrix = worldMatrix3x4Array[idx];
			weight = blendWeights[2];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((worldMatrix * vertex).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
		#endif

		//Fourth Bone
		#if BONES_PER_VERTEX > 3
			idx = int(blendIndices[3]);
			worldMatrix = worldMatrix3x4Array[idx];
			weight = blendWeights[3];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((worldMatrix * vertex).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
		#endif
	//---------------End Skinning Unrolled Loop------------------

	newNormal = normalize(newNormal);

	//Calculate the local light vector
	lightVector = worldLightPosition.xyz;
	
	//Calculate the light attenuation, ax^2 + bx + c
	float dist = length(lightVector);
	attenuation = vec4(1.0 / max((lightAttenuation.y + (dist * lightAttenuation.z) + (dist * dist * lightAttenuation.w)), 1.0));

	//Calculate the half vector
	halfVector = worldEyePosition + lightVector;
	
	//Copy the texture coords
	passNormal = normal;

	// Transform the current vertex from object space to clip space
	gl_Position = viewProjectionMatrix * blendPos;
}