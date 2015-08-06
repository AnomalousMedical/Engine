#version 120

uniform mat4 worldViewProj;

//Input
attribute vec4 vertex; //Vertex
attribute vec2 uv0; //Uv Coord

//Output
varying vec2 texCoords;

//----------------------------------
//Shared Vertex Program
//----------------------------------
void main(void)
{
	texCoords = uv0;
	gl_Position = worldViewProj * vertex;
}