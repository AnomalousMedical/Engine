uniform mat4 worldViewProj;	//The world view projection matrix

//Input
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

	// Transform the current vertex from object space to clip space
	gl_Position = worldViewProj * vertex;
}