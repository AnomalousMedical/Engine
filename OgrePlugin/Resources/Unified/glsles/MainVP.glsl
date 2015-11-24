precision highp float;

uniform mat4 cameraMatrix;			//The matrix for the final camera transform, either view projection (hardware skinning) or world view projection
uniform vec3 eyePosition;			//The eye position the coordinate space should match the eyePosition space (object or world)
uniform vec4 lightPosition;			//The position of the light the coordinate space should match the eyePosition space (object or world)
uniform vec4 lightAttenuation;		//The attenuation of the light

//Input
attribute vec4 vertex; //Vertex
attribute vec3 normal; //Normal

#ifndef NO_MAPS
	attribute vec2 uv0;			//Uv Coord
	#ifdef PARITY
		attribute vec4 tangent; //Tangent
	#else
		attribute vec3 tangent; //Tangent
	#endif
	attribute vec3 binormal;	//Binormal
#endif //NO_MAPS

//Pose Animation
#if POSE_COUNT > 0
	attribute vec3 uv1; //Pose1Pos
	#if POSE_COUNT > 1
		attribute vec3 uv2; //Pose2Pos
	#endif
	uniform vec4 poseAnimAmount;
#endif

#if BONES_PER_VERTEX > 0
	uniform vec4 worldMatrix3x4Array[180];		//This is an array of bones, the index is the maximum amount of bones supported * 3
	attribute vec4 blendIndices;				//The blend indices
	attribute vec4 blendWeights;				//The blend weights
#endif //BONES_PER_VERTEX

//Output
#ifdef NO_MAPS
	varying vec3 passNormal;	//Output normal
#else
	varying vec2 texCoords;		//Output Texture coords
#endif //NO_MAPS
varying vec3 lightVector;		//Light vector in tangent space
varying vec3 halfVector;		//Eye vector in tangent space
varying vec4 attenuation;		 //Attenuation per vertex

//External Functions
//Pack function packs values from -1 to 1 to 0 and 1
vec3 pack(vec3 packValue)
{
	return 0.5 * packValue + 0.5;
}

//----------------------------------
//Shared Vertex Program
//----------------------------------
void main(void)
{
	vec4 localVertex = vertex;
	vec3 localNormal = normal;
	#ifndef NO_MAPS
		vec3 localTangent = tangent.xyz;
	#endif //NO_MAPS

	//Hardware Pose Animation
	#if POSE_COUNT == 1
		localVertex.xyz = localVertex.xyz + poseAnimAmount.x * uv1;
	#else //elsif not working, so do it this way instead
		#if POSE_COUNT == 2
			localVertex.xyz = localVertex.xyz + poseAnimAmount.x * uv1 + poseAnimAmount.y * uv2;
		#endif
	#endif

	#if BONES_PER_VERTEX > 0
		//Hardware Skinning
		vec4 blendPos = vec4(0,0,0,0);
		vec3 newNormal = vec3(0,0,0);
		#ifndef NO_MAPS
			vec3 newTangent = vec3(0,0,0);
		#endif //NO_MAPS
		//-----------Skinning Unrolled Loop--------------
		int idx;
		mat4 worldMatrix;
		float weight;
		mat3 worldMatrixRot; //Rotation only matrix, prevents translation from screwing up normals

		//First Bone
		#if BONES_PER_VERTEX > 0
			idx = int(blendIndices[0]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[0];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((localVertex * worldMatrix).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
			#ifndef NO_MAPS
				newTangent += (worldMatrixRot * tangent.xyz) * weight;
			#endif //NO_MAPS
		#endif //BONES_PER_VERTEX > 0

		//Second Bone
		#if BONES_PER_VERTEX > 1
			idx = int(blendIndices[1]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[1];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((localVertex * worldMatrix).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
			#ifndef NO_MAPS
				newTangent += (worldMatrixRot * tangent.xyz) * weight;
			#endif //NO_MAPS
		#endif //BONES_PER_VERTEX > 1

		//Third Bone
		#if BONES_PER_VERTEX > 2
			idx = int(blendIndices[2]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[2];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((localVertex * worldMatrix).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
			#ifndef NO_MAPS
				newTangent += (worldMatrixRot * tangent.xyz) * weight;
			#endif //NO_MAPS
		#endif //BONES_PER_VERTEX > 2

		//Fourth Bone
		#if BONES_PER_VERTEX > 3
			idx = int(blendIndices[3]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[3];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((localVertex * worldMatrix).xyz, 1.0) * weight;
			newNormal += (worldMatrixRot * normal) * weight;
			#ifndef NO_MAPS
				newTangent += (worldMatrixRot * tangent.xyz) * weight;
			#endif //NO_MAPS
		#endif //BONES_PER_VERTEX > 3
		//---------------End Skinning Unrolled Loop------------------

		localVertex = blendPos;
		localNormal = normalize(newNormal);
		#ifndef NO_MAPS
			localTangent = normalize(newTangent);
			vec3 newBinormal = cross(newTangent.xyz, newNormal.xyz);
		#endif //NO_MAPS
	#endif //BONES_PER_VERTEX > 0

	#ifdef NO_MAPS
		//Calculate the local light vector
		lightVector = lightPosition.xyz;
	
		//Calculate the light attenuation, ax^2 + bx + c
		float dist = length(lightVector);
		attenuation = vec4(1.0 / max((lightAttenuation.y + (dist * lightAttenuation.z) + (dist * dist * lightAttenuation.w)), 1.0));

		//Calculate the half vector
		halfVector = eyePosition + lightVector;
	
		//Copy the texture coords
		passNormal = localNormal;

	#else //NO_MAPS
		//Tangent space conversion matrix
		#ifdef PARITY
			mat3 TBNMatrix = mat3(tangent.xyz, binormal * tangent.w, normal);
		#else
			mat3 TBNMatrix = mat3(tangent, binormal, normal);
		#endif

		//Calculate the local light vector
		lightVector = lightPosition.xyz - localVertex.xyz;
	
		//Calculate the light attenuation, ax^2 + bx + c
		float dist = length(lightVector);
		attenuation = vec4(1.0 / max((lightAttenuation.y + (dist * lightAttenuation.z) + (dist * dist * lightAttenuation.w)), 1.0));

		//Calculate the half vector
		lightVector = normalize(lightVector);
		halfVector = normalize(eyePosition - localVertex.xyz) + lightVector;

		//Transform the light vector from object space into tangent space
		lightVector = lightVector * TBNMatrix;
		lightVector = pack(lightVector);
	
		//Transform the eye vector to tangent space
		halfVector = halfVector * TBNMatrix;
		halfVector = pack(halfVector);
	
		//Copy the texture coords
		texCoords = uv0;
	#endif //NO_MAPS

	// Transform the current vertex from object space to clip space
	gl_Position = cameraMatrix * localVertex;
}