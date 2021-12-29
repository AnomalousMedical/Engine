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

SurfaceReflectanceInfo GetSurfaceReflectance(   //int Workflow, //Not including workflow option, make separate function if needed
    float3     BaseColor,
    float4     PhysicalDesc
)
{
    float Metallic;

    //This is from the GLTF_PBR shader
    SurfaceReflectanceInfo SrfInfo;

    float3 specularColor;

    float3 f0 = float3(0.04, 0.04, 0.04);

    // Metallic and Roughness material properties are packed together
    // In glTF, these factors can be specified by fixed scalar values
    // or from a metallic-roughness map
    //if (Workflow == PBR_WORKFLOW_SPECULAR_GLOSINESS)
    //{
    //    SrfInfo.PerceptualRoughness = 1.0 - PhysicalDesc.a; // glossiness to roughness
    //    f0 = PhysicalDesc.rgb;

    //    // f0 = specular
    //    specularColor = f0;
    //    float oneMinusSpecularStrength = 1.0 - max(max(f0.r, f0.g), f0.b);
    //    SrfInfo.DiffuseColor = BaseColor.rgb * oneMinusSpecularStrength;

    //    // do conversion between metallic M-R and S-G metallic
    //    Metallic = GLTF_PBR_SolveMetallic(BaseColor.rgb, specularColor, oneMinusSpecularStrength);
    //}
    //else if (Workflow == PBR_WORKFLOW_METALLIC_ROUGHNESS)
    //{
        // Roughness is stored in the 'g' channel, metallic is stored in the 'b' channel.
        // This layout intentionally reserves the 'r' channel for (optional) occlusion map data
    SrfInfo.PerceptualRoughness = PhysicalDesc.g;
    Metallic = PhysicalDesc.b;

    SrfInfo.DiffuseColor = BaseColor.rgb * (float3(1.0, 1.0, 1.0) - f0) * (1.0 - Metallic);
    specularColor = lerp(f0, BaseColor.rgb, Metallic);
    //}

    //#ifdef ALPHAMODE_OPAQUE
    //    baseColor.a = 1.0;
    //#endif
    //
    //#ifdef MATERIAL_UNLIT
    //    gl_FragColor = float4(gammaCorrection(baseColor.rgb), baseColor.a);
    //    return;
    //#endif

    SrfInfo.PerceptualRoughness = clamp(SrfInfo.PerceptualRoughness, 0.0, 1.0);

    // Compute reflectance.
    float reflectance = max(max(specularColor.r, specularColor.g), specularColor.b);

    SrfInfo.Reflectance0 = specularColor.rgb;
    // Anything less than 2% is physically impossible and is instead considered to be shadowing. Compare to "Real-Time-Rendering" 4th editon on page 325.
    SrfInfo.Reflectance90 = clamp(reflectance * 50.0, 0.0, 1.0) * float3(1.0, 1.0, 1.0);

    return SrfInfo;
}

// The following equation models the Fresnel reflectance term of the spec equation (aka F())
// Implementation of fresnel from "An Inexpensive BRDF Model for Physically based Rendering" by Christophe Schlick
// (https://www.cs.virginia.edu/~jdl/bib/appearance/analytic%20models/schlick94b.pdf), Equation 15
float3 SchlickReflection(float VdotH, float3 Reflectance0, float3 Reflectance90)
{
    return Reflectance0 + (Reflectance90 - Reflectance0) * pow(clamp(1.0 - VdotH, 0.0, 1.0), 5.0);
}

// Visibility = G(v,l,a) / (4 * (n,v) * (n,l))
// see https://google.github.io/filament/Filament.md.html#materialsystem/specularbrdf/geometricshadowing(specularg)
float SmithGGXVisibilityCorrelated(float NdotL, float NdotV, float AlphaRoughness)
{
    float a2 = AlphaRoughness * AlphaRoughness;

    float GGXV = NdotL * sqrt(max(NdotV * NdotV * (1.0 - a2) + a2, 1e-7));
    float GGXL = NdotV * sqrt(max(NdotL * NdotL * (1.0 - a2) + a2, 1e-7));

    return 0.5 / (GGXV + GGXL);
}

AngularInfo GetAngularInfo(float3 PointToLight, float3 Normal, float3 View)
{
    float3 n = normalize(Normal);       // Outward direction of surface point
    float3 v = normalize(View);         // Direction from surface point to camera
    float3 l = normalize(PointToLight); // Direction from surface point to light
    float3 h = normalize(l + v);        // Direction of the vector between l and v

    AngularInfo info;
    info.NdotL = clamp(dot(n, l), 0.0, 1.0);
    info.NdotV = clamp(dot(n, v), 0.0, 1.0);
    info.NdotH = clamp(dot(n, h), 0.0, 1.0);
    info.LdotH = clamp(dot(l, h), 0.0, 1.0);
    info.VdotH = clamp(dot(v, h), 0.0, 1.0);

    return info;
}

// The following equation(s) model the distribution of microfacet normals across the area being drawn (aka D())
// Implementation from "Average Irregularity Representation of a Roughened Surface for Ray Reflection" by T. S. Trowbridge, and K. P. Reitz
// Follows the distribution function recommended in the SIGGRAPH 2013 course notes from EPIC Games [1], Equation 3.
float NormalDistribution_GGX(float NdotH, float AlphaRoughness)
{
    float a2 = AlphaRoughness * AlphaRoughness;
    float f = NdotH * NdotH * (a2 - 1.0) + 1.0;
    return a2 / (PI * f * f);
}

void BRDF(in float3                 PointToLight,
    in float3                 Normal,
    in float3                 View,
    in SurfaceReflectanceInfo SrfInfo,
    out float3                SpecContrib,
    out float                 NdotL)
{
    AngularInfo angularInfo = GetAngularInfo(PointToLight, Normal, View);

    SpecContrib = float3(0.0, 0.0, 0.0);
    NdotL = angularInfo.NdotL;
    // If one of the dot products is larger than zero, no division by zero can happen. Avoids black borders.
    if (angularInfo.NdotL > 0.0 || angularInfo.NdotV > 0.0)
    {
        //           D(h,a) * G(v,l,a) * F(v,h,f0)
        // f(v,l) = -------------------------------- = D(h,a) * Vis(v,l,a) * F(v,h,f0)
        //               4 * (n,v) * (n,l)
        // where
        //
        // Vis(v,l,a) = G(v,l,a) / (4 * (n,v) * (n,l))

        // It is not a mistake that AlphaRoughness = PerceptualRoughness ^ 2 and that
        // SmithGGXVisibilityCorrelated and NormalDistribution_GGX then use a2 = AlphaRoughness ^ 2.
        // See eq. 3 in https://blog.selfshadow.com/publications/s2013-shading-course/karis/s2013_pbs_epic_notes_v2.pdf
        float AlphaRoughness = SrfInfo.PerceptualRoughness * SrfInfo.PerceptualRoughness;
        float  D = NormalDistribution_GGX(angularInfo.NdotH, AlphaRoughness);
        float  Vis = SmithGGXVisibilityCorrelated(angularInfo.NdotL, angularInfo.NdotV, AlphaRoughness);
        float3 F = SchlickReflection(angularInfo.VdotH, SrfInfo.Reflectance0, SrfInfo.Reflectance90);

        SpecContrib = F * Vis * D;
    }
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

    //float3 eyeDir = normalize(g_ConstantsCB.CameraPos.xyz - Pos);

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
            ray.Direction = rayDir;
            float shading = saturate(CastShadow(ray, Recursion).Shading);

            col += Color * g_ConstantsCB.LightColor[i].rgb * NdotL * shading;
            //These commented lines and the eyeDir above give crappy specular highlights
            //float3 halfVec = normalize(eyeDir + rayDir);
            //float specularLight = pow(saturate(dot(pertbNorm, halfVec)), 250);
            //col += specularLight;
        }
        col += Color * g_ConstantsCB.Darkness;
    }
    Color = col * (1.0 / float(NUM_LIGHTS)) + g_ConstantsCB.AmbientColor.rgb;
}

void LightingPass(inout float3 Color, float3 Pos, float3 Norm, float3 pertbNorm, uint Recursion, float4 physicalInfo)
{
    RayDesc ray;
    float3  col = float3(0.0, 0.0, 0.0);

    // Add a small offset to avoid self-intersections.
    ray.Origin = Pos + Norm * SMALL_OFFSET;
    ray.TMin = 0.0;

    float3 view = g_ConstantsCB.CameraPos.xyz - Pos;
    SurfaceReflectanceInfo surfInfo = GetSurfaceReflectance(Color, physicalInfo);

    for (int i = 0; i < NUM_LIGHTS; ++i)
    {
        // Limit max ray length by distance to light source.
        ray.TMax = distance(g_ConstantsCB.LightPos[i].xyz, Pos) * 1.01;

        float3 rayDir = normalize(g_ConstantsCB.LightPos[i].xyz - Pos);
        float  NdotL;// = max(0.0, dot(pertbNorm, rayDir));
        float3 SpecContrib;
            BRDF(rayDir, pertbNorm, view, surfInfo,
                SpecContrib, NdotL);

        // Optimization - don't trace rays if NdotL is zero or negative
        if (NdotL > 0.0)
        {
            // Cast multiple rays that are distributed within a cone.
            ray.Direction = rayDir;
            float shading = saturate(CastShadow(ray, Recursion).Shading);

            col += (Color + SpecContrib) * g_ConstantsCB.LightColor[i].rgb * NdotL * shading;
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

void LightAndShadePhysicalUV
(
    inout PrimaryRayPayload payload, float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D colorTexture, Texture2D normalTexture, Texture2D physicalTexture, SamplerState colorSampler, SamplerState normalSampler,
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
    float4 physical = physicalTexture.SampleLevel(normalSampler, uv, mip);

    // Apply lighting.
    float3 rayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
    LightingPass(payload.Color, rayOrigin, normal, pertNormal, payload.Recursion + 1, physical);
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

void LightAndShadePhysical(
    inout PrimaryRayPayload payload, float3 barycentrics,
    CubeAttribVertex posX, CubeAttribVertex posY, CubeAttribVertex posZ,
    Texture2D colorTexture, Texture2D normalTexture, Texture2D physicalTexture, SamplerState colorSampler, SamplerState normalSampler)
{

    // Calculate texture coordinates.
    float2 uv = posX.uv.xy * barycentrics.x +
        posY.uv.xy * barycentrics.y +
        posZ.uv.xy * barycentrics.z;

    LightAndShadePhysicalUV(payload, barycentrics,
        posX, posY, posZ,
        colorTexture, normalTexture, physicalTexture, colorSampler, normalSampler,
        uv);
}

void LightAndShadeReflectiveUV
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

    float3 baseColor = colorTexture.SampleLevel(colorSampler, uv, mip).rgb;
    float4 physical = physicalTexture.SampleLevel(normalSampler, uv, mip);
    float roughness = physical.g;

    // Reflect from the normal
    RayDesc ray;
    ray.Origin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent() + normal * SMALL_OFFSET;
    ray.TMin = 0.0;
    ray.TMax = 100.0;
    ray.Direction = reflect(WorldRayDirection(), pertNormal);
    float3 reflectedColor = CastPrimaryRay(ray, payload.Recursion + 1).Color;

    // Calculate final color
    payload.Color = baseColor * roughness + reflectedColor * (1.0f - roughness);

    // Apply lighting.
    float3 rayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
    LightingPass(payload.Color, rayOrigin, normal, pertNormal, payload.Recursion + 1, physical);

    payload.Depth = RayTCurrent();
}

void LightAndShadeReflective
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

    LightAndShadeReflectiveUV(payload, barycentrics,
        posX, posY, posZ,
        colorTexture, normalTexture, physicalTexture,
        colorSampler, normalSampler,
        uv);
}