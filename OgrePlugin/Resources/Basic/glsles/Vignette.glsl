precision mediump float;

uniform vec4 bgColor;
varying vec2 texCoords;

//----------------------------------
//Measurement Fragment Program
//----------------------------------
void main(void)
{
    vec2 position = texCoords - vec2(0.5);
    float len = length(position);
    gl_FragColor = vec4( bgColor.rgb * (1.0 - len), 1.0 );
}