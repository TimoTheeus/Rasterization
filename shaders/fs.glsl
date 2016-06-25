﻿#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
uniform sampler2D pixels;		// texture sampler

uniform vec4 ambientCol;
uniform vec4 materialCol;
uniform vec4 specularCol;

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
   // outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
    outputColor = ambientCol;
}