#version 120

uniform sampler2D colorTexture;  //The color map
uniform vec4 bgColor;			 //The background color

varying vec2 texCoords;

//----------------------------------
//Measurement Fragment Program
//----------------------------------
void main(void)
{
	vec4 texColor = texture2D(colorTexture, texCoords.xy);
	gl_FragColor.rgb = (texColor.rgb * texColor.a) + (bgColor.rgb * (1 - texColor.a));
	gl_FragColor.a = 1.0f;
}