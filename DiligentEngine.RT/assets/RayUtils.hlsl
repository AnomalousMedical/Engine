#ifndef PI
#   define  PI 3.141592653589793
#endif

RaytracingAccelerationStructure g_TLAS;
ConstantBuffer<Constants>       g_ConstantsCB;

int GetMip(float depth)
{
    //Lame mip calculation, but looks tons better than just mip0.
    //Need to add screen size and some more info
    int mip = min(depth / 4, 4);
    return mip;
}

PrimaryRayPayload CastPrimaryRay(RayDesc ray, uint Recursion)
{
    PrimaryRayPayload payload = {float3(0, 0, 0), 0.0, Recursion};

    // Manually terminate the recusrion as the driver doesn't check the recursion depth.
    if (Recursion >= g_ConstantsCB.MaxRecursion)
    {
        // set pink color for debugging
        //payload.Color = float3(0.95, 0.18, 0.95);
        return payload;
    }
    TraceRay(g_TLAS,            // Acceleration structure
             RAY_FLAG_NONE,
             ~0,                // Instance inclusion mask - all instances are visible
             PRIMARY_RAY_INDEX, // Ray contribution to hit group index (aka ray type)
             HIT_GROUP_STRIDE,  // Multiplier for geometry contribution to hit
                                // group index (aka the number of ray types)
             PRIMARY_RAY_INDEX, // Miss shader index
             ray,
             payload);
    return payload;
}

ShadowRayPayload CastShadow(RayDesc ray, uint Recursion)
{
    // By default initialize Shading with 0.
    // With RAY_FLAG_SKIP_CLOSEST_HIT_SHADER, only intersection and any-hit shaders are executed.
    // Any-hit shader is not used in this tutorial, intersection shader can not write to payload, 
    // so on intersection payload. Shading is always 0,
    // on miss shader payload.Shading will be initialized with 1.
    // With this flags shadow casting executed as fast as possible.
    ShadowRayPayload payload = {0.0, Recursion};
    
    // Manually terminate the recusrion as the driver doesn't check the recursion depth.
    if (Recursion >= g_ConstantsCB.MaxRecursion)
    {
        payload.Shading = 1.0;
        return payload;
    }
    //This was modified to use PRIMARY_RAY_INDEX, which makes the any hit opacity shaders work
    TraceRay(g_TLAS,            // Acceleration structure
             RAY_FLAG_SKIP_CLOSEST_HIT_SHADER | RAY_FLAG_ACCEPT_FIRST_HIT_AND_END_SEARCH,
             OPAQUE_GEOM_MASK,  // Instance inclusion mask - only opaque instances are visible
             PRIMARY_RAY_INDEX,  // Ray contribution to hit group index (aka ray type)
             HIT_GROUP_STRIDE,  // Multiplier for geometry contribution to hit 
                                // group index (aka the number of ray types)
             SHADOW_RAY_INDEX,  // Miss shader index
             ray,
             payload);

    return payload;
}

float3 GetNearbyEmissiveLighting(RayDesc ray, uint Recursion)
{
    EmissiveRayPayload payload = { float3(0.0, 0.0, 0.0), Recursion };

    // Manually terminate the recusrion as the driver doesn't check the recursion depth.
    if (Recursion >= g_ConstantsCB.MaxRecursion)
    {
        payload.Color = float3(0.0, 0.0, 0.0);
        return payload.Color;
    }
    //This was modified to use PRIMARY_RAY_INDEX, which makes the any hit opacity shaders work
    TraceRay(g_TLAS,            // Acceleration structure
        RAY_FLAG_NONE,
        OPAQUE_GEOM_MASK,  // Instance inclusion mask - only opaque instances are visible
        EMISSIVE_RAY_INDEX,  // Ray contribution to hit group index (aka ray type)
        HIT_GROUP_STRIDE,  // Multiplier for geometry contribution to hit 
                           // group index (aka the number of ray types)
        EMISSIVE_RAY_INDEX,  // Miss shader index
        ray,
        payload);

    return payload.Color;
}

// Calculate perpendicular to specified direction.
void GetRayPerpendicular(float3 dir, out float3 outLeft, out float3 outUp)
{
    const float3 a = abs(dir);
    const float2 c = float2(1.0, 0.0);
    const float3 axis = a.x < a.y ? (a.x < a.z ? c.xyy : c.yyx) :
        (a.y < a.z ? c.xyx : c.yyx);
    outLeft = normalize(cross(dir, axis));
    outUp = normalize(cross(dir, outLeft));
}

// Returns a ray within a cone.
float3 DirectionWithinCone(float3 dir, float2 offset)
{
    float3 left, up;
    GetRayPerpendicular(dir, left, up);
    return normalize(dir + left * offset.x + up * offset.y);
}