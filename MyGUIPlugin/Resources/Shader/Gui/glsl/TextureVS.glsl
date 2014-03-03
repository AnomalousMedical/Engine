//Input
attribute vec4 vertex; //Vertex
attribute vec2 uv0; //Uv Coord
attribute vec4 colour; //Color

//Output
varying vec2 texCoords;
varying vec4 passColor;

//----------------------------------
//Measurement Vertex Program
//----------------------------------
void main(void)
{
	texCoords = uv0;
	passColor = colour;

	// Transform the current vertex from object space to clip space
	gl_Position = vertex;
}