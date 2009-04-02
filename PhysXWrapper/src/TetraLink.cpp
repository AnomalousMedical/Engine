#include "StdAfx.h"
#include "..\include\TetraLink.h"
#include "MeshHash.h"
#include "TetraMesh.h"
#include "NxBounds3.h"
#include "MathUtil.h"

namespace PhysXWrapper
{

static void barycentricCoords(const NxVec3 &p0, const NxVec3 &p1,const NxVec3 &p2, const NxVec3 &p3,const NxVec3 &p, NxVec3 &barycentricCoords)
{
	NxVec3 q  = p-p3;
	NxVec3 q0 = p0-p3;
	NxVec3 q1 = p1-p3;
	NxVec3 q2 = p2-p3;

	NxMat33 m;
	m.setColumn(0,q0);
	m.setColumn(1,q1);
	m.setColumn(2,q2);

	NxReal det = m.determinant();

	m.setColumn(0, q);
	barycentricCoords.x = m.determinant();

	m.setColumn(0, q0); m.setColumn(1,q);
	barycentricCoords.y = m.determinant();

	m.setColumn(1, q1); m.setColumn(2,q);
	barycentricCoords.z = m.determinant();

	if (det != 0.0f)
	{
		barycentricCoords /= det;
	}
}

static void doBuildLinks(NxBounds3& tetraMeshBounds, NxArray<NxU32>& tetraMeshIndices, NxArray<NxVec3>& tetraMeshVertices, NxArray<NxVec3>& graphicsMeshVertices, TetraLink* tetraLinks)
{
	MeshHash hash;

	// hash tetrahedra for faster search
	hash.setGridSpacing(tetraMeshBounds.min.distance(tetraMeshBounds.max) * 0.1f);

	NxU32 tcount = tetraMeshIndices.size()/4;

	NxU32 *idx = &tetraMeshIndices[0];

	NxU32 index = 0;

	NxArray< NxU32 > newIndices;

	for (NxU32 i = 0; i < tcount; i++)
	{

		NxU32 i1 = *idx++;
		NxU32 i2 = *idx++;
		NxU32 i3 = *idx++;
		NxU32 i4 = *idx++;

		//if ( !mDeletions || mDeletions[i] == 0 )
		//{

			newIndices.push_back(i1);
			newIndices.push_back(i2);
			newIndices.push_back(i3);
			newIndices.push_back(i4);

			NxBounds3 tetraBounds;

			tetraBounds.setEmpty();
			tetraBounds.include(tetraMeshVertices[i1]);
			tetraBounds.include(tetraMeshVertices[i2]);
			tetraBounds.include(tetraMeshVertices[i3]);
			tetraBounds.include(tetraMeshVertices[i4]);

			hash.add(tetraBounds, index);

			index++;
		//}
	}

	tcount = newIndices.size()/4;



	NxU32 vcount = graphicsMeshVertices.size();

	//Scan each vertex.
	for (NxU32 i = 0; i < vcount; i++)
	{
		NxVec3 pos = graphicsMeshVertices[i];

		NxArray<int> itemIndices;

		hash.queryUnique(pos, itemIndices);

		NxReal minDist = 0.0f;
		NxVec3 b;
		int isize,num;
		isize = num = (int)itemIndices.size();
		if (num == 0)
			num = tcount;

		NxU32 *indices = &newIndices[0];
		
		//Check each vertex returned to see if it is in the tetrahedron.
		for (int i = 0; i < num; i++)
		{
			int j = i;

			if ( isize > 0)
			{
				j = itemIndices[i];
			}

			NxU32 *idx = &indices[j*4];

			NxU32 i1 = *idx++;
			NxU32 i2 = *idx++;
			NxU32 i3 = *idx++;
			NxU32 i4 = *idx++;

			NxVec3 &p0 = tetraMeshVertices[i1];
			NxVec3 &p1 = tetraMeshVertices[i2];
			NxVec3 &p2 = tetraMeshVertices[i3];
			NxVec3 &p3 = tetraMeshVertices[i4];

			NxVec3 b;

			barycentricCoords(p0, p1, p2, p3, pos, b);

			// is the vertex inside the tetrahedron? If yes we take it
			if (b.x >= 0.0f && b.y >= 0.0f && b.z >= 0.0f && (b.x + b.y + b.z) <= 1.0f)
			{
				tetraLinks->barycentricCoord = MathUtil::copyVector3(b);
				tetraLinks->tetraIndex = j;
				break;
			}

			// otherwise, if we are not in any tetrahedron we take the closest one.
			// start by computing the distance
			NxReal dist = 0.0f;
			if (b.x + b.y + b.z > 1.0f)
			{
				dist = b.x + b.y + b.z - 1.0f;
			}
			if (b.x < 0.0f)
			{
				dist = (-b.x < dist) ? dist : -b.x;
			}
			if (b.y < 0.0f)
			{
				dist = (-b.y < dist) ? dist : -b.y;
			}
			if (b.z < 0.0f)
			{
				dist = (-b.z < dist) ? dist : -b.z;
			}

			// If this is the first element or the distance is closer set this tetrahedron as the tetrahedron.
			if (i == 0 || dist < minDist)
			{
				minDist = dist;
				tetraLinks->barycentricCoord = MathUtil::copyVector3(b);
				tetraLinks->tetraIndex = j;
			}
		}
		++tetraLinks; //Increment to next tetra link
	}
}

void TetraLink::buildLinks(TetraMesh^ graphicsMesh, TetraMesh^ tetraMesh, TetraLink* tetraLinks)
{
	NxBounds3 tetraMeshBounds;
	NxArray<NxU32> tetraMeshIndices;
	NxArray<NxVec3> tetraMeshVertices;
	NxArray<NxVec3> graphicsMeshVertices;

	//compute bounds
	float* vertex = tetraMesh->Vertices;
	for(unsigned int i = 0; i < tetraMesh->VertexCount; ++i)
	{
		NxVec3 v(vertex);
		tetraMeshBounds.include(v);
		tetraMeshVertices.push_back(v);
		vertex += 3;
	}

	NxU32* indices = tetraMesh->Indices;
	NxU32 numIndices = tetraMesh->TriangleCount * 4;
	for (NxU32 i=0; i < numIndices; i++)
	{
		tetraMeshIndices.push_back( *indices++ );
	}

	vertex = graphicsMesh->Vertices;
	for(unsigned int i = 0; i < graphicsMesh->VertexCount; ++i)
	{
		NxVec3 v(vertex);
		graphicsMeshVertices.push_back(v);
		vertex += 3;
	}

	doBuildLinks(tetraMeshBounds, tetraMeshIndices, tetraMeshVertices, graphicsMeshVertices, tetraLinks);
}

}