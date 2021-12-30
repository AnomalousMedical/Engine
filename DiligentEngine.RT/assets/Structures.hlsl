struct SpriteFrame {
    float2 uvs[4];
};

struct BlasInstanceData
{
    int baseTexture;
    int normalTexture;
    int physicalTexture;
    int emissiveTexture;
    float pad1;
    float pad2;
    float pad3;
    float pad4;
};

struct CubeAttribVertex {
  float4 uv;
  float4 tangent;
  float4 binormal;
  float4 normal;
};

struct PrimaryRayPayload
{
    float3 Color;
    float  Depth;
    uint   Recursion;
};

struct EmissiveRayPayload
{
    float3 Color;
    uint   Recursion;
};

struct ShadowRayPayload
{
    float  Shading;   // 0 - fully shadowed, 1 - fully in light, 0..1 - for semi-transparent objects
    uint   Recursion; // Current recusrsion depth
};

struct Constants
{
    // Camera world position
    float4   CameraPos;

    // Near and far clip plane distances
    float2   ClipPlanes;
    float2   Padding0;

    // Camera view frustum corner rays
    float4   FrustumRayLT;
    float4   FrustumRayLB;
    float4   FrustumRayRT;
    float4   FrustumRayRB;


    // The number of shadow PCF samples
    int      ShadowPCF; 
    // Maximum ray recursion depth
    int      MaxRecursion;
    float    Darkness;
    float1   Padding2;

    // Light properties
    float4  AmbientColor;
    float4  LightPos[NUM_LIGHTS];
    float4  LightColor[NUM_LIGHTS];

    //Sky properties
    float4 Pallete[6];
};

struct SurfaceReflectanceInfo
{
    float  PerceptualRoughness;
    float3 Reflectance0;
    float3 Reflectance90;
    float3 DiffuseColor;
};

struct AngularInfo
{
    float NdotL;   // cos angle between normal and light direction
    float NdotV;   // cos angle between normal and view direction
    float NdotH;   // cos angle between normal and half vector
    float LdotH;   // cos angle between light direction and half vector
    float VdotH;   // cos angle between view direction and half vector
};

// Instance mask.
#define OPAQUE_GEOM_MASK      0x01
#define TRANSPARENT_GEOM_MASK 0x02

// Ray types
#define HIT_GROUP_STRIDE  3
#define PRIMARY_RAY_INDEX 0
#define SHADOW_RAY_INDEX  1
#define EMISSIVE_RAY_INDEX 2


// Small offset between ray intersection and new ray origin to avoid self-intersections.
# define SMALL_OFFSET 0.0001

