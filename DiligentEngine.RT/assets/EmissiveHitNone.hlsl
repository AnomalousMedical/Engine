
#include "Structures.hlsl"
#include "RayUtils_OldShim.hlsl"

//This is a shader for an object with no emissive materials
//It just returns black

[shader("closesthit")]
void main(inout EmissiveRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    payload.Color = float3(0.0, 0.0, 0.0);
}
