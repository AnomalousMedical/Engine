#version 120

uniform sampler2D colorTexture;  //The color map

varying vec2 texCoords;

//----------------------------------
//Measurement Fragment Program
//----------------------------------
void main(void)
{
	gl_FragColor = texture2D(colorTexture, texCoords.xy);
}