#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
uniform sampler2D pixels;		// texture sampler
uniform float ambientCoefficient=0.5f;

// shader output
out vec4 outputColor;
out vec4 ambientColor;

// fragment shader
void main()
{
    outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
    ambientColor= outputColor*ambientCoefficient;
}