#version 120

uniform vec4 alpha;

varying vec4 passColor;

//----------------------------------
//Measurement Fragment Program
//----------------------------------
void main(void)
{
	gl_FragColor = passColor;
	gl_FragColor.a = alpha.a;
}