precision highp float;

uniform vec4 worldMatrix3x4Array[180]; //This is an array of bones, the index is the maximum amount of bones supported
uniform mat4 viewProjectionMatrix;

attribute vec4 blendIndices;
attribute vec4 blendWeights;

//Input
attribute vec4 vertex; //Vertex
attribute vec2 uv0; //Uv Coord
//Pose Animation
#if POSE_COUNT > 0
	attribute vec3 uv1; //Pose1Pos
	#if POSE_COUNT > 1
		attribute vec3 uv2; //Pose2Pos
	#endif
	uniform vec4 poseAnimAmount;
#endif

//Output
varying vec2 texCoords;

//----------------------------------
//Shared Vertex Program
//----------------------------------
void main(void)
{
	vec4 poseVertex = vertex;
	//Hardware Pose Animation
	#if POSE_COUNT == 1
		poseVertex.xyz = poseVertex.xyz + poseAnimAmount.x * uv1;
	#else //elsif not working, so do it this way instead
		#if POSE_COUNT == 2
			poseVertex.xyz = poseVertex.xyz + poseAnimAmount.x * uv1 + poseAnimAmount.y * uv2;
		#endif
	#endif

	//Hardware Skinning
	vec4 blendPos = vec4(0,0,0,0);
	//-----------Skinning Unrolled Loop--------------
	//Remove blocks for bones that are not needed
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
        
			blendPos += vec4((poseVertex * worldMatrix).xyz, 1.0) * weight;
		#endif

		//Second Bone
		#if BONES_PER_VERTEX > 1
			idx = int(blendIndices[1]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[1];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((poseVertex * worldMatrix).xyz, 1.0) * weight;
		#endif

		//Third Bone
		#if BONES_PER_VERTEX > 2
			idx = int(blendIndices[2]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[2];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((poseVertex * worldMatrix).xyz, 1.0) * weight;
		#endif

		//Fourth Bone
		#if BONES_PER_VERTEX > 3
			idx = int(blendIndices[3]) * 3;
			worldMatrix[0] = worldMatrix3x4Array[idx];
			worldMatrix[1] = worldMatrix3x4Array[idx + 1];
			worldMatrix[2] = worldMatrix3x4Array[idx + 2];
			worldMatrix[3] = vec4(0.0, 0.0, 0.0, 1.0);
			weight = blendWeights[3];
			worldMatrixRot = mat3(worldMatrix);
        
			blendPos += vec4((poseVertex * worldMatrix).xyz, 1.0) * weight;
		#endif
	//---------------End Skinning Unrolled Loop------------------

	texCoords = uv0;
	gl_Position = viewProjectionMatrix * blendPos;
}