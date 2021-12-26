
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
             PRIMARY_RAY_INDEX,  // Miss shader index
             ray,
             payload);

    return payload;
}

// Calculate perpendicular to specified direction.
void GetRayPerpendicular(float3 dir, out float3 outLeft, out float3 outUp)
{
    const float3 a    = abs(dir);
    const float2 c    = float2(1.0, 0.0);
    const float3 axis = a.x < a.y ? (a.x < a.z ? c.xyy : c.yyx) :
                                    (a.y < a.z ? c.xyx : c.yyx);
    outLeft = normalize(cross(dir, axis));
    outUp   = normalize(cross(dir, outLeft));
}

// Returns a ray within a cone.
float3 DirectionWithinCone(float3 dir, float2 offset)
{
    float3 left, up;
    GetRayPerpendicular(dir, left, up);
    return normalize(dir + left * offset.x + up * offset.y);
}

// Calculate lighting.
void LightingPass(inout float3 Color, float3 Pos, float3 Norm, float3 pertbNorm, uint Recursion)
{
    RayDesc ray;
    float3  col = float3(0.0, 0.0, 0.0);

    // Add a small offset to avoid self-intersections.
    ray.Origin = Pos + Norm * SMALL_OFFSET;
    ray.TMin   = 0.0;

    for (int i = 0; i < NUM_LIGHTS; ++i)
    {
        // Limit max ray length by distance to light source.
        ray.TMax = distance(g_ConstantsCB.LightPos[i].xyz, Pos) * 1.01;

        float3 rayDir = normalize(g_ConstantsCB.LightPos[i].xyz - Pos);
        float  NdotL   = max(0.0, dot(pertbNorm, rayDir));

        // Optimization - don't trace rays if NdotL is zero or negative
        if (NdotL > 0.0)
        {
            // Cast multiple rays that are distributed within a cone.
            int   PCFSamples = Recursion > 1 ? min(1, g_ConstantsCB.ShadowPCF) : g_ConstantsCB.ShadowPCF;
            float shading    = 0.0;
            for (int j = 0; j < PCFSamples; ++j)
            {
                float2 offset = float2(g_ConstantsCB.DiscPoints[j / 2][(j % 2) * 2], g_ConstantsCB.DiscPoints[j / 2][(j % 2) * 2 + 1]);
                ray.Direction = DirectionWithinCone(rayDir, offset * 0.005);
                shading       += saturate(CastShadow(ray, Recursion).Shading);
            }
            
            shading = PCFSamples > 0 ? shading / float(PCFSamples) : 1.0;

            col += Color * g_ConstantsCB.LightColor[i].rgb * NdotL * shading;
        }
        col += Color * g_ConstantsCB.Darkness;
    }
    Color = col * (1.0 / float(NUM_LIGHTS)) + g_ConstantsCB.AmbientColor.rgb;
}

void AnyHitOpacityMapUV(float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D opacityTexture, SamplerState mySampler, float2 uv)
{
    int mip = GetMip(RayTCurrent());

    float opacity = opacityTexture.SampleLevel(mySampler, uv, mip).a;

    if (opacity < 0.5f) {
        IgnoreHit();
    }

    //Doing nothing lets the ray tracer continue as normal
}

void AnyHitOpacityMap(float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D opacityTexture, SamplerState mySampler)
{
    // Calculate texture coordinates.
    float2 uv = posX.uv.xy * barycentrics.x +
        posY.uv.xy * barycentrics.y +
        posZ.uv.xy * barycentrics.z;

    AnyHitOpacityMapUV(barycentrics,
        posX, posY, posZ,
        opacityTexture, mySampler, uv);
}

float3 GetPerterbedNormal(
    float3 barycentrics, float3 normal,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
     Texture2D normalTexture, SamplerState normalSampler,
    float2 uv, float mip
)
{
    // Calculate vertex tangent.
    float3 tangent = posX.tangent.xyz * barycentrics.x +
        posY.tangent.xyz * barycentrics.y +
        posZ.tangent.xyz * barycentrics.z;

    // Calculate vertex binormal.
    float3 binormal = posX.binormal.xyz * barycentrics.x +
        posY.binormal.xyz * barycentrics.y +
        posZ.binormal.xyz * barycentrics.z;

    //Get Mapped normal
    float3 pertNormal = normalTexture.SampleLevel(normalSampler, uv, mip).rgb * float3(2.0, 2.0, 2.0) - float3(1.0, 1.0, 1.0);
    float3x3 tbn = MatrixFromRows(tangent, binormal, normal);
    pertNormal = normalize(mul(pertNormal, tbn)); //Can probably skip this normalize

    return pertNormal;
}

void LightAndShadeUV
(
inout PrimaryRayPayload payload, float3 barycentrics, 
CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ, 
Texture2D colorTexture, Texture2D normalTexture, SamplerState colorSampler, SamplerState normalSampler,
float2 uv
)
{
    payload.Depth = RayTCurrent();

    int mip = GetMip(payload.Depth);

    // Calculate vertex normal.
    float3 normal = posX.normal.xyz * barycentrics.x +
        posY.normal.xyz * barycentrics.y +
        posZ.normal.xyz * barycentrics.z;

    //Get Mapped normal
    float3 pertNormal = GetPerterbedNormal(barycentrics, normal,
        posX, posY, posZ,
        normalTexture, normalSampler,
        uv, mip);
    
    //Convert to world space
    normal = normalize(mul((float3x3) ObjectToWorld3x4(), normal));
    pertNormal = normalize(mul((float3x3) ObjectToWorld3x4(), pertNormal));
    
    // Sample texturing.
    payload.Color = colorTexture.SampleLevel(colorSampler, uv, mip).rgb;
    
    // Apply lighting.
    float3 rayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
    LightingPass(payload.Color, rayOrigin, normal, pertNormal, payload.Recursion + 1);
}

void LightAndShadeUVColorOnly(
    inout PrimaryRayPayload payload, float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D colorTexture, SamplerState colorSampler,
    float2 uv)
{
    payload.Depth = RayTCurrent();

    int mip = GetMip(payload.Depth);

    // Calculate vertex normal.
    float3 normal = posX.normal.xyz * barycentrics.x +
        posY.normal.xyz * barycentrics.y +
        posZ.normal.xyz * barycentrics.z;

    //Convert to world space
    normal = normalize(mul((float3x3) ObjectToWorld3x4(), normal));

    // Sample texturing.
    payload.Color = colorTexture.SampleLevel(colorSampler, uv, mip).rgb;

    // Apply lighting.
    float3 rayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
    LightingPass(payload.Color, rayOrigin, normal, normal, payload.Recursion + 1);
}

void LightAndShade(
    inout PrimaryRayPayload payload, float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D colorTexture, Texture2D normalTexture, SamplerState colorSampler, SamplerState normalSampler)
{

    // Calculate texture coordinates.
    float2 uv = posX.uv.xy * barycentrics.x +
        posY.uv.xy * barycentrics.y +
        posZ.uv.xy * barycentrics.z;

    LightAndShadeUV(payload, barycentrics,
        posX, posY, posZ,
        colorTexture, normalTexture, colorSampler, normalSampler,
        uv);
}

void LightAndShadeShinyUV
(
    inout PrimaryRayPayload payload, float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D colorTexture, Texture2D normalTexture, Texture2D physicalTexture,
    SamplerState colorSampler, SamplerState normalSampler,
    float2 uv
)
{
    payload.Depth = RayTCurrent();

    int mip = GetMip(payload.Depth);

    // Calculate vertex normal.
    float3 normal = posX.normal.xyz * barycentrics.x +
        posY.normal.xyz * barycentrics.y +
        posZ.normal.xyz * barycentrics.z;

    //Get Mapped normal
    float3 pertNormal = GetPerterbedNormal(barycentrics, normal,
        posX, posY, posZ,
        normalTexture, normalSampler,
        uv, mip);

    //Convert to world space
    normal = normalize(mul((float3x3) ObjectToWorld3x4(), normal));
    pertNormal = normalize(mul((float3x3) ObjectToWorld3x4(), pertNormal));


    // Reflect normal.
    float3 rayDir = reflect(WorldRayDirection(), pertNormal);

    RayDesc ray;
    ray.Origin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent() + normal * SMALL_OFFSET;
    ray.TMin = 0.0;
    ray.TMax = 100.0;

    // Cast multiple rays that are distributed within a cone.
    float3    color = float3(0.0, 0.0, 0.0);
    const int ReflBlur = payload.Recursion > 1 ? 1 : 1;// g_ConstantsCB.SphereReflectionBlur;, can use roughness texture here
    for (int j = 0; j < ReflBlur; ++j)
    {
        float2 offset = float2(g_ConstantsCB.DiscPoints[j / 2][(j % 2) * 2], g_ConstantsCB.DiscPoints[j / 2][(j % 2) * 2 + 1]);
        ray.Direction = DirectionWithinCone(rayDir, offset * 0.01);
        color += CastPrimaryRay(ray, payload.Recursion + 1).Color;
    }

    color /= float(ReflBlur);

    // Sample texturing.
    float3 texColor = colorTexture.SampleLevel(colorSampler, uv, mip).rgb;

    float reflectivity = 0.3f;

    payload.Color = texColor * (1.0f - reflectivity) + color * reflectivity;

    // Apply lighting.
    float3 rayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
    LightingPass(payload.Color, rayOrigin, normal, pertNormal, payload.Recursion + 1);

    payload.Depth = RayTCurrent();
}

void LightAndShadeShiny
(
    inout PrimaryRayPayload payload, float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D colorTexture, Texture2D normalTexture, Texture2D physicalTexture, 
    SamplerState colorSampler, SamplerState normalSampler
)
{
    // Calculate texture coordinates.
    float2 uv = posX.uv.xy * barycentrics.x +
        posY.uv.xy * barycentrics.y +
        posZ.uv.xy * barycentrics.z;

    LightAndShadeShinyUV(payload, barycentrics,
        posX, posY, posZ,
        colorTexture, normalTexture, physicalTexture,
        colorSampler, normalSampler,
        uv);
}