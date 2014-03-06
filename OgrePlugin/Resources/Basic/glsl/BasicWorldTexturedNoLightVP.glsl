#version 120

uniform mat4 worldViewProj;	//The world view projection matrix

//Input
attribute vec4 vertex; //Vertex
attribute vec2 uv0; //Uv Coord

//Output
varying vec2 texCoords;

//----------------------------------
//Measurement Vertex Program
//----------------------------------
void main(void)
{
	texCoords = uv0;

	// Transform the current vertex from object space to clip space
	gl_Position = worldViewProj * vertex;
}