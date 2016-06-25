#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec4 fPosition;
uniform sampler2D pixels;		// texture sampler

uniform vec4 ambientCol;
uniform vec4 materialCol;
uniform vec4 specularCol;
uniform vec4 lightPosition;

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	vec4 lightdirection = fPosition - lightPosition;
    outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
    outputColor *= ambientCol;
}