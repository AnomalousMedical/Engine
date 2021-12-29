
#include "Structures.hlsl"

ConstantBuffer<Constants> g_ConstantsCB;

[shader("miss")]
void main(inout PrimaryRayPayload payload)
{
    // Generate sky color.
    float factor = clamp((-WorldRayDirection().y + 0.5) / 1.5 * 4.0, 0.0, 4.0);
    int   idx = floor(factor);
    factor -= float(idx);
    float3 color = lerp(g_ConstantsCB.Pallete[idx].xyz, g_ConstantsCB.Pallete[idx + 1].xyz, factor);

    payload.Color = color;
    //payload.Depth = RayTCurrent(); // bug in DXC for SPIRV
    payload.Depth = g_ConstantsCB.ClipPlanes.y;
}
