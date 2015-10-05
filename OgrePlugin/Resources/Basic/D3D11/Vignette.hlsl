void main( uniform float4 bgColor,
		   uniform float resX,
		   uniform float resY,

		   float4 fragCoord : SV_POSITION,
			
			out float4 outColor : SV_TARGET ) 
{
	float2 resolution = float2(resX, resY);

    //determine origin
    float2 position = (fragCoord.xy / resolution.xy) - float2(0.5, 0.5);

    //determine the vector length of the center position
    float len = length(position);

    //show our length for debugging
    outColor = float4( bgColor.rgb * (1.0 - len), 1.0 );
}