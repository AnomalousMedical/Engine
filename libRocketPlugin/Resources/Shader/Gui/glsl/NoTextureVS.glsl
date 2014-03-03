//Input
uniform mat4 elementWorldViewProj;	//The world view projection matrix

attribute vec4 vertex; //Vertex
attribute vec4 colour; //Color

//Output
varying vec4 passColor;

//----------------------------------
//Measurement Vertex Program
//----------------------------------
void main(void)
{
	passColor = colour;
	gl_Position = elementWorldViewProj * vertex;
}