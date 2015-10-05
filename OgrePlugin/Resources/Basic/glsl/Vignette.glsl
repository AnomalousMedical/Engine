#version 120

uniform vec4 bgColor;
uniform float resX;
uniform float resY;

//----------------------------------
//Measurement Fragment Program
//----------------------------------
void main(void)
{
	vec2 resolution = vec2(resX, resY);

    //determine origin
    vec2 position = (gl_FragCoord.xy / resolution.xy) - vec2(0.5);

    //determine the vector length of the center position
    float len = length(position);

    //show our length for debugging
    gl_FragColor = vec4( bgColor.rgb * (1.0 - len), 1.0 );
}