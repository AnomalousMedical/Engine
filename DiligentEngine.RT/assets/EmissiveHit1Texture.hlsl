
#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> $$(VERTICES);
StructuredBuffer<uint> $$(INDICES);

Texture2D    $$(EMISSIVE_TEXTURES);
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout EmissiveRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = $$(VERTICES)[$$(INDICES)[vertId + 0]];
    CubeAttribVertex posY = $$(VERTICES)[$$(INDICES)[vertId + 1]];
    CubeAttribVertex posZ = $$(VERTICES)[$$(INDICES)[vertId + 2]];

    GetEmissiveLighting
    (
        payload, barycentrics, 
        posX, posY, posZ, 
        $$(EMISSIVE_TEXTURES),
        g_SamLinearWrap
    );
}
