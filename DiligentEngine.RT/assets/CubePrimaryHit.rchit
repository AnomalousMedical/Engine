
#include "structures.fxh"
#include "RayUtils.fxh"

StructuredBuffer<CubeAttribVertex> g_Vertices;
StructuredBuffer<uint> g_Indices;

Texture2D    g_CubeTextures[NUM_TEXTURES];
Texture2D    g_CubeNormalTextures[NUM_TEXTURES];
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = g_Vertices[g_Indices[vertId + 0]];
    CubeAttribVertex posY = g_Vertices[g_Indices[vertId + 1]];
    CubeAttribVertex posZ = g_Vertices[g_Indices[vertId + 2]];

    // Calculate texture coordinates.
    float2 uv = posX.uv.xy * barycentrics.x +
                posY.uv.xy * barycentrics.y +
                posZ.uv.xy * barycentrics.z;

    // Calculate vertex tangent.
    float3 tangent = posX.tangent.xyz * barycentrics.x +
                     posY.tangent.xyz * barycentrics.y +
                     posZ.tangent.xyz * barycentrics.z;
    
    // Calculate vertex binormal.
    float3 binormal = posX.binormal.xyz * barycentrics.x +
                      posY.binormal.xyz * barycentrics.y +
                      posZ.binormal.xyz * barycentrics.z;
    
    // Calculate vertex normal.
    float3 normal = posX.normal.xyz * barycentrics.x +
                    posY.normal.xyz * barycentrics.y +
                    posZ.normal.xyz * barycentrics.z;

    //Get Mapped normal
    float3 pertNormal = g_CubeNormalTextures[InstanceID()].SampleLevel(g_SamLinearWrap, uv, 0).rgb * float3(2.0, 2.0, 2.0) - float3(1.0, 1.0, 1.0);
    float3x3 tbn = MatrixFromRows(tangent, binormal, normal);
    pertNormal = normalize(mul(pertNormal, tbn)); //Can probably skip this normalize
    
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
