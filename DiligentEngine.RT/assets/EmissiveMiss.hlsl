
#include "Structures.hlsl"

[shader("miss")]
void main(inout EmissiveRayPayload payload)
{
	// Set 0 on hit and 1 otherwise.
	payload.Color = float3(0.0, 0.0, 0.0);
}
