
#include "structures.fxh"
#include "RayUtils.fxh"

ConstantBuffer<CubeAttribs>  g_CubeAttribsCB;

Texture2D    g_CubeTextures[NUM_TEXTURES];
Texture2D    g_CubeNormalTextures[NUM_TEXTURES];
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    // Get vertex indices for primitive.
    uint3 primitive = g_CubeAttribsCB.Primitives[PrimitiveIndex()].xyz;

    // Calculate texture coordinates.
    float2 uv = g_CubeAttribsCB.UVs[primitive.x].xy * barycentrics.x +
                g_CubeAttribsCB.UVs[primitive.y].xy * barycentrics.y +
                g_CubeAttribsCB.UVs[primitive.z].xy * barycentrics.z;

    // Calculate vertex tangent.
    float3 tangent = g_CubeAttribsCB.Tangents[primitive.x].xyz * barycentrics.x +
                    g_CubeAttribsCB.Tangents[primitive.y].xyz * barycentrics.y +
                    g_CubeAttribsCB.Tangents[primitive.z].xyz * barycentrics.z;

    // Calculate vertex binormal.
    float3 binormal = g_CubeAttribsCB.Binormals[primitive.x].xyz * barycentrics.x +
                    g_CubeAttribsCB.Binormals[primitive.y].xyz * barycentrics.y +
                    g_CubeAttribsCB.Binormals[primitive.z].xyz * barycentrics.z;

    // Calculate vertex normal.
    float3 normal = g_CubeAttribsCB.Normals[primitive.x].xyz * barycentrics.x +
                    g_CubeAttribsCB.Normals[primitive.y].xyz * barycentrics.y +
                    g_CubeAttribsCB.Normals[primitive.z].xyz * barycentrics.z;
    
    //Get Mapped normal
    float3 pertNormal = g_CubeNormalTextures[InstanceID()].SampleLevel(g_SamLinearWrap, uv, 0).rgb * float3(2.0, 2.0, 2.0) - float3(1.0, 1.0, 1.0);
    float3x3 tbn = MatrixFromRows(tangent, binormal, normal);
    pertNormal = normalize(mul(pertNormal, tbn));

    //Convert to world space
    normal = normalize(mul((float3x3) ObjectToWorld3x4(), normal));
    pertNormal = normalize(mul((float3x3) ObjectToWorld3x4(), pertNormal));

    // Sample texturing. Ray tracing shaders don't support LOD calculation, so we must specify LOD and apply filtering.
    payload.Color = g_CubeTextures[InstanceID()].SampleLevel(g_SamLinearWrap, uv, 0).rgb;
    payload.Depth = RayTCurrent();
    
    // Apply lighting.
    float3 rayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
    LightingPass(payload.Color, rayOrigin, normal, pertNormal, payload.Recursion + 1);
}
