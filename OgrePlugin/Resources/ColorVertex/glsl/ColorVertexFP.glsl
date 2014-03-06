#version 120

varying vec4 passColor;

//----------------------------------
//Measurement Fragment Program
//----------------------------------
void main(void)
{
	gl_FragColor = passColor;
}