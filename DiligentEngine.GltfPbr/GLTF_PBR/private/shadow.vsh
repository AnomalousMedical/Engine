struct GLTF_VS_Input
{
    float3 Pos     : ATTRIB0;
    float3 Normal  : ATTRIB1;
    float2 UV0     : ATTRIB2;
    float2 UV1     : ATTRIB3;
    float4 Joint0  : ATTRIB4;
    float4 Weight0 : ATTRIB5;
};

cbuffer cbTransform
{
    float4x4 g_WorldViewProj;
};

void main(in  GLTF_VS_Input  VSIn,
    out float4 Pos   : SV_POSITION)
{
    Pos = mul(float4(VSIn.Pos, 1.0), g_WorldViewProj);
}
