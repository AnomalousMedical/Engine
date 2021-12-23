
#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> $$(VERTICES);
StructuredBuffer<uint> $$(INDICES);

Texture2D    $$(COLOR_TEXTURES); //This might be better loaded independently
SamplerState g_SamLinearWrap;

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = $$(VERTICES)[$$(INDICES)[vertId + 0]];
    CubeAttribVertex posY = $$(VERTICES)[$$(INDICES)[vertId + 1]];
    CubeAttribVertex posZ = $$(VERTICES)[$$(INDICES)[vertId + 2]];

    // Sample texturing. Ray tracing shaders don't support LOD calculation, so we must specify LOD and apply filtering.
    Texture2D opacityTexture = $$(COLOR_TEXTURES);

    //-----------

    //Lame mip calculation, but looks tons better than just mip0. This needs to be an input value
    int mip = min(payload.Depth / 4, 4);

    // Calculate texture coordinates.
    float2 uv = posX.uv.xy * barycentrics.x +
        posY.uv.xy * barycentrics.y +
        posZ.uv.xy * barycentrics.z;

    float opacity = opacityTexture.SampleLevel(g_SamLinearWrap, uv, mip).a;

    if (opacity < 0.5f) {
        IgnoreHit();
    }

    //Doing nothing lets the ray tracer continue as normal
}


